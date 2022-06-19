using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Player : MonoBehaviour
    {
        public StackHolder Holder;
        [SerializeField] private ScoreManager _scoreManager; 
        [SerializeField] private  float _maxMoveSpeed = 4f;
        [Range(0,0.9f),SerializeField] private  float _weightMaxSlowdownPercentage = 0.3f;
        [SerializeField] private  int _stackSizeForMaxSlowdown = 5;
        [SerializeField] private Transform _movingRoot;
        [SerializeField] private float _pushBackStrenght = 5f;
        [SerializeField] private float _interactionSlowdown = 0.3f;
        [SerializeField] private float _interactionSlowdownDuration = 1f;
        private float _slowdownTime = 0f;
        private float _slowdownMaxTime = 0f;
        private float _weightSlowdownPercentage;
        private bool _isGameFinished;
        private float _pushLength;
        private float _targetPushLength;
        
        private void Awake()
        {
            Holder = GetComponentInChildren<StackHolder>();
            Holder.Player = this;

            Holder.OnPriceChanged += OnPriceChange;
            Holder.SizeChanged += OnStackSizeChanged;
        }
        private void Update()
        {
            _slowdownTime -= Time.deltaTime;
            if (!_isGameFinished)
            {
                MoveForward();
                return;
            }
        }
        public void SetMove(bool canMove) => _isGameFinished = !canMove;
        public void SetCursorFollow(bool follow)
        {
            var moveToC = GetComponentInChildren<MoveToCursor>();
            if (moveToC) moveToC.enabled = follow;
        }
        private void MoveForward()
        {
            Vector3 vec = _movingRoot.forward * ((_maxMoveSpeed - _weightSlowdownPercentage*_maxMoveSpeed) * Time.deltaTime);
            
            // Slowdown easing out
            if (_slowdownTime > 0)
            {
                vec *= (1 - Mathf.Lerp(0, _interactionSlowdown,_slowdownTime/_slowdownMaxTime));
                
            }
            // Applies push
            if (_pushLength > 0)
            {
                float amount =  Mathf.Lerp(0, _targetPushLength, _pushLength / _targetPushLength) * Time.deltaTime;
                vec.z -= amount;
                _pushLength -= amount;
            }

            
            _movingRoot.position += vec;
        }
        public void ApplySlowdown(float seconds)
        {
            _slowdownMaxTime = seconds;
            _slowdownTime = seconds;
        }

        public void ApplyPushBack(float length)
        {
            _pushLength = length;
            _targetPushLength = length;
        }

        private void OnPriceChange(Stackable item, int value)
        {
            _scoreManager?.IndicateScoreChange(item.transform.position, value);

        }
        private void OnStackSizeChanged(Stackable item,int currentSize, bool addedToStack)
        {
            _weightSlowdownPercentage =
                _weightMaxSlowdownPercentage * Mathf.Clamp((float)currentSize / _stackSizeForMaxSlowdown,0f,1f );
        }
        public void OnCollideWithObstacle(Obstacle obstacle, Stackable item)
        {
            if (obstacle.AppliesSlowdown) ApplySlowdown(_interactionSlowdownDuration);

            switch (obstacle)
            {
                case Trap trap:
                    Holder.RemoveAfter(item);
                    if(trap.HasPushBack) ApplyPushBack(_pushBackStrenght);
                    break;
                case SellerObstacle:
                    _scoreManager?.ChangePlayerBalance(item.GetPrice());
                    Holder.RemoveFromStack(item);
                    item.SetToDestroy();
                    break;
            }
        }
        public List<Stackable> GetMostValuable(int count)
        {
            List<Stackable> items = Holder.GetSorted();
            items.Reverse();
            if (count >= items.Count) return items;
            items.RemoveRange(count, items.Count - count);
            return items;
        }

        public int GetScore() => _scoreManager.GetScore();
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private  float _maxMoveSpeed = 4f;
        [Range(0,0.9f),SerializeField] private  float _weightMaxSlowdownPercentage = 0.3f;
        [SerializeField] private  int _stackSizeForMaxSlowdown = 5;

        [SerializeField] private Transform _movingRoot;
        [SerializeField] private TMPro.TMP_Text _totalPriceTextField;
        [SerializeField] private GameObject _priceIndicator;
        [SerializeField] private float _pushBackStrenght = 5f;
        [SerializeField] private float _interactionSlowdown = 0.3f;
        [SerializeField] private float _interactionSlowdownDuration = 1f;
        private Transform _holderTransform;
        private float _slowdownTime = 0f;
        private float _slowdownMaxTime = 0f;
        private float _weightSlowdownPercentage;
        private bool _isGameFinished;
        private bool _stopHolder;
        private float _pushLength;
        private int _totalPrice;
        private float _targetPushLength;
        private StackHolder _stackHolder;
        private Vector3 _lastPriceChangePos;
        private PriceIndicator _lastIndicator;

        private void Awake()
        {
            _stackHolder = GetComponentInChildren<StackHolder>();
            _holderTransform = _stackHolder.transform;
            _stackHolder.OnPriceChanged += OnPriceChange;
            _stackHolder.SizeChanged += OnStackSizeChanged;
        }
        private void Update()
        {
            _slowdownTime -= Time.deltaTime;
            if (!_isGameFinished)
            {
                MoveForward();
                return;
            }
            if (!_stopHolder)
            {
                Vector3 vec = _movingRoot.forward * (_maxMoveSpeed * Time.deltaTime);
                _holderTransform.position += vec;
            }
        }
        public void ReachedFinish()
        {
            _isGameFinished = true;
            PlayerBalance.GetInstance().Add(_totalPrice);
            PlayerBalance.GetInstance().Save();
        }
        public void ReachedFullStop()
        {
            _stopHolder = true;
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
            _totalPrice += value;
            ShowPrice();
            IndicatePriceChange(item,value);
        }
        private void ShowPrice() => _totalPriceTextField.text = $"{_totalPrice} $";
        private void IndicatePriceChange(Stackable item, int value, bool forceNew = false)
        {
            Vector3 itemPos = item.transform.position;
            if ((_lastPriceChangePos - itemPos).magnitude > 4 || !_lastIndicator || forceNew || Math.Sign(_lastIndicator.GetValue()) != Math.Sign(value))
            {
                _lastIndicator = Instantiate(_priceIndicator, itemPos, Quaternion.identity).GetComponent<PriceIndicator>();
                _lastIndicator.SetValue(value);
            }
            else
            {
                _lastIndicator.AddValue(value);
            }
            _lastPriceChangePos = itemPos;
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
                    _stackHolder.RemoveAfter(item);
                    if(trap.HasPushBack) ApplyPushBack(_pushBackStrenght);
                    break;
                case SellerObstacle:
                    PlayerBalance.GetInstance().Add(item.GetPrice());
                    _stackHolder.RemoveFromStack(item);
                    item.SetToDestroy();
                    break;
            }
        }
        
    }
}

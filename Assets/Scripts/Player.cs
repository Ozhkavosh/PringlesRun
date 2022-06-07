using UnityEngine;

namespace Assets.Scripts
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 2f;
        [SerializeField] private Transform _orientation;
        [SerializeField] private Transform _movingRoot;
        [SerializeField] private StackHolder _stackHolder;
        [SerializeField] private Camera _playerCamera;
        [SerializeField] private float _interactionSlowdown = 0.3f;
        private Transform _holderTransform;
        private float _slowdownTime = 0f;
        private float _slowdownMaxTime = 0f;
        private bool _isGameFinished;
        private bool _stopHolder;
        private float _pushLength;
        private float _targetPushLength;
        void Awake()
        {
            _holderTransform = _stackHolder.transform;
        }
        void Update()
        {
            _slowdownTime -= Time.deltaTime;
            if (!_isGameFinished)
            {
                MoveForward();
                return;
            }
            if (!_stopHolder)
            {
                Vector3 vec = _orientation.forward * (_moveSpeed * Time.deltaTime);
                _holderTransform.position += vec;
            }
        }
        public void FinishGame()
        {
            _isGameFinished = true;
        }
        public void StopHand()
        {
            _stopHolder = true;
        }
        void MoveForward()
        {
            Vector3 vec = _orientation.forward * (_moveSpeed*Time.deltaTime);
            if (_slowdownTime > 0)
            {
                vec *= (1 - Mathf.Lerp(0, _interactionSlowdown,_slowdownTime/_slowdownMaxTime));
                
            }

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
    }
}

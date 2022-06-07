using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class Stackable : MonoBehaviour
    {
        
        [SerializeField] public Transform StackPosition;
        [SerializeField] public Transform RootTransform;
        [SerializeField] public Transform BackPosition;
        [SerializeField] private int _basePrice;
        [Range(0.05f, 1f), SerializeField]
        private float _followSpeed = 0.3f;

        public StackHolder Holder { get; set; }
        public UnityEngine.Events.UnityEvent<Stackable,int> PriceChangeEvent;
        public UnityEngine.Events.UnityEvent<Collider> TriggerEntered;
        public ItemType Type;

        private Transform _transformToStackOn;
        private Collider _collider;
        private bool _wasStacked;
        private int _price;
        private Rigidbody _rigidbody;
        private bool _awaitsToGetDestroyed;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _price = _basePrice;
            _collider = GetComponent<Collider>();
        }
        void Update()
        {
            if (!_wasStacked || _awaitsToGetDestroyed) return;
            Vector3 newPos = _transformToStackOn.position;
            newPos -= BackPosition.localPosition;
            RootTransform.position = Vector3.Lerp(RootTransform.position, newPos, _followSpeed);
        }
        public void AddPrice(int price)
        {
            PriceChangeEvent.Invoke(this, price);
            _price += price;
        }
        public int GetPrice()
        {
            return _price;
        }
        public void StackOn(Transform transform)
        {
            _transformToStackOn = transform;
            _wasStacked = true;
            _rigidbody.isKinematic = true;
            _collider.isTrigger = true;
        }
        public void Unstack()
        {
            _transformToStackOn = null;
            _wasStacked = false;
            _rigidbody.isKinematic = false;
            _collider.isTrigger = false;
        }

        public void SetToDestroy()
        {
            Debug.Log("Set to destroy",this);
            _awaitsToGetDestroyed = true;
        }

        public bool PreparingToDestroy()
        {
            return _awaitsToGetDestroyed;
        }
        public bool IsStacked()
        {
            return _wasStacked;
        }
        private void OnTriggerEnter(Collider other)
        {
            if ( !_wasStacked || _awaitsToGetDestroyed) return;
            TriggerEntered.Invoke(other);
        }
    }
}


using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class PriceIndicator : MonoBehaviour
    {
        public Action<PriceIndicator> LifetimeEnded = (a)=>{};

        [SerializeField] private TMPro.TMP_Text _textField;
        [SerializeField] private float _maxLifetime = 1f;
        [SerializeField] private Animation _anim;
        private float _lifetime;
        private int _price;
        private Transform _cameraTransform;
        private void Start()
        {
            _cameraTransform = Camera.main.transform;
            transform.LookAt(2*transform.position- _cameraTransform.position);
            _lifetime = _maxLifetime;
        }
        private void Update()
        {
            _lifetime -= Time.deltaTime;
            if (!Alive())
            {
                LifetimeEnded.Invoke(this);
                return;
            }
            _textField.alpha =Mathf.Clamp(_lifetime/(_maxLifetime / 2f),0,1);
        }
        public void SetValue( int value)
        {
            _lifetime = _maxLifetime;
            _price = value;
            _textField.text = value + "$";
            if (_price < 0)
            {
                _textField.color = Color.red;
            }
            _anim?.Play();
        }

        public bool Alive()
        {
            return _lifetime > 0;
        }
        
        public void AddValue( int value)
        {
            SetValue(_price + value);
        }

        public GameObject CreateCopyAt(Vector3 position)
        {
            GameObject obj = Instantiate(gameObject, position, Quaternion.identity);
            obj.transform.SetParent(transform.parent);
            obj.SetActive(true);
            return obj;
        }
        public int GetValue() => _price;
    }
}

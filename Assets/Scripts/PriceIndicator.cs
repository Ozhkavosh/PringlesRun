
using UnityEngine;

namespace Assets.Scripts
{
    public class PriceIndicator : MonoBehaviour
    {
        [SerializeField] private TMPro.TMP_Text _textField;
        [SerializeField] private float _lifetime = 1f;
        [SerializeField] private Animation _anim;
        private float _timer;
        private int _price;
        private Transform _cameraTransform;
        private void Start()
        {
            _cameraTransform = Camera.main.transform;
            transform.LookAt(2*transform.position- _cameraTransform.position);

        }
        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer > _lifetime) Destroy(gameObject);
            if(_timer > _lifetime / 2)
            {
                _textField.alpha = 1f- (_timer - _lifetime/2f)/(_lifetime / 2f);
            }
            if ((transform.position - _cameraTransform.position).z < 0) Destroy(gameObject);
        }
        public void SetValue( int value)
        {
            _price = value;
            _textField.text = value + "$";
            if (_price < 0)
            {
                _textField.color = Color.red;
            }
            _anim.Play();
        }

        
        public void AddValue( int value)
        {
            _price += value;
            _lifetime += 1;
            _textField.text = _price + "$";
            _anim.Play();
        }
        public int GetValue() => _price;
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class StackHolder : MonoBehaviour
    {
        [SerializeField] private List<Stackable> _stockpile;
        [SerializeField] private TMPro.TMP_Text _totalPriceTextField;
        [SerializeField] private GameObject _priceIndicator;
        private int _totalPrice;
        private Vector3 _lastPriceChangePos;
        private PriceIndicator _lastIndicator;
        private Transform _stackPosTransform;
        private Transform _freeStackTransform;
        private Player _player;

        private void Awake()
        {
            _stockpile = new List<Stackable>();
            _stackPosTransform = transform;
            _freeStackTransform = transform.parent.parent;
            _player = GetComponent<Player>();
        }

        public void AddToStack( Stackable item)
        {
            if (item.IsStacked() || _stockpile.Contains(item)) return;

            IndicatePriceChange(item, item.GetPrice(), true);
            StackItem(item);
            _stockpile.Add(item);
            _stackPosTransform = item.StackPosition;
        }
        public void RemoveFromStack(Stackable item)
        {
            IndicatePriceChange(item, -item.GetPrice());
            UnstackItem(item);
            _stockpile.Remove(item);
            _stackPosTransform = (_stockpile.Count > 0 )? _stockpile[^1].StackPosition : transform;
        }
        public void OnPriceChange(Stackable item, int value) => IndicatePriceChange(item, value);
        private void IndicatePriceChange( Stackable item, int value,bool forceNew = false)
        {
            _totalPrice += value;
            Vector3 itemPos = item.transform.position;
            if ((_lastPriceChangePos - itemPos).magnitude > 4 || !_lastIndicator || forceNew || Math.Sign(_lastIndicator.GetValue())!=Math.Sign(value))
            {
                _lastIndicator = Instantiate(_priceIndicator, itemPos, Quaternion.identity).GetComponent<PriceIndicator>();
                _lastIndicator.SetValue(value);
            }
            else
            {
                _lastIndicator.AddValue(value);
            }
            _lastPriceChangePos = itemPos;
            ShowPrice();
        }
        private void ShowPrice() => _totalPriceTextField.text = $"{_totalPrice} $";
        private void OnTriggerEnter(Collider other)
        {
            _player.ApplySlowdown(1f);
            var stackable = other.GetComponent<Stackable>();
            if (stackable!=null && !stackable.PreparingToDestroy())
            {
                AddToStack(stackable);
            }
        }

        public void OnCollideWithObstacle(Obstacle obstacle, Stackable item)
        {
            if (obstacle.AppliesPushback) _player.ApplyPushBack(5f);

            int index = _stockpile.IndexOf(item);
            item.SetToDestroy();
            Destroy(item.gameObject);
            if (item == _stockpile[^1])
            {
                RemoveFromStack(item);
                return;
            }
            Queue<Stackable> queue = new Queue<Stackable>();
            for (int i = index; i < _stockpile.Count; i++)
            {
                queue.Enqueue(_stockpile[i]);
            }
            while (queue.Count > 0)
            {
                RemoveFromStack(queue.Dequeue());
            }
        }

        private void UnstackItem(Stackable item)
        {
            item.Unstack();
            item.PriceChangeEvent.RemoveListener(OnPriceChange);
            item.TriggerEntered.RemoveListener(OnTriggerEnter);
            item.Holder = null;
            item.transform.parent = _freeStackTransform;
        }
        private void StackItem(Stackable item)
        {
            item.StackOn(_stackPosTransform ? _stackPosTransform: transform);
            item.TriggerEntered.AddListener(OnTriggerEnter);
            item.PriceChangeEvent.AddListener(OnPriceChange);
            item.Holder = this;
            item.transform.parent = transform.parent;
        }
    }
}

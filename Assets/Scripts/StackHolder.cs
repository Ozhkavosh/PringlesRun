using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class StackHolder : MonoBehaviour
    {
        public Action<Stackable, int> OnPriceChanged;
        public Action<Stackable,int, bool> SizeChanged;

        private List<Stackable> _stockpile;
        private Vector3 _lastPriceChangePos;
        private PriceIndicator _lastIndicator;
        private Transform _stackPosTransform;
        private Transform _freeStackTransform;
        public Player Player;

        private void Awake()
        {
            SizeChanged += (stackable,a, b) => { };
            _stockpile = new List<Stackable>();
            _stackPosTransform = transform;
            _freeStackTransform = transform.parent.parent;
        }

        public void AddToStack( Stackable item)
        {
            if (item.IsStacked() || _stockpile.Contains(item)) return;
            
            OnPriceChange(item, item.GetPrice());
            StackItem(item);
            _stockpile.Add(item);
            _stackPosTransform = item.StackPosition;

            SizeChanged.Invoke(item, _stockpile.Count, true);
        }
        public void RemoveFromStack(Stackable item, bool causesPriceChange = true)
        {
            if (!item.IsStacked() || !_stockpile.Contains(item)) return;

            if(causesPriceChange) OnPriceChange(item, -item.GetPrice());

            UnstackItem(item);
            int i = IndexOf(item);
            if (i + 1 < _stockpile.Count)
            {
                _stockpile[i + 1].StackOn(i > 0 ? _stockpile[i - 1].StackPosition : transform);
            }
            _stockpile.Remove(item);
            _stackPosTransform = (_stockpile.Count > 0 )? _stockpile[^1].StackPosition : transform;

            SizeChanged.Invoke(item, _stockpile.Count, false);
        }

        public Stackable[] GetAllAfter(int index)
        {
            Stackable[] items = _stockpile.GetRange(index, _stockpile.Count - index).ToArray();
            return items;
        }
        public Stackable[] GetAllAfter(Stackable item)
        {
            int index = _stockpile.IndexOf(item);
            Stackable[] items = _stockpile.GetRange(index, _stockpile.Count - index).ToArray();
            return items;
        }

        public int IndexOf(Stackable item) => _stockpile.IndexOf(item);
        public bool IsLast(Stackable item) => _stockpile[^1] == item;
        public void OnPriceChange(Stackable item, int value) => OnPriceChanged.Invoke(item, value);

        public void RemoveAfter(Stackable item)
        {
            item.SetToDestroy();
            if (IsLast(item))
            {
                RemoveFromStack(item);
                return;
            }
            foreach (var stackable in GetAllAfter(item))
            {
                RemoveFromStack(stackable);
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            var stackable = other.GetComponent<Stackable>();
            if (stackable!=null && !stackable.PreparingToDestroy())
            {
                AddToStack(stackable);
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
        public List<Stackable> GetSorted()
        {
            List<Stackable> sorted = new List<Stackable>(_stockpile);
            sorted.Sort((a, b) => a.GetPrice().CompareTo(b.GetPrice()) );
            return sorted;
        }
    }
}

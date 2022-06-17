using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Market : MonoBehaviour
{
    public int Capacity;
    [SerializeField] private float _priceMultiplier = 1f;
    [SerializeField] private Transform[] _positions;
    [SerializeField] private float _moveTime = 0.1f;
    private List<MovingObject> _itemsMoving;
    private int _count = 0;
    private bool _movementEnabled;
    private void Awake()
    {
        _itemsMoving = new List<MovingObject>(Capacity);
    }
    public bool TrySell(Stackable stackable)
    {
        if (_count >= Capacity) return false;
        stackable.AddPrice((int)(stackable.GetPrice() * _priceMultiplier));

        stackable.DisableStacking();
        stackable.Holder.RemoveFromStack(stackable, false);
        stackable.SetKinematic(true);
        
        _itemsMoving.Add(new MovingObject(stackable.transform,_positions[_count%_positions.Length].position,_moveTime));
        _count++;
        return true;
    }
    private void Update()
    {
        if (!_movementEnabled) return;
        MoveToPosition();
    }
    public void SetEnabledMove(bool enabled) => _movementEnabled = enabled;
    private void MoveToPosition()
    {
        int i = 0;
        List<MovingObject> remList = new List<MovingObject>(Capacity);
        foreach (var item in _itemsMoving)
        {
            float t = item.t / item.Duration;
            item.Obj.position = Vector3.Lerp(item.Obj.position, item.Destination, t);
            item.t += Time.deltaTime;
            i++;
            if (item.t > item.Duration)
            {
                remList.Add(item);
            }
        }
        foreach (var item in remList)
        {
            _itemsMoving.Remove(item);
        }

    }
    private class MovingObject
    {
        public Transform Obj;
        public Vector3 Destination;
        public float t;
        public float Duration;
        public MovingObject(Transform item,Vector3 dest, float duration)
        {
            this.Obj = item; this.t = 0; this.Duration = duration;
            Destination = dest;
        }
    }
}

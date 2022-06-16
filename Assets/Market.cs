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
    private Stackable[] _stackables;
    private List<MovingObject> _itemsMoving;
    private int _count = 0;
    private void Awake()
    {
        _stackables = new Stackable[Capacity];
        _itemsMoving = new List<MovingObject>(Capacity);
    }
    public bool TrySell(Stackable stackable)
    {
        if (_count >= Capacity) return false;
        _stackables[_count] = stackable;
        stackable.AddPrice((int)(stackable.GetPrice() * _priceMultiplier));
        _itemsMoving.Add(new MovingObject(stackable.transform,_positions[_count%_positions.Length].position,_moveTime));
        _count++;
        return true;
    }
    private void Update()
    {
        MoveToPosition();
    }
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

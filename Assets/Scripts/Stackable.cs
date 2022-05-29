using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[RequireComponent(typeof(Collider))]
public class Stackable : MonoBehaviour
{
    [SerializeField] int basePrice;
    Transform transformToStackOn;
    [SerializeField] public Transform stackPosition;
    [SerializeField] public Transform rootTransform;
    [SerializeField] public Transform backPosition;
    public ItemType Type;
    Collider _collider;
    bool wasStacked = false;
    int _price;
    private void Start()
    {
        _price = basePrice;
        _collider = GetComponent<Collider>();
    }
    void Update()
    {
        if (!wasStacked) return;
        Vector3 newPos = transformToStackOn.position;
        newPos -= backPosition.localPosition;
        rootTransform.position = Vector3.MoveTowards(rootTransform.position, newPos, 0.2f);
    }
    public void AddPrice(int price)
    {
        _price += price;
        print($"Price {basePrice} -> {_price}");
    }
    public int GetPrice()
    {
        return _price;
    }
    public void SetStackOn(Transform transform)
    {
        transformToStackOn = transform;
        wasStacked = true;
        _collider.isTrigger = false;
    }
    public void Unstack()
    {
        wasStacked = false;
        transformToStackOn = null;
    }
    public bool IsStacked()
    {
        return wasStacked;
    }
    private void OnTriggerEnter(Collider other)
    {
        if ( !wasStacked) return;
        Stackable stackable = other.GetComponent<Stackable>();
        if (!stackable) return;
        StackHolder.instance.AddToStack(stackable);
    }
}

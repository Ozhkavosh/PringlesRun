using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Stackable : MonoBehaviour
{
    [SerializeField] int basePrice;
    [SerializeField] public Transform stackPosition;
    [SerializeField] public Transform rootTransform;
    [SerializeField] public Transform backPosition;
    [Range(0.05f,1f),SerializeField] float followSpeed = 0.3f;
    public UnityEngine.Events.UnityEvent<Stackable,int> priceChangeEvent;
    public UnityEngine.Events.UnityEvent<Collider> triggerEntered;
    public ItemType Type;
    Transform transformToStackOn;
    Collider _collider;
    bool wasStacked = false;
    int _price;
    public StackHolder holder { get;  set; }
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
        rootTransform.position = Vector3.Lerp(rootTransform.position, newPos, followSpeed);
    }
    public void AddPrice(int price)
    {
        priceChangeEvent.Invoke(this, price);
        _price += price;
    }
    public int GetPrice()
    {
        return _price;
    }
    public void StackOn(Transform transform)
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
        triggerEntered.Invoke(other);
    }
}

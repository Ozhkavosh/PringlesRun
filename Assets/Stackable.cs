using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[RequireComponent(typeof(Collider))]
public class Stackable : MonoBehaviour
{
    [SerializeField] UnityEngine.Events.UnityEvent<Stackable> OnStacked;
    [SerializeField] int basePrice;
    Transform stackOnTransform;
    [SerializeField] float zStackOffset;
    bool _stacked = false;
    int _price;
    private void Start()
    {
        _price = basePrice;
    }
    void Update()
    {
        if (!_stacked) return;
        Vector3 newPos = new Vector3(stackOnTransform.position.x,stackOnTransform.position.y,stackOnTransform.position.z);
        newPos.z += zStackOffset;
        transform.position = newPos;
        
    }
    void AddPrice(int price)
    {
        _price += price;
    }
    public void SetStackOn(Transform transform)
    {
        stackOnTransform = transform;
        _stacked = true;
        OnStacked.Invoke(this);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!_stacked) return;
        Stackable stackable = other.GetComponent<Stackable>();
        if (stackable == null) return;
        stackable.SetStackOn(transform);
        GetComponent<Collider>().enabled = false;
    }
}

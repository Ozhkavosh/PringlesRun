using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackHolder : MonoBehaviour
{
    public static StackHolder instance;
    [SerializeField] List<Stackable> stockpile;
    Stackable stackedLast;
    private void Awake()
    {
        if(instance is null)
            instance ??= this;
        else
        {
            Destroy(this);
        }
        stockpile = new List<Stackable>(10);
    }
    private void OnTriggerEnter(Collider other)
    {
        
        Stackable stackable = other.GetComponent<Stackable>();
        if (!stackable) return;
        AddToStack(stackable);

    }
    public void AddToStack( Stackable item)
    {
        if (item.wasStacked) return;
        stockpile.Add(item);
        if(stackedLast)
        {
            item.SetStackOn(stackedLast.stackPosition);
        }
        else
        {
            item.SetStackOn(transform);
        }
        stackedLast = item;
    }
}

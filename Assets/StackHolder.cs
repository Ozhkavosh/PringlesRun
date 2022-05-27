using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackHolder : MonoBehaviour
{
    [SerializeField] List<Stackable> stockpile;
    Stackable stacked;
    void Start()
    {
        stockpile = new List<Stackable>(10);
    }

    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        
        Stackable stackable = other.GetComponent<Stackable>();
        if (!stackable) return;
        if (stacked == stackable) return;
        print(stackable.gameObject.name);

        stacked = stackable;
        stackable.SetStackOn(transform);

    }
    public void AddToStack( Stackable item)
    {
        stockpile.Add(item);
    }
}

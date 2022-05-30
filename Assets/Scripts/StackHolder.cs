using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StackHolder : MonoBehaviour
{
    public static StackHolder instance;
    [SerializeField] LinkedList<Stackable> stockpile;
    [SerializeField] TMPro.TMP_Text textField;
    [SerializeField] GameObject priceIndicator;
    int totalPrice;
    private Vector3 lastPriceChangePos;
    private PriceIndicator lastIndicator;
    Stackable stackedLast;
    private void Awake()
    {
        if(instance is null)
            instance ??= this;
        else
            Destroy(this);
        stockpile = new LinkedList<Stackable>();
    }
    private void OnTriggerEnter(Collider other)
    {
        
        Stackable stackable = other.GetComponent<Stackable>();
        if (!stackable) return;
        AddToStack(stackable);
        stackable.triggerEntered.AddListener(OnTriggerEnter);
    }
    public void AddToStack( Stackable item)
    {
        if (item.IsStacked()) return;
        if(stackedLast)
        {
            item.StackOn(stackedLast.stackPosition);
        }
        else
        {
            item.StackOn(transform);
        }
        stockpile.AddLast(item);
        stackedLast = item;
        item.priceChangeEvent.AddListener(OnPriceChange);
        item.holder = this;
        item.transform.parent = transform.parent;
        CalculatePrice();
    }
    public void RemoveFromStack(Stackable item)
    {
        LinkedListNode<Stackable> node = stockpile.Find(item);
        stackedLast = stockpile.Last.Value;

        stockpile.Remove(item);
        item.Unstack();
        item.priceChangeEvent.RemoveListener(OnPriceChange);
        item.triggerEntered.RemoveListener(OnTriggerEnter);
        item.transform.parent = GameObject.Find("Stackables").transform;
    }
    public void CalculatePrice()
    {
        LinkedListNode<Stackable> item = stockpile.First;
        totalPrice = item.Value.GetPrice();
        for (int i = 1; i < stockpile.Count; i++)
        {
            item = item.Next;
            totalPrice += item.Value.GetPrice();
        }
        ShowPrice();
    }
    public void OnPriceChange(Stackable item, int value)
    {
        totalPrice += value;
        Vector3 itemPos = item.transform.position;
        if ( (lastPriceChangePos-itemPos).magnitude >4 | !lastIndicator)
        {
            Vector3 newPos = itemPos + new Vector3(0.3f, 1.3f,0);
            lastIndicator = Instantiate(priceIndicator, newPos, Quaternion.identity).GetComponent<PriceIndicator>();
            lastIndicator.SetValue(value);
        }
        else
        {
            lastIndicator.AddValue(value);
        }


        lastPriceChangePos = itemPos;
        ShowPrice();
    }
    private void ShowPrice() => textField.text = $"{totalPrice} $";
}

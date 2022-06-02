using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StackHolder : MonoBehaviour
{
    [SerializeField] LinkedList<Stackable> stockpile;
    [SerializeField] TMPro.TMP_Text textField;
    [SerializeField] GameObject priceIndicator;
    private int totalPrice;
    private Vector3 lastPriceChangePos;
    private PriceIndicator lastIndicator;
    private Transform stackablesTransform;
    Stackable stackedLast;
    private void Awake()
    {
        stackablesTransform = GameObject.Find("Stackables").transform;
        stockpile = new LinkedList<Stackable>();
    }

    public void AddToStack( Stackable item)
    {
        IndicatePriceChange(item, item.GetPrice(), true);

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
        item.triggerEntered.AddListener(OnTriggerEnter);
        item.priceChangeEvent.AddListener(OnPriceChange);
        item.holder = this;
        item.transform.parent = transform.parent;
    }
    public void RemoveFromStack(Stackable item)
    {
        IndicatePriceChange(item, -item.GetPrice(), true);

        item.Unstack();
        item.priceChangeEvent.RemoveListener(OnPriceChange);
        item.triggerEntered.RemoveListener(OnTriggerEnter);
        item.transform.parent = stackablesTransform;

        LinkedListNode<Stackable> node = stockpile.Find(item);
        LinkedListNode<Stackable> nextNode = node.Next;
        LinkedListNode<Stackable> prevNode = node.Previous;
        if (nextNode!=null)
        {
            if(prevNode != null)
            {
                nextNode.Value.StackOn(prevNode.Value.stackPosition);
            }
            else
            {
                nextNode.Value.StackOn(transform);
            }
                
        }
        stockpile.Remove(node);
        if (stockpile.Count == 0)
        {
            stackedLast = null;
        }
        else
        {
            stackedLast = stockpile.Last.Value;
        }
        
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
        
        IndicatePriceChange(item, value);
        
    }
    private void IndicatePriceChange( Stackable item, int value,bool forceNew = false)
    {
        totalPrice += value;
        Vector3 itemPos = item.transform.position;
        if ((lastPriceChangePos - itemPos).magnitude > 4 | !lastIndicator | forceNew)
        {
            Vector3 newPos = itemPos + new Vector3(0.3f, 1.3f, 0);
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
    private void OnTriggerEnter(Collider other)
    {
        Stackable stackable = other.GetComponent<Stackable>();
        if (stackable)
        {
            AddToStack(stackable);
            return;
        }
        Obstacle obstacle = other.GetComponent<Obstacle>();
        if (obstacle)
        {
            return;
        }
    }
}

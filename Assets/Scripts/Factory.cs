using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    public FactoryBehaviorScript factoryBehavior;
    public int additionalPrice;

    private void OnTriggerEnter(Collider other)
    {
        Stackable item = other.GetComponent<Stackable>();
        if (item == null) return;
        if (item.Type != factoryBehavior.inputType) return;
        Convert(item,factoryBehavior.outputType);
        item.holder.GetComponent<Player>().ApplySlowdown(1f);
    }
    private void Convert(Stackable obj, ItemType type)
    {
        obj.Type = type;
        Transform root = obj.rootTransform;
        Destroy(root.GetChild(0).gameObject );
        GameObject newObject = factoryBehavior.GetObject();
        newObject.transform.parent = root;
        newObject.transform.position = root.position;
        newObject.transform.SetAsFirstSibling();
        obj.AddPrice(additionalPrice);
    }
}

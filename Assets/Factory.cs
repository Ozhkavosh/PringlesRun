using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    [SerializeField] ItemType ConvetingType;
    [SerializeField] ItemType ConvetIntoType;
    public GameObject Log;
    public GameObject Planks;
    public GameObject Crate;

    private void OnTriggerEnter(Collider other)
    {
        Stackable item = other.GetComponent<Stackable>();
        if (item.Type != ConvetingType) return;
        Convert(item,ConvetIntoType);
    }
    private void Convert(Stackable obj, ItemType type)
    {
        Transform root = obj.transform.parent;
        Destroy(root.GetChild(0).gameObject );
        GameObject newObject = Instantiate(GetGFX(type));
        newObject.transform.parent = root;
        newObject.transform.position = root.position;
        newObject.transform.SetAsFirstSibling();
    }
    public GameObject GetGFX(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Log:
                return Log;
            case ItemType.Plank:
                return Planks;
            case ItemType.Crate:
                return Crate;
            default:
                return null;
        }
    }
}

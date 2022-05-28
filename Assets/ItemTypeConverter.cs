
using UnityEngine;

public class ItemTypeConverter: MonoBehaviour
{
    private static ItemTypeConverter _Instance;
    //public static ItemTypeConverter Instance
    //{
    //}
    public GameObject GetGFX(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Log:
                return Log;
            case ItemType.Plank:
                return Log;
            case ItemType.Crate:
                return Log;
            default:
                return null;
        }
    }
    [Header("Grapics for types")]
    public GameObject Log;
    public GameObject Planks;
    public GameObject Crate;
}


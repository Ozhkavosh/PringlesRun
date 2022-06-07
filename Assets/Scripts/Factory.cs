using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Factory : MonoBehaviour
    {
        [SerializeField] private List<ConversionType> _itemConversionTypes;
        [SerializeField] private int _additionalPrice;

        private void OnTriggerEnter(Collider other)
        {
            var item = other.GetComponent<Stackable>();
            if (item == null) return;
            for (int i = 0; i < _itemConversionTypes.Count; i++)
            {
                if (TryToConvert(item, _itemConversionTypes[i]))
                    return;
            }
        }
        private bool TryToConvert(Stackable obj, ConversionType conversionType)
        {
            if (conversionType.ItemInputType != obj.Type) return false;

            // Swaps graphic object to prefab
            obj.Type = conversionType.ItemOutputType;
            Transform root = obj.RootTransform;
            Destroy(root.GetChild(0).gameObject );

            GameObject newObject = conversionType.GetGraphicsObject();
            newObject.transform.parent = root;
            newObject.transform.position = root.position;
            newObject.transform.SetAsFirstSibling();
            obj.AddPrice(_additionalPrice);
            return true;
        }
        [Serializable]
        private struct ConversionType
        {
            [SerializeField] internal ItemType ItemInputType;
            [SerializeField] internal ItemType ItemOutputType;
            [SerializeField] internal GameObject ItemGraphicsPrefab;
            public ConversionType(ItemType inputItemType, ItemType outItemType, GameObject graphicsGameObject)
            {
                ItemGraphicsPrefab = graphicsGameObject;
                ItemInputType = inputItemType;
                ItemOutputType = outItemType;
            }
            internal GameObject GetGraphicsObject()
            {
                return Instantiate(ItemGraphicsPrefab);
            }
        }
    }
}

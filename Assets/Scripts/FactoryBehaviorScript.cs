
using UnityEngine;

[CreateAssetMenu(fileName ="Factory",menuName ="Factory Behavior")]
public class FactoryBehaviorScript : ScriptableObject
{
    public ItemType inputType;
    public ItemType outputType;
    [SerializeField] GameObject GraphicsPrefab;

    public GameObject GetObject()
    {
        return Instantiate( GraphicsPrefab);
    }
}


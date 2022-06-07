
using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName ="Factory",menuName ="Factory Behavior")]
    public class FactoryBehaviorScript : ScriptableObject
    {
        public ItemType InputType;
        public ItemType OutputType;
        [SerializeField] GameObject _graphicsPrefab;

        public GameObject GetObject()
        {
            return Instantiate( _graphicsPrefab);
        }
    }
}


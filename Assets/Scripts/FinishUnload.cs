using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class FinishUnload : MonoBehaviour
    {
        [SerializeField] private Market _market;
        private List<Stackable> _stackables = new (10);
        private void OnTriggerEnter(Collider other)
        {
            Stackable stackable = other.GetComponent<Stackable>();
            if (!stackable | _stackables.Contains(stackable)) return;

            _market.SetEnabledMove(true);
            _market.TrySell(stackable);
            _stackables.Add(stackable);
        }
    }
}

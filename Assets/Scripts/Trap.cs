
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    class Trap : Obstacle
    {
        public bool HasPushBack = true;
        [SerializeField] private float InteractionCooldown = 1f;
        private bool _onCooldown;

        private void OnTriggerEnter(Collider other)
        {
            if (_onCooldown) return;
            Interact(other);
            if (InteractionCooldown > 0 ) StartCoroutine(WaitCooldown());
        }
        private IEnumerator WaitCooldown()
        {
            _onCooldown = true;
            yield return new WaitForSeconds(InteractionCooldown);
            _onCooldown = false;
        }

    }
}

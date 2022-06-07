using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Obstacle : MonoBehaviour
    {
        public bool AppliesPushback;
        [SerializeField] private float _destroyCooldown = 1f;
        [SerializeField] private ParticleSystem _particleSystem;
        private bool _onCooldown = false;
        private void OnTriggerEnter(Collider other)
        {
            Stackable stackable = other.GetComponent<Stackable>();
            if (stackable == null || _onCooldown) return;

            stackable.Holder.OnCollideWithObstacle(this,stackable);
            if (_particleSystem!=null) _particleSystem.Play();

            StartCoroutine(WaitCooldown());

        }

        private IEnumerator WaitCooldown()
        {
            _onCooldown = true;
            yield return new WaitForSeconds(_destroyCooldown);
            _onCooldown = false;
        }
    }
}

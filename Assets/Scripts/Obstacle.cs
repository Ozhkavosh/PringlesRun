using UnityEngine;

namespace Assets.Scripts
{
    public class Obstacle : MonoBehaviour
    {
        public bool AppliesSlowdown;
        [SerializeField] private ParticleSystem _particleSystem;

        protected void Interact(Collider other)
        {
            Stackable stackable = other.GetComponent<Stackable>();
            if (stackable == null) return;
            if(stackable.Holder!=null) stackable.Holder.Player.OnCollideWithObstacle(this, stackable);

            if (_particleSystem != null) _particleSystem.Play();
        }

    }
}

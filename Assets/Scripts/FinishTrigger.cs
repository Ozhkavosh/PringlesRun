using UnityEngine;

namespace Assets.Scripts
{
    public class FinishTrigger : MonoBehaviour
    {

        private void OnTriggerEnter(Collider other)
        {

            Player player = other.GetComponent<Player>();
            if (player is null) return;
            player.ReachedFinish();

        }
    }
}

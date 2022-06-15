using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class FinishUnload : MonoBehaviour
    {
        
        [SerializeField] private Transform _target;
        private List<Stackable> _stackables = new (10);
        private void OnTriggerEnter(Collider other)
        {
            var holder = other.GetComponent<StackHolder>();
            if (holder != null)
            {
                Debug.Log("Player collided with unload trigger",this);
                holder.Player.ReachedFullStop();
                return;
            }
            Stackable stackable = other.GetComponent<Stackable>();
            if (!stackable | _stackables.Contains(stackable)) return;

            _stackables.Add(stackable);
            stackable.Unstack();
            Rigidbody rb =  other.attachedRigidbody;
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.velocity =new Vector3();
            rb.AddForce(Vector3.up * 17, ForceMode.Impulse);
            StartCoroutine(PulseToTarget(rb));
        }
        private IEnumerator PulseToTarget(Rigidbody rigidbody)
        {
            yield return new WaitForSeconds(2);
            Vector3 direction = _target.position-rigidbody.transform.position;
            direction.Normalize();
            rigidbody.velocity = new Vector3();
            rigidbody.AddForce(direction*20, ForceMode.Impulse);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishUnload : MonoBehaviour
{
    List<Stackable> stackables = new List<Stackable>(10);
    [SerializeField]Transform target;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        print(other.gameObject.name);
        Stackable stackable = other.GetComponent<Stackable>();
        if (!stackable | stackables.Contains(stackable)) return;
        stackables.Add(stackable);
        stackable.Unstack();
        Rigidbody rb =  other.attachedRigidbody;
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.velocity =new Vector3();
        rb.AddForce(Vector3.up * 17, ForceMode.Impulse);
        StartCoroutine(TargetMove(rb));
    }
    private IEnumerator TargetMove(Rigidbody rigidbody)
    {
        yield return new WaitForSeconds(2);
        Vector3 direction = target.position-rigidbody.transform.position;
        direction.Normalize();
        rigidbody.velocity = new Vector3();
        rigidbody.AddForce(direction*20, ForceMode.Impulse);
    }
}

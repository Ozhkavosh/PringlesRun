using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForwardScript : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] Transform orientation;
    void Start()
    {
        
    }
    void Update()
    {
        Vector3 vec = orientation.forward * (moveSpeed*Time.deltaTime);
        transform.position+=vec;
    }
}

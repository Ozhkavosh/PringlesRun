using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] Transform followObject;
    [SerializeField] Vector3 shift;
    [SerializeField] float pitch;
    void Start()
    {
        
    }

    void Update()
    {
        Vector3 pos = followObject.position + shift;
        transform.position = pos;
        transform.rotation = Quaternion.Euler(pitch, 0, 0);
    }
}

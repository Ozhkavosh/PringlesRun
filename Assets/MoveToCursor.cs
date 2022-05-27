using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToCursor : MonoBehaviour
{
    float mousePos;
    [SerializeField] float minLim,maxLim;
    void Update()
    {
        mousePos = Input.mousePosition.x;
        mousePos = Mathf.Clamp(mousePos/ Screen.width, 0f, 1f);
        Vector3 newPos = transform.position;
        newPos.x = minLim + (maxLim-minLim)*mousePos;
        transform.position =newPos;
    }
}

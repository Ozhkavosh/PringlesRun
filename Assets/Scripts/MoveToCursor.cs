using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToCursor : MonoBehaviour
{
    new Camera camera;
    Vector3 mousePos;
    [SerializeField] float minLim,maxLim;
    bool leftHold;
    private void Start()
    {
        if (camera is null) camera = Camera.main;
    }
    void Update()
    {
        mousePos = Input.mousePosition;


        if (Input.GetMouseButtonDown(0)) leftHold = true;
        else if (Input.GetMouseButtonUp(0)) leftHold = false;

        if (leftHold) MoveTo();
    }
    void MoveTo()
    {
        Vector3 pos = new Vector3(mousePos.x,mousePos.y,4);
        pos = camera.ScreenToWorldPoint(pos);
        float xPos = Mathf.Clamp(pos.x, minLim, maxLim);
        transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
    }

}

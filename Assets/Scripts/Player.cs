using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] Transform orientation;
    [SerializeField] Transform movingRoot;
    [SerializeField] StackHolder stackHolder;
    [SerializeField] Camera playerCamera;
    [SerializeField] float slowdown = 0.3f;
    private Transform cameraTransform;
    private Transform holderTransform;
    private float slowdownTime = 0f;
    private float slowdownMaxTime = 0f;
    bool isGameFinished;
    bool stopHand;
    void Start()
    {
        cameraTransform = playerCamera.transform;
        holderTransform = stackHolder.transform;
    }
    void Update()
    {
        slowdownTime -= Time.deltaTime;
        if (!isGameFinished)
        {
            MoveForward();
            return;
        }
        if (!stopHand)
        {
            Vector3 vec = orientation.forward * (moveSpeed * Time.deltaTime);
            holderTransform.position += vec;
        }
    }
    public void FinishGame()
    {
        isGameFinished = true;
    }
    public void StopHand()
    {
        stopHand = true;
    }
    void MoveForward()
    {
        Vector3 vec = orientation.forward * (moveSpeed*Time.deltaTime);
        if (slowdownTime > 0)
        {
            vec *= (1 - Mathf.Lerp(0, slowdown,slowdownTime/slowdownMaxTime));
        }
        movingRoot.position += vec;
    }
    public void ApplySlowdown(float seconds)
    {
        slowdownMaxTime = seconds;
        slowdownTime = seconds;
    }
}

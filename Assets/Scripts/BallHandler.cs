using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Rigidbody2D pivot;
    [SerializeField] private float detachDelay = 0.2f;
    [SerializeField] private float respawnDelay = 0.2f;

    private Rigidbody2D currentBallRb;
    private SpringJoint2D currentBallSprintJoint;

    private bool isDragging;
    private Vector2 touchPos;
    private Vector2 worldPos;
    private Camera mainCam; 
    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        currentBallSprintJoint = pivot.GetComponent<SpringJoint2D>();
        SpawnNewBall();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentBallRb == null) 
        {
            return;
        }
        if (!Touchscreen.current.primaryTouch.press.isPressed)
        {
            if (isDragging)
            {
                LaunchBall();
            }
            return;
        }
        isDragging = true;
        currentBallRb.isKinematic = true;
        touchPos = Touchscreen.current.primaryTouch.position.ReadValue();
        worldPos = mainCam.ScreenToWorldPoint(touchPos);
        currentBallRb.position = worldPos;
        
    }

    private void LaunchBall()
    {
        isDragging = false;
        currentBallRb.isKinematic = false;
        currentBallRb = null;

        Invoke(nameof(DetachBall), detachDelay);
    }
    public void DetachBall()
    {
        currentBallSprintJoint.enabled = false;
        Invoke(nameof(SpawnNewBall), respawnDelay);
    }
    public void SpawnNewBall()
    {
        currentBallRb = Instantiate(ballPrefab, pivot.position, Quaternion.identity).GetComponent<Rigidbody2D>();
        currentBallSprintJoint.enabled = true;
        currentBallSprintJoint.connectedBody = currentBallRb;
    }
}

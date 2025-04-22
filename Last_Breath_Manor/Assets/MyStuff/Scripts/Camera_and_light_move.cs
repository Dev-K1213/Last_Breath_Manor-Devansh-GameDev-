using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterMovement : MonoBehaviour
{
    public bool canMove = true;

    


    CharacterController characterController;
    public float moveSpeed = 5f;
    private Vector3 moveDirection;
    private AudioSource footstepAudio;

    public float footstepInterval = 0.5f;
    private float footstepTimer = 0f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        footstepAudio = GetComponent<AudioSource>();
    }

   void Update()
{
    moveDirection.y = -1f; // gravity

    if (canMove)
    {
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

        Vector3 horizontalVelocity = new Vector3(moveDirection.x, 0, moveDirection.z);
        bool isMoving = horizontalVelocity.magnitude > 0.1f;

        if (isMoving && characterController.isGrounded)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f)
            {
                footstepAudio.pitch = UnityEngine.Random.Range(0.7f, 1.4f); // Vary pitch
                footstepAudio.Play();
                footstepTimer = footstepInterval;
            }
        }
        else
        {
            footstepTimer = 0f;
        }
    }
    else
    {
        // Still apply gravity so we don't float or fall through
        characterController.Move(Vector3.down * Time.deltaTime);
    }
}

public void AddMoveInput(float forwardInput, float rightInput)
{
    Vector3 forward = Camera.main.transform.forward;
    Vector3 right = Camera.main.transform.right;

    forward.y = 0f;
    right.y = 0f;

    forward.Normalize();
    right.Normalize();

    moveDirection = (forwardInput * forward) + (rightInput * right);
}


}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterMovement : MonoBehaviour
{
    CharacterController characterController;
    public float moveSpeed = 5f;
    private Vector3 moveDirection;

    // Add reference to AudioSource
    private AudioSource footstepAudio;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        footstepAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        moveDirection.y = -1f;

        
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

        Vector3 horizontalVelocity = new Vector3(moveDirection.x, 0, moveDirection.z);
        bool isMoving = horizontalVelocity.magnitude > 0.1f;

        // Play or stop footstep sound
        if (isMoving)
        {
            if (!footstepAudio.isPlaying)
            {
                footstepAudio.loop = true;
                footstepAudio.Play();
            }
        }
        else
        {
            footstepAudio.Stop();
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

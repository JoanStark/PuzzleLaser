﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private string horizontalInputName;
    [SerializeField] private string VerticalInputName;

    private CharacterController characterController;

    [SerializeField] private float walkSpeed, runSpeed;
    [SerializeField] private float runBuildUp;
    [SerializeField] private KeyCode runKey;

    private float movementSpeed;

    [SerializeField] private AnimationCurve jumpFallOff;
    [SerializeField] private float jumpMultiplier;
    [SerializeField] private KeyCode jumpKey;

    private bool isJumping;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        float vertInput = Input.GetAxis(VerticalInputName);
        float horizInput = Input.GetAxis(horizontalInputName);

        Vector3 forwardMovement = transform.forward * vertInput;
        Vector3 rightMovement = transform.right * horizInput;

        characterController.SimpleMove(Vector3.ClampMagnitude(forwardMovement + rightMovement, 1.0f) * movementSpeed);

        SetMovementSpeed();
        JumpInput();
    }

    private void SetMovementSpeed()
    {
        if (Input.GetKey(runKey))
        {
            movementSpeed = Mathf.Lerp(movementSpeed, runSpeed, Time.deltaTime * runBuildUp);
        }
        else
        {
            movementSpeed = Mathf.Lerp(movementSpeed, walkSpeed, Time.deltaTime * runBuildUp);
        }
    }

    private void JumpInput()
    {
        if(Input.GetKeyDown(jumpKey) && !isJumping)
        {
            isJumping = true;
            StartCoroutine(JumpEvent());
        }
    }

    private IEnumerator JumpEvent()
    {
        characterController.slopeLimit = 90.0f;
        float timeInAir = 0.0f;

        do
        {
            float jumpForce = jumpFallOff.Evaluate(timeInAir);

            characterController.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);
            timeInAir += Time.deltaTime;
            yield return null;
        } while (!characterController.isGrounded && characterController.collisionFlags != CollisionFlags.Above);

        characterController.slopeLimit = 45.0f;
        isJumping = false;
    }
}

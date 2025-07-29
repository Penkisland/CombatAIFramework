using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityCharacterController;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR;

public class LMS_BasicLocomotion : MonoBehaviour
{
    public DAB_LocomotionConfig locomotionConfig;
    private Animator animator;
    private Camera mainCamera;

    private float curSpeed;

    public enum MovementHandleCompType
    {
        None,
        Animator,
        NavMeshAgent,
        CharacterController
    }

    public MovementHandleCompType movementHandleCompType = MovementHandleCompType.Animator;

    public enum LocomotionState
    {
        Idle,
        Walking,
        Running,
        Jumping,
        Falling
    }

    public LocomotionState locomotionState = LocomotionState.Idle;
    public float acceleration;

    void Awake()
    {
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        if (animator == null)
        {
            Debug.LogError("Animator component not found on the GameObject.");
            return;
        }
    }

    void OnAnimatorMove()
    {
        Vector3 moveDirection = CalMovementDir();

        MoveCharacter(moveDirection);
    }

    private Vector3 CalMovementDir()
    {
        Transform cameraTransform = mainCamera.transform;
        Vector3 cameraForward = cameraTransform.forward;
        // calculate the forward direction relative to the camera
        cameraForward.y = 0; // keep the forward direction horizontal
        cameraForward.Normalize();
        Vector3 cameraRight = cameraTransform.right;
        // calculate the right direction relative to the camera
        cameraRight.y = 0; // keep the right direction horizontal
        cameraRight.Normalize();
        Vector3 moveDirection = cameraForward * Input.GetAxis("Vertical") + cameraRight * Input.GetAxis("Horizontal");
        moveDirection.Normalize();

        return moveDirection;
    }

    private void MoveCharacter(Vector3 dir)
    {
        switch (movementHandleCompType)
        {
            case MovementHandleCompType.Animator:
                animator.ApplyBuiltinRootMotion();
                break;
            case MovementHandleCompType.NavMeshAgent:
                HandleNavMeshAgentMovement();
                break;
            case MovementHandleCompType.CharacterController:
                HandleCharacterControllerMovement(dir);
                break;
        }
    }

    private void HandleCharacterControllerMovement(Vector3 dir)
    {
        if (gameObject.TryGetComponent(out CharacterController characterController))
        {
            switch (locomotionState)
            {
                case LocomotionState.Idle:
                    animator.SetFloat("Speed", 0);
                    break;
                case LocomotionState.Walking:
                    curSpeed = Mathf.Lerp(curSpeed, locomotionConfig.walkSpeed, Time.deltaTime * acceleration);
                    characterController.Move(curSpeed * Time.deltaTime * dir);
                    // rotate the character to face the movement direction
                    if (dir != Vector3.zero)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(dir);
                        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * locomotionConfig.rotationSpeed);
                    }
                    if (animator != null)
                    {
                        animator.SetFloat("Speed", curSpeed);
                        // calculate the angle between the character's forward direction and the camera's forward direction
                        // float angle = Vector3.SignedAngle(transform.forward, mainCamera.transform.forward, Vector3.up);
                        // animator.SetFloat("Direction", angle / 180f); // Normalize to [-1, 1]
                        // apply speed factor to animation
                        animator.SetFloat("SpeedFactor", locomotionConfig.walkSpeed / locomotionConfig.walkingThreshold);
                    }
                    break;
                case LocomotionState.Running:
                    curSpeed = Mathf.Lerp(curSpeed, locomotionConfig.runSpeed, Time.deltaTime * acceleration);
                    characterController.Move(curSpeed * Time.deltaTime * dir);
                    // rotate the character to face the movement direction
                    if (dir != Vector3.zero)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(dir);
                        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * locomotionConfig.rotationSpeed);
                    }
                    if (animator != null)
                    {
                        animator.SetFloat("Speed", curSpeed);
                        // calculate the angle between the character's forward direction and the camera's forward direction
                        // float angle = Vector3.SignedAngle(transform.forward, mainCamera.transform.forward, Vector3.up);
                        // animator.SetFloat("Direction", angle / 180f); // Normalize to [-1, 1]
                        // apply speed factor to animation
                        animator.SetFloat("SpeedFactor", locomotionConfig.runSpeed / locomotionConfig.runningThreshold);
                    }
                    break;
                case LocomotionState.Jumping:
                    // Implement jumping logic here
                    break;
                case LocomotionState.Falling:
                    // Implement falling logic here
                    break;
            }
        }
        else
        {
            Debug.LogError("CharacterController component not found on the GameObject.");
        }
    }

    private void HandleNavMeshAgentMovement()
    {
        if (gameObject.TryGetComponent(out NavMeshAgent navMeshAgent))
        {
            switch (locomotionState)
            {
                case LocomotionState.Idle:
                    navMeshAgent.velocity = Vector3.zero;
                    animator.SetFloat("Speed", 0);
                    break;
                case LocomotionState.Walking:
                    curSpeed = Mathf.Lerp(curSpeed, locomotionConfig.walkSpeed, Time.deltaTime * acceleration);
                    navMeshAgent.speed = curSpeed;
                    if (animator != null)
                    {
                        animator.SetFloat("Speed", locomotionConfig.walkSpeed);
                        float angle = Mathf.Atan2(navMeshAgent.desiredVelocity.x, navMeshAgent.desiredVelocity.z) * Mathf.Rad2Deg;
                        animator.SetFloat("Direction", angle / 180f); // Normalize to [-1, 1]
                        animator.SetFloat("SpeedFactor", locomotionConfig.walkSpeed / locomotionConfig.walkingThreshold);
                    }
                    break;
                case LocomotionState.Running:
                    curSpeed = Mathf.Lerp(curSpeed, locomotionConfig.runSpeed, Time.deltaTime * acceleration);
                    navMeshAgent.speed = curSpeed;
                    if (animator != null)
                    {
                        animator.SetFloat("Speed", locomotionConfig.runSpeed);
                        float angle = Mathf.Atan2(navMeshAgent.desiredVelocity.x, navMeshAgent.desiredVelocity.z) * Mathf.Rad2Deg;
                        animator.SetFloat("Direction", angle / 180f); // Normalize to [-1, 1]
                        animator.SetFloat("SpeedFactor", locomotionConfig.runSpeed / locomotionConfig.runningThreshold);
                    }
                    break;
                case LocomotionState.Jumping:
                    // Implement jumping logic here
                    break;
                case LocomotionState.Falling:
                    // Implement falling logic here
                    break;
            }
        }
        else
        {
            Debug.LogError("NavMeshAgent component not found on the GameObject.");
        }
    }

    public void SetLocomotionState(LocomotionState newState)
    {
        locomotionState = newState;
    }
}

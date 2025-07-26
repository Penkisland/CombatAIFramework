using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DA_NewLocomotionConfig", menuName = "ScriptableObjects/DataAsset/DAB_LocomotionConfig", order = 1)]
public class DAB_LocomotionConfig : ScriptableObject
{
    [Header("Locomotion Settings")]
    public float walkSpeed = 2.0f;
    public float runSpeed = 5.0f;
    public float jumpHeight = 2.0f;
    public float gravity = -9.81f;
    public float groundCheckDistance = 0.1f;
    public LayerMask groundLayer;
}

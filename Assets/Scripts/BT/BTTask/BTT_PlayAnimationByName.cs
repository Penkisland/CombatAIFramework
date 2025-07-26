using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class BTT_PlayAnimationByName : Task
{
    Animator animator;
    public string animationName;

    public float animationSpeed = 1f;

    public float blendTime = 0.1f;
    public int layerIndex = 0;

    public override void OnAwake()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on the GameObject.");
        }
    }

    public override void OnStart()
    {
        base.OnStart();
        if (animator != null)
        {
            animator.speed = animationSpeed;
            animator.CrossFade(animationName, blendTime);
        }
        else
        {
            Debug.LogError("Animator component is not initialized.");
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (animator == null || string.IsNullOrEmpty(animationName) ||
            !animator.HasState(layerIndex, Animator.StringToHash(animationName)))
        {
            return TaskStatus.Failure;
        }

        // Check if the animation is playing
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animationName) && 
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            return TaskStatus.Running;
        }

        // If the animation is not playing, return success
        return TaskStatus.Success;
    }
}

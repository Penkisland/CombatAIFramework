using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class BTT_DoMove : Task
{
    NavMeshAgent navMeshAgent;
    public Transform target;
    public enum FocusType { None, Target }
    public FocusType focusType = FocusType.None;
    public float toleranceRadius = 0.1f;
    public float maxWaitTime = 5f;
    private float waitTime;

    public override void OnAwake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent component not found on the GameObject.");
        }
    }

    public override void OnStart()
    {
        base.OnStart();
        if (navMeshAgent != null && target != null)
        {
            navMeshAgent.SetDestination(target.position);
            waitTime = 0f;
        }
        else
        {
            Debug.LogError("NavMeshAgent or target is not initialized.");
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (navMeshAgent == null || target == null)
        {
            return TaskStatus.Failure;
        }

        switch (focusType)
        {
            case FocusType.Target:
                navMeshAgent.transform.LookAt(target);
                break;
            case FocusType.None:
            default:
                // No specific focus, just continue moving towards the target
                break;
        }

        // Check if the agent has reached the destination
        if (Vector3.Distance(navMeshAgent.transform.position, target.position) <= toleranceRadius)
        {
            return TaskStatus.Success;
        }

        // Increment wait time
        waitTime += Time.deltaTime;
        if (waitTime > maxWaitTime)
        {
            return TaskStatus.Failure; // Failed to reach the destination within the time limit
        }

        return TaskStatus.Running; // Still moving towards the target
    }
}

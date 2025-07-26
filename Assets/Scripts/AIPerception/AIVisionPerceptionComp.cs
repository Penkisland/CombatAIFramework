using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIVisionPerceptionComp : MonoBehaviour
{
    [Header("Vision Perception Settings")]
    public float fieldOfViewAngle = 110f;
    public float detectionRange = 20f;
    public float detectionHeight = 1.5f;
    public float lifeSpan = 5f; // How long the perception lasts if not updated
    public LayerMask targetLayer;
    public LayerMask obstacleLayer;
    public float detectionInterval = 0.5f;


    private Coroutine detectionCoroutine;

    public List<Transform> detectedTargets = new List<Transform>();
    // Start is called before the first frame update

    void Start()
    {
        detectionCoroutine = StartCoroutine(DetectionRoutine());
    }

    void OnDestroy()
    {
        if (detectionCoroutine != null)
        {
            StopCoroutine(detectionCoroutine);
        }
    }

    void OnDrawGizmos()
    {
        // Draw a cone to represent the field of view
        Vector3 forward = transform.forward * detectionRange;
        Vector3 leftBoundary = Quaternion.Euler(0, -fieldOfViewAngle / 2, 0) * forward;
        Vector3 rightBoundary = Quaternion.Euler(0, fieldOfViewAngle / 2, 0) * forward;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);

        //Draw a circle with the detection radius at the detection height
        Gizmos.color = Color.blue;
        Vector3 center = transform.position;
        for (int i = 10; i < 360; i += 10)
        {
            float angle = i * Mathf.Deg2Rad;
            Vector3 point = center + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * detectionRange;
            Vector3 prevPoint = center + new Vector3(Mathf.Cos((i - 10) * Mathf.Deg2Rad), 0, Mathf.Sin((i - 10) * Mathf.Deg2Rad)) * detectionRange;
            Gizmos.DrawLine(prevPoint - Vector3.up * detectionHeight / 2, point - Vector3.up * detectionHeight / 2);
            Gizmos.DrawLine(point + Vector3.up * detectionHeight / 2, prevPoint + Vector3.up * detectionHeight / 2);
            if (i % 60 == 0) // Draw every 60 degrees
            {
                Gizmos.DrawLine(point + Vector3.up * detectionHeight / 2, point - Vector3.up * detectionHeight / 2);
            }
        }
    }

    IEnumerator DetectionRoutine()
    {
        while (true)
        {
            DetectTargets();
            yield return new WaitForSeconds(detectionInterval);
        }
    }

    void DetectTargets()
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, detectionRange, targetLayer);

        if (targetsInViewRadius.Length > 0)
        {
            foreach (var target in targetsInViewRadius)
            {
                if (IsTargetInSight(target.transform))
                {
                    if (!detectedTargets.Contains(target.transform))
                    {
                        detectedTargets.Add(target.transform);
                        if (target.TryGetComponent<AIStimulateSourceComp>(out var sourceComp))
                        {
                            sourceComp.OnTargetDetected(transform, lifeSpan);
                        }
                    }
                    else
                    {
                        // Reset the lifespan if the target is already detected
                        if (target.TryGetComponent<AIStimulateSourceComp>(out var sourceComp))
                        {
                            sourceComp.OnTargetDetected(transform, lifeSpan);
                        }
                    }
                }
            }
        }

        // Handle lost targets
        if (detectedTargets.Count >= 0)
        {
            for (int i = 0; i < detectedTargets.Count; i++)
            {
                Transform target = detectedTargets[i];
                if (!IsTargetInSight(target))
                {
                    if (target.TryGetComponent<AIStimulateSourceComp>(out var sourceComp))
                    {
                        sourceComp.OnLostSight(transform);
                        if (sourceComp.GetLifeSpan(transform) <= 0)
                        {
                            detectedTargets.RemoveAt(i);
                            sourceComp.OnLost(transform);
                            i--; // Adjust index after removal
                        }
                    }
                }
            }
        }
    }

    bool IsTargetInSight(Transform target)
    {
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, directionToTarget);
        RaycastHit hit;
        // Check if the target is within the field of view angle and not obstructed by obstacles
        Physics.Raycast(transform.position, directionToTarget, out hit, detectionRange, obstacleLayer);
        if (hit.collider != null && hit.transform != target)
        {
            return false; // Target is obstructed by an obstacle
        }
        return angle < fieldOfViewAngle / 2 && Vector3.Distance(transform.position, target.position) <= detectionRange && 
               Mathf.Abs(target.position.y - transform.position.y) <= 0.5f * detectionHeight; // Check height difference
    }
}

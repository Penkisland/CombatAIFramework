using System.Collections.Generic;
using UnityEngine;

public class AIStimulateSourceComp : MonoBehaviour
{
    Dictionary<Transform, float> detectedTargets = new Dictionary<Transform, float>();

    public void OnTargetDetected(Transform target, float lifeSpan = 5f)
    {
        if (!detectedTargets.ContainsKey(target))
        {
            detectedTargets[target] = lifeSpan; // Set initial lifespan
        }
        else
        {
            detectedTargets[target] = lifeSpan; // Reset lifespan
        }
    }

    public void OnLostSight(Transform source)
    {
        if (detectedTargets.ContainsKey(source))
        {
            detectedTargets[source] -= source.GetComponent<AIVisionPerceptionComp>().detectionInterval;
        }
    }

    public void OnLost(Transform target)
    {
        if (detectedTargets.ContainsKey(target))
        {
            detectedTargets.Remove(target);
        }
    }

    public float GetLifeSpan(Transform target)
    {
        if (detectedTargets.ContainsKey(target))
        {
            return detectedTargets[target];
        }
        return 0f; // Target not detected
    }
}

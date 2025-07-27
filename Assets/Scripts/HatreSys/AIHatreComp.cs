using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIVisionPerceptionComp))]
public class AIHatreComp : MonoBehaviour
{
    public DA_AIHatreConfig hatreConfig;

    private AIVisionPerceptionComp visionPerception;
    private Dictionary<Transform, float> hatreValues = new Dictionary<Transform, float>();
    public float timerInterval = 1f; // Time in seconds between updates
    // Start is called before the first frame update
    void Start()
    {
        if (hatreConfig == null)
        {
            Debug.LogError("Hatre configuration is not set!");
            return;
        }

        visionPerception = GetComponent<AIVisionPerceptionComp>();
        InvokeRepeating(nameof(OnTimer), 0f, timerInterval);
    }

    void OnTimer()
    {
        if (visionPerception.detectedTargets.Count > 0)
        {
            foreach (var target in visionPerception.detectedTargets)
            {
                if (target != null && !hatreValues.ContainsKey(target))
                {
                    hatreValues[target] = hatreConfig.minHatre; // Initialize with minimum hatre value
                }

                if (hatreValues.ContainsKey(target))
                {
                    hatreValues[target] += hatreConfig.hatreWhenDetected * timerInterval;
                    hatreValues[target] = Mathf.Clamp(hatreValues[target], hatreConfig.minHatre, hatreConfig.maxHatre);
                }
            }

            List<Transform> lostTargets = visionPerception.GetLostTargets();
            foreach (var lostTarget in lostTargets)
            {
                if (hatreValues.ContainsKey(lostTarget))
                {
                    hatreValues[lostTarget] -= hatreConfig.hatreWhenLost * timerInterval;
                    hatreValues[lostTarget] = Mathf.Clamp(hatreValues[lostTarget], hatreConfig.minHatre, hatreConfig.maxHatre);
                    if (hatreValues[lostTarget] <= hatreConfig.minHatre)
                    {
                        hatreValues.Remove(lostTarget); // Remove target if hatre value is zero or less
                    }
                }
            }
        }
    }

    public void OnAttackHatre(Transform attacker)
    {
        if (hatreValues.ContainsKey(attacker))
        {
            hatreValues[attacker] += hatreConfig.hatreWhenAttacked;
            hatreValues[attacker] = Mathf.Clamp(hatreValues[attacker], hatreConfig.minHatre, hatreConfig.maxHatre);
        }
        else
        {
            hatreValues[attacker] = hatreConfig.hatreWhenAttacked; // Initialize if not present
        }
    }

    public void OnKilledHatre(Transform attacker)
    {
        if (hatreValues.ContainsKey(attacker))
        {
            hatreValues[attacker] += hatreConfig.hatreWhenKilled;
            hatreValues[attacker] = Mathf.Clamp(hatreValues[attacker], hatreConfig.minHatre, hatreConfig.maxHatre);
        }
        else
        {
            hatreValues[attacker] = hatreConfig.hatreWhenKilled; // Initialize if not present
        }
    }

    void OnDestroy()
    {
        CancelInvoke(nameof(OnTimer)); // Stop the timer when the component is destroyed
        hatreValues.Clear(); // Clear the hatre values dictionary
    }
}

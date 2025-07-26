using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class BTT_SelectTarget : Task
{
    SharedTransform target;
    AIVisionPerceptionComp perceptionComp;

    public override void OnAwake()
    {
        perceptionComp = GetComponent<AIVisionPerceptionComp>();
    }

    public override TaskStatus OnUpdate()
    {
        if (perceptionComp == null || perceptionComp.detectedTargets.Count == 0)
        {
            return TaskStatus.Failure;
        }

        // Select the first detected target
        target.Value = perceptionComp.detectedTargets[0];
        return TaskStatus.Success;
    }
}

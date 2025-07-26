using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DA_NewAIPerceptionConfig", menuName = "ScriptableObjects/DataAsset/DAB_AIPerceptionConfig", order = 1)]
public class DAB_AIPerceptionConfig : ScriptableObject
{
    public float fieldOfViewAngle;
    public float detectionRange;
    public float detectionHeight;
    public float lifeSpan;
    public LayerMask targetLayer;
    public LayerMask obstacleLayer;
    public float detectionInterval;
}

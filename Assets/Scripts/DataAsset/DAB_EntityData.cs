using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DA_NewEntityData", menuName = "ScriptableObjects/DataAsset/DAB_EntityData")]
public class DAB_EntityData : ScriptableObject
{
    public DAB_AIBTConfig aiBTConfig;
    public DAB_LocomotionConfig locomotionConfig;
    public DAB_AnimTagConfig animTagConfig;
    public DAB_AIPerceptionConfig aiPerceptionConfig;
}

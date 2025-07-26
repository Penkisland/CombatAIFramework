using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;


[CreateAssetMenu(fileName = "DA_NewAIBTConfig", menuName = "ScriptableObjects/DataAsset/DAB_AIBTConfig", order = 1)]
public class DAB_AIBTConfig : ScriptableObject
{
    [Serializable]
    public struct AIBTMap
    {
        public string name;
        public BehaviorTree behaviorTree;
    }

    public List<AIBTMap> aiBTMaps = new List<AIBTMap>();
}

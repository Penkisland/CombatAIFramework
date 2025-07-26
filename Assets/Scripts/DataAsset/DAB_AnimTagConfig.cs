using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DA_NewAnimTagConfig", menuName = "ScriptableObjects/DataAsset/DAB_AnimTagConfig")]
public class DAB_AnimTagConfig : ScriptableObject
{
    [Serializable]
    public struct AnimTagConfig
    {
        public string animTag;
        public GA_ConditionedAbilityBase conditionedAbilityBase;
    }

    public List<AnimTagConfig> animTagConfigs;
}

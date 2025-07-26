using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GA_ConditionedAbilityBase", menuName = "ScriptableObjects/GAS/Conditioned Ability Base")]
public class GA_ConditionedAbilityBase : ScriptableObject
{
    public string abilityName;
    public string description;
    public float cooldownTime;
    public bool isActive;
    public bool isCoolingDown;

    public enum AbilityCooldownStartType
    {
        OnActivation,
        OnDeactivation,
        OnConditionMet
    }

    public AbilityCooldownStartType cooldownStartType;

    // Method to activate the ability
    public Action<GAS_GameAbilitySet> onActivate;
    public Action<GAS_GameAbilitySet> onDeactivate;

    void OnEnable()
    {
        isActive = false;
        isCoolingDown = false;

        onActivate += (abilitySet) =>
        {
            isActive = true;
            Debug.Log($"{abilityName} activated.");
        };

        onDeactivate += (abilitySet) =>
        {
            isActive = false;
            Debug.Log($"{abilityName} deactivated.");
        };
    }
}

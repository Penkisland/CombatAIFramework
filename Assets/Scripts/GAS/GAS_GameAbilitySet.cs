using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GAS_GameAbilitySet : MonoBehaviour
{
    public enum AbilityRunningState
    {
        NotRunning,
        Running,
        End
    }
    [Serializable] public struct Ability
    {
        public string abilityName;
        public GA_ConditionedAbilityBase conditionedAbilityBase;
    }

    public AbilityRunningState abilityRunningState = AbilityRunningState.NotRunning;

    public List<Ability> abilities;
    public Dictionary<string, GA_ConditionedAbilityBase> abilityDictionary = new Dictionary<string, GA_ConditionedAbilityBase>();
    public Transform target;

    public void ActivateAbility(string abilityName)
    {
        abilityDictionary[abilityName].onActivate?.Invoke(this);
        if (abilityDictionary[abilityName].cooldownStartType == GA_ConditionedAbilityBase.AbilityCooldownStartType.OnActivation)
        {
            StartCoroutine(CooldownCoroutine(abilityDictionary[abilityName]));
            abilityRunningState = AbilityRunningState.Running;
        }
    }

    public void DeactivateAbility(string abilityName)
    {
        abilityDictionary[abilityName].onDeactivate?.Invoke(this);
        if (abilityDictionary[abilityName].cooldownStartType == GA_ConditionedAbilityBase.AbilityCooldownStartType.OnDeactivation)
        {
            StartCoroutine(CooldownCoroutine(abilityDictionary[abilityName]));
            abilityRunningState = AbilityRunningState.End;
        }
    }

    private IEnumerator CooldownCoroutine(GA_ConditionedAbilityBase ability)
    {
        ability.isCoolingDown = true;
        yield return new WaitForSeconds(ability.cooldownTime);
        ability.isCoolingDown = false;
        Debug.Log($"{ability.abilityName} cooldown complete.");
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (var ability in abilities)
        {
            if (!abilityDictionary.ContainsKey(ability.abilityName))
            {
                abilityDictionary.Add(ability.abilityName, Instantiate(ability.conditionedAbilityBase));
            }
        }
    }
}

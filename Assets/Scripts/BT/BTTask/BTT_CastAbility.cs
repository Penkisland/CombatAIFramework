using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class BTT_CastAbility : Task
{
    public string abilityName;
    private GAS_GameAbilitySet gameAbilitySet;
    private Animator animator;

    public override void OnAwake()
    {
        gameAbilitySet = GetComponent<GAS_GameAbilitySet>();
        animator = GetComponent<Animator>();
        if (gameAbilitySet == null || animator == null)
        {
            Debug.LogError("GAS_GameAbilitySet component not found on the GameObject.");
        }
    }

    public override void OnStart()
    {
        if (gameAbilitySet == null)
        {
            Debug.LogError("GAS_GameAbilitySet component not found on the GameObject.");
            return;
        }

        // Check if the ability exists in the game ability set
        if (!gameAbilitySet.abilityDictionary.ContainsKey(abilityName))
        {
            Debug.LogError($"Ability '{abilityName}' not found in the game ability set.");
            return;
        }

        // Activate the ability
        GA_ConditionedAbilityBase ability = gameAbilitySet.abilityDictionary[abilityName];
        if (ability == null)
        {
            Debug.LogError($"Conditioned ability base for '{abilityName}' is null.");
            return;
        }
        // Trigger the ability activation
        ability.onActivate?.Invoke(gameAbilitySet);
        animator.Play(abilityName); // Play the animation associated with the ability
    }

    public override TaskStatus OnUpdate()
    {
        if (animator.GetNextAnimatorStateInfo(0).IsName(abilityName) && 
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            // Ability animation has finished playing
            gameAbilitySet.DeactivateAbility(abilityName);
            return TaskStatus.Success;
        }
        else if (gameAbilitySet.abilityRunningState == GAS_GameAbilitySet.AbilityRunningState.Running)
        {
            // Ability is still running
            return TaskStatus.Running;
        }

        return TaskStatus.Failure;
    }
}

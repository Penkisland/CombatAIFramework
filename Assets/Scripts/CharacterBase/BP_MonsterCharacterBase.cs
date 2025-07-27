using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// BP_MonsterCharacterBase is the base class for monster characters in the game.
/// It manages the character's attributes, initializes components, and handles basic functionality.
/// For example, it handles when the monster is attacked, how its attribute changes.
/// </summary>

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(FSM_BasicFSM))]
[RequireComponent(typeof(AIVisionPerceptionComp))]
[RequireComponent(typeof(AIHatreComp))]
[RequireComponent(typeof(BehaviorTree))]
[RequireComponent(typeof(ANSC_ANSContainer))]
[RequireComponent(typeof(GAS_GameAbilitySet))]
[RequireComponent(typeof(NavMeshAgent))]
public class BP_MonsterCharacterBase : MonoBehaviour
{
    public DAB_EntityData entityData;

    [Header("Attributes")]
    public float curHealth;
    public float curToughness;
    public float curAtk;
    public float curDef;
    public float curMoveRate;
    public float curAtkRate;
    public float curDefRate;
    public float curImpactResistanceRate;
    private List<float> curImpactResistance;

    void Awake()
    {
        InitializeComponents();
    }


    private void InitializeComponents()
    {
        if (entityData != null)
        {
            curHealth = entityData.basicAttributes.maxHealth;
            curToughness = entityData.basicAttributes.maxToughness;
            curAtk = entityData.basicAttributes.baseAtk;
            curDef = entityData.basicAttributes.baseDef;
            curMoveRate = entityData.basicAttributes.moveRate;
            curAtkRate = entityData.basicAttributes.atkRate;
            curDefRate = entityData.basicAttributes.defRate;
            curImpactResistanceRate = entityData.basicAttributes.impactResistanceRate;
            curImpactResistance = new List<float>(entityData.basicAttributes.impactResistance);
        }
    }

    void OnDestroy()
    {
        curImpactResistance?.Clear();
        curImpactResistance = null;
    }

    public void OnAttacked(float damage)
    {
    }

    /// <summary>
    /// blend the hitback animation with the current animation.
    /// dtermines the power of the hitback animation based on the list of impact resistance.
    /// calculate the hitback direction based on the hit direction and the character's facing direction.
    /// apply toughness change to the character.
    /// </summary>
    /// <param name="toughnessChange"></param>
    /// <param name="hitDirection"></param>
    /// <param name="hitAngle">the angle between the hit direction and the character's facing direction</param>
    public virtual void OnToughnessChanged(float toughnessChange, Vector3 hitDirection, out float hitAngle)
    {
        Vector3 fwd = transform.forward;
        hitAngle = Vector3.Angle(hitDirection, fwd);
    }
}

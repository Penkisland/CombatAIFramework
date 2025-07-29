using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ST_MC_Idle", menuName = "ScriptableObjects/StateMachine/MC/ST_MC_Idle")]
public class ST_MC_Idle : ST_BasicState
{
    protected override void OnEnable()
    {
        base.OnEnable();

        onEnter += (fsm) =>
        {
            fsm.gameObject.GetComponent<LMS_BasicLocomotion>().SetLocomotionState(LMS_BasicLocomotion.LocomotionState.Idle);
        };

        onCheckConditions += (fsm) =>
        {
            conditionMap["Walk"] = Input.GetAxis("Vertical") != 0;
        };
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ST_MC_Walk", menuName = "ScriptableObjects/StateMachine/MC/ST_MC_Walk")]
public class ST_MC_Walk : ST_BasicState
{
    protected override void OnEnable()
    {
        base.OnEnable();

        onEnter += (fsm) =>
        {
            fsm.gameObject.GetComponent<LMS_BasicLocomotion>().SetLocomotionState(LMS_BasicLocomotion.LocomotionState.Walking);
        };

        onCheckConditions += (fsm) =>
        {
            conditionMap["Idle"] = Input.GetAxis("Vertical") == 0;
            conditionMap["Run"] = Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift);
        };
    }
}

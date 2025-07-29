using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ST_MC_Run", menuName = "ScriptableObjects/StateMachine/MC/ST_MC_Run")]
public class ST_MC_Run : ST_BasicState
{
    protected override void OnEnable()
    {
        base.OnEnable();

        onEnter += (fsm) =>
        {
            fsm.gameObject.GetComponent<LMS_BasicLocomotion>().SetLocomotionState(LMS_BasicLocomotion.LocomotionState.Running);
        };

        onCheckConditions += (fsm) =>
        {
            conditionMap["Walk"] = Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift);
            conditionMap["Idle"] = Input.GetAxis("Vertical") == 0;
        };
    }
}

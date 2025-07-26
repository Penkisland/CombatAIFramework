using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBasicState", menuName = "ScriptableObjects/FSM/ST_BasicState", order = 1)]
public class ST_BasicState : ScriptableObject
{
    public string stateName;

    [SerializeField]
    public struct ConditionTransition
    {
        public string conditionName;
        public ST_BasicState nextState;
    }


    public List<ConditionTransition> transitions = new List<ConditionTransition>();
    public List<string> conditions = new List<string>();
    public List<ST_BasicState> flows = new List<ST_BasicState>();

    public Dictionary<string, bool> conditionMap = new Dictionary<string, bool>();

    public Action<FSM_BasicFSM> onEnter;
    public Action<FSM_BasicFSM> onExit;
    public Action<FSM_BasicFSM> onUpdate;
    public Action<FSM_BasicFSM> onCheckConditions;

    /// <summary>
    /// When use, write like this:
    /// fsm.onTimer += (fsm) => { Debug.Log("Timer triggered!"); };
    /// then start the timer with fsm.InvokeTimer();
    /// </summary>
    public Action<FSM_BasicFSM> onTimer;


    /// <summary>
    /// Initialize all delegates and condition map.
    /// onTimer should be set first before setting other delegates.
    /// </summary>
    protected virtual void OnEnable()
    {
        foreach (var condition in conditions)
        {
            if (!conditionMap.ContainsKey(condition))
            {
                conditionMap.Add(condition, false);
            }
        }
    }
}

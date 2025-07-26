using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_BasicFSM : MonoBehaviour
{
    public float timerInterval = 1.0f; // Default interval for the timer

    private ST_BasicState currentState;
    private Coroutine timerCoroutine;
    public List<ST_BasicState> states = new List<ST_BasicState>();
    public Action<FSM_BasicFSM> onTimer;

    public Dictionary<string, ST_BasicState> stateMap = new Dictionary<string, ST_BasicState>();
    // Start is called before the first frame update
    void Start()
    {
        RegisterStates();
        if (states.Count > 0)
        {
            currentState = states[0];
            currentState.onEnter?.Invoke(this);
        }
        else
        {
            Debug.LogWarning("No states registered in FSM.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentState?.onUpdate?.Invoke(this);
        currentState?.onCheckConditions?.Invoke(this);
        CheckTransition();
    }

    void OnDestroy()
    {
        UnRegisterStates();
        StopTimer();
    }

    void RegisterStates()
    {
        foreach (var state in states)
        {
            if (!stateMap.ContainsKey(state.stateName))
            {
                stateMap.Add(state.stateName, state);
            }
        }
    }

    void UnRegisterStates()
    {
        stateMap.Clear();
    }

    private void CheckTransition()
    {
        foreach (var transition in currentState.transitions)
        {
            if (currentState.conditionMap.TryGetValue(transition.conditionName, out bool conditionMet) && conditionMet)
            {
                GoToState(transition.nextState.stateName);
                break;
            }
        }
    }

    public void GoToState(string stateName)
    {
        ST_BasicState nextState = stateMap[stateName];
        if (nextState != null)
        {
            currentState.onExit?.Invoke(this);
            currentState = nextState;
            currentState.onEnter?.Invoke(this);
        }
        else
        {
            Debug.LogWarning($"State {stateName} not found.");
        }
    }

    public void InvokeTimer()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
        timerCoroutine = StartCoroutine(InvokeOnTimerRepeat());
    }

    public void StopTimer()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
    }


    private IEnumerator InvokeOnTimerRepeat()
    {
        while (true)
        {
            onTimer?.Invoke(this);
            yield return new WaitForSeconds(timerInterval);
        }
    }
}

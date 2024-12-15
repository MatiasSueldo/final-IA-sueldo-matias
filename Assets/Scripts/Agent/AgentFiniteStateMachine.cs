using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentFiniteStateMachine : MonoBehaviour
{
    Dictionary<AgentStates, AgentState> allStates = new Dictionary<AgentStates, AgentState>();
    AgentState _currentState;
    Agent enemy;


    private void Start()
    {
        EnemyFollow.onFoundPlayer += SetFollowState;
    }
    public AgentFiniteStateMachine(Agent enemy)
    {
        this.enemy = enemy;
    }
    public void SetFollowState(Vector3 target)
    {
        if (GetCurrentState() is not AgentChase)
        {
            ChangeState(AgentStates.Chase, target);
        }
    }

    public void AddState(AgentStates enemyState, AgentState state)
    {

        if (!allStates.ContainsKey(enemyState))
        {
            allStates.Add(enemyState, state);
            state.fsm = this;
            state.enemy = this.enemy;
        }
        else
        {
            allStates[enemyState] = state;
        }
    }

    public void Update()
    {
        _currentState.OnUpdate();
    }

    public void ChangeState(AgentStates state, Vector3 target)
    {
        _currentState?.OnExit();
        if (allStates.ContainsKey(state)) _currentState = allStates[state];
        _currentState?.OnEnter(target);
    }
    public AgentState GetCurrentState()
    {
        return _currentState;
    }
}


public enum AgentStates
{
    Idle, Attack, Follow, BackToBase, Chase
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AgentState
{
    public abstract void OnEnter(Vector3 target);
    public abstract void OnUpdate();
    public abstract void OnExit();
    public AgentFiniteStateMachine fsm;
    public Agent enemy;
}

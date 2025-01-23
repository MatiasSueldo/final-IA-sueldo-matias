using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AgentIdle : AgentState
{

    public override void OnEnter(Vector3 target)
    {
        enemy.currentState = AgentStates.Idle;
    }

    public override void OnExit()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnUpdate()
    {
        enemy.SwitchToAttackState();
        if (Vector3.Distance(enemy.transform.position, enemy.target.transform.position)>enemy.separationRadius*4)
        {
            fsm.ChangeState(AgentStates.Follow, enemy.target.transform.position);
        }
    }

}

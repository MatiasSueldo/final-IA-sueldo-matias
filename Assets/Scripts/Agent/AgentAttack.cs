using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AgentAttack : AgentState
{
    Vector3 attackTarget;
    public override void OnEnter(Vector3 target)
    {
        attackTarget = target;
        enemy.currentState = AgentStates.Attack;
    }

    public override void OnExit()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnUpdate()
    {
        if(enemy.life<20)
        {
            if (enemy.redAgent)
            {
                fsm.ChangeState(AgentStates.BackToBase, GameManager.Instance.redBase.transform.position);
            }
            else
            {
                fsm.ChangeState(AgentStates.BackToBase, GameManager.Instance.greenBase.transform.position);
            }
        }
        enemy.shoot(attackTarget);
        enemy.OutOfAttackState();
    }



    

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyIdle : State
{

    public override void OnEnter(Vector3 target)
    {
    }

    public override void OnExit()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnUpdate()
    {
        SwitchToAttackState();
        if (Input.GetMouseButtonDown(enemy.GetMouseClick()))
        {
            Vector3 click = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            click = new Vector3(click.x, click.y, 0);
            fsm.ChangeState(EnemyState.Chase, click);
        }
    }
    public void SwitchToAttackState()
    {
        if (enemy.redAgent)
        {
            enemy.enemyAgents = GameManager.Instance.greenAgents;
        }
        else
        {
            enemy.enemyAgents = GameManager.Instance.redAgents;
        }
        foreach (Agent agent in enemy.enemyAgents)
        {
            if (Vector3.Distance(enemy.transform.position, agent.transform.position) < enemy.enemyAttackRadius)
            {
                if (enemy.InSight(enemy.transform.position, agent.transform.position))
                    fsm.ChangeState(EnemyState.Attack, agent.transform.position);
            }
        }
    }

}

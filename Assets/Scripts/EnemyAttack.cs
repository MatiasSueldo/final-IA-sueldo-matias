using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAttack : State
{
    Vector3 attackTarget;
    public override void OnEnter(Vector3 target)
    {
        attackTarget = target;
        enemy.currentState = EnemyState.Attack;
    }

    public override void OnExit()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnUpdate()
    {
        enemy.shoot(attackTarget);

        if (Input.GetMouseButtonDown(enemy.GetMouseClick()))
        {
            Vector3 click = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            click = new Vector3(click.x, click.y, 0);
            fsm.ChangeState(EnemyState.Chase, click);
        }
    }



    

}

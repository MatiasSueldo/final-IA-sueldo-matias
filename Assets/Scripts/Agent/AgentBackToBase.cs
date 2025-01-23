using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentBackToBase : AgentState
{
    public float speed;
    List<Node> _path;
    Vector3 _target;

    public override void OnEnter(Vector3 target)
    {
        _target = target;
        enemy.currentState = AgentStates.BackToBase;
        _path = GameManager.Instance.pf.CalculateMovement(GetNearestNode(), GetNearestNodeToTarget(_target));
    }

    public Node GetNearestNode()
    {
        float nearestDistance = Mathf.Infinity;
        Node nearestNodeToTarget = null;
        foreach (Node node in GameManager.Instance.GetNodes())
        {
            float nodeDistanceToNode = Vector3.Distance(node.transform.position, enemy.transform.position);
            if (nodeDistanceToNode < nearestDistance)
            {
                nearestDistance = nodeDistanceToNode;
                nearestNodeToTarget = node;
            }
        }
        return nearestNodeToTarget;
    }

    public Node GetNearestNodeToTarget(Vector3 target)
    {
        float nearestDistance = Mathf.Infinity;
        Node nearestNodeToTarget = null;
        foreach (Node node in GameManager.Instance.GetNodes())
        {
            float nodeDistanceToPlayer = Vector3.Distance(node.transform.position, target);
            if (nodeDistanceToPlayer < nearestDistance)
            {
                nearestDistance = nodeDistanceToPlayer;
                nearestNodeToTarget = node;
            }
        }
        return nearestNodeToTarget;
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        /*if (InSight(enemy.transform.position, enemy.target.transform.position))
        {
            enemy.currentState = AgentStates.Chase;
            fsm.ChangeState(AgentStates.Follow, enemy.target.transform.position);
        }*/
        /*if(enemy.GetTargetPlayer()!= null)
        {
            fsm.ChangeState(EnemyState.Follow,enemy.GetTargetPlayer().transform.position);
        }*/
        if (_path != null && _path.Count > 0)
        {
            Vector3 dir = _path[0].transform.position - enemy.transform.position;
            dir.z = 0;
            //Intento validar si se va a poder llegar al proximo nodo
            /*if (!InSight(_path[1].transform.position, _path[0].transform.position))
            {
                fsm.ChangeState(EnemyState.Idle, Vector3.zero);
            }*/
            if (dir.magnitude <= 0.5)
            {
                
                _path.RemoveAt(0);
                
            }
            else
            {
               
                enemy.Move(dir);
            }

        }
        if (_path == null || _path.Count <= 0)
        {
            Vector3 dir = _target-enemy.transform.position;
            dir.z = 0;
            if (dir.magnitude <= 0.1)
            {
                if (enemy.life >= 100)
                {
                    fsm.ChangeState(AgentStates.Follow, enemy.target.transform.position);

                }
                else
                {
                    enemy.recoverLifeCoroutine();
                }
            }
            else
            {
                enemy.Move(dir);
            }
        }
    }

    public void SetPath(List<Node> path)
    {
        _path = path;
        _path?.Reverse();
    }
    public bool InSight(Vector3 a, Vector3 b)
    {
        return !Physics.Raycast(a, b - a, Vector3.Distance(a, b), GameManager.Instance.WallMask);
    }
}

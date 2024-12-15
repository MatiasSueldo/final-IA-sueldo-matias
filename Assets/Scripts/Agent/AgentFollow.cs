using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AgentFollow : AgentState
{

    public delegate void FoundPlayer(Vector3 position);
    public static event FoundPlayer onFoundPlayer;

    public override void OnEnter(Vector3 target)
    {
        enemy.currentState = AgentStates.Follow;
    }

    public void SetFollowState(Vector3 target)
    {
        if (fsm.GetCurrentState() is not AgentChase)
        {
            fsm.ChangeState(AgentStates.Chase, target);
        }
    }
    public override void OnExit()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnUpdate()
    {
        enemy.SwitchToAttackState();
        if (!InSight(enemy.transform.position, enemy.target.transform.position))
        {
            enemy.currentState = AgentStates.Chase; 
            fsm.ChangeState(AgentStates.Chase,enemy.target.transform.position);
        }
        else
        {
            if (Vector3.Distance(enemy.transform.position, enemy.target.transform.position) < enemy.separationRadius)
            {
                fsm.ChangeState(AgentStates.Idle, enemy.target.transform.position);
            }
        }
        if (!HasToUseObstacleAvoidance())
        {
            Vector3 vec = Arrive(enemy.target.transform.position);
            AddForce(vec);
        }
        Separation(enemy.redAgent ? GameManager.Instance.redAgents: GameManager.Instance.greenAgents);
        Move();

    }
    public Vector3 Seek(Vector3 targetPos)
    {
        return Seek(targetPos, enemy._maxSpeed);
    }

    public Vector3 Seek(Vector3 targetPos, float maxSpeed)
    {
        Debug.DrawLine(enemy.transform.position, targetPos, Color.yellow);
        Vector3 vectorDeseado = targetPos - enemy.transform.position;

        vectorDeseado.Normalize();
        vectorDeseado *= maxSpeed;

        Vector3 steering = vectorDeseado - enemy.velocity;
        steering = Vector3.ClampMagnitude(steering, enemy._maxForce * Time.deltaTime);


        return steering;

    }
    public virtual void Move()
    {

        enemy.transform.position += enemy.velocity * enemy.speed * Time.deltaTime;
        enemy.transform.forward = enemy.velocity * enemy.speed;
        AddForce(Separation(enemy.redAgent ? GameManager.Instance.redAgents : GameManager.Instance.greenAgents));
        UpdateBoundPosition();
    }


    public bool HasToUseObstacleAvoidance()
    {
        Vector3 avoidance = ObstacleAvoidance();
        avoidance.z = 0;
        AddForce(avoidance * 10);
        return avoidance != Vector3.zero;
    }

    public Vector3 Arrive(Vector3 ship)
    {
        Vector3 desired = Vector3.zero;
        int foodCount = 0;

        if (Vector3.Distance(enemy.transform.position, ship) < enemy.viewRadius)
            desired += ship;

        float dist = Vector3.Distance(enemy.transform.position, desired);
        if (dist < enemy.viewRadius)
        {
            return Seek(desired, enemy._maxSpeed * (dist / enemy.viewRadius));
        }
        else
        {
            return Seek(desired, enemy._maxSpeed);
        }
    }
    public Vector3 Alignment(List<Agent> agents)
    {
        Vector3 desired = Vector3.zero;
        int boidCount = 0;

        foreach (Agent item in agents)
        {
            if (Vector3.Distance(enemy.transform.position, item.transform.position) > enemy.viewRadius) continue;
            desired += item.velocity;
            boidCount++;
        }
        desired /= boidCount;
        return SteeringToAlignment(desired.normalized * enemy._maxSpeed);
    }

    public Vector3 SteeringToAlignment(Vector3 desired)
    {
        return Vector3.ClampMagnitude(desired - enemy.velocity, enemy._maxForce * Time.deltaTime);
    }

    public Vector3 Pursuit(Agent agent)
    {
        Vector3 desired = Vector3.zero;
        desired += agent.transform.position + agent.velocity;

        return Seek(desired * enemy._maxSpeed);
    }
    /*public Vector3 Evade(List<Player> agents)
    {
        Vector3 desired = Vector3.zero;
        int enemyCount = 0;

        foreach (Player item in agents)
        {
            if (Vector3.Distance(transform.position, item.transform.position) > viewRadius) continue;
            enemyCount++;
            desired += item.transform.position + item.velocity;
        }

        return -Seek(desired / enemyCount, _maxSpeed);
    }*/

    /*public Vector3 Flee(List<Player> agents)
    {
        Vector3 desired = Vector3.zero;
        int enemyCount = 0;

        foreach (Player item in agents)
        {
            if (Vector3.Distance(transform.position, item.transform.position) > viewRadius) continue;
            enemyCount++;
            desired += item.transform.position;
        }

        return -Seek(desired / enemyCount, _maxSpeed);

    }*/

    public Vector3 ObstacleAvoidance()
    {
        /*Debug.DrawLine(transform.position, transform.position + transform.forward * viewRadius);
        if (Physics.Raycast(transform.position, transform.forward, viewRadius,obstacleLayer))
        {
            return Seek(transform.forward, _maxSpeed);
        }*/

        Debug.DrawLine(enemy.transform.position + enemy.transform.up * enemy.size, enemy.transform.position + enemy.transform.up * enemy.size + enemy.transform.forward * enemy.dodgeRadius, Color.green);
        Debug.DrawLine(enemy.transform.position - enemy.transform.up * enemy.size, enemy.transform.position - enemy.transform.up * enemy.size + enemy.transform.forward * enemy.dodgeRadius, Color.green);

        if (Physics.Raycast(enemy.transform.position + enemy.transform.up * enemy.size, enemy.transform.forward, enemy.dodgeRadius, enemy.obstacleLayer))
        {
            return Seek(enemy.transform.right - enemy.transform.up * 4, enemy._maxSpeed);
        }
        else if (Physics.Raycast(enemy.transform.position - enemy.transform.up * enemy.size, enemy.transform.forward, enemy.dodgeRadius, enemy.obstacleLayer))
        {
            return Seek(enemy.transform.right + enemy.transform.up, enemy._maxSpeed);
        }


        return Vector3.zero;
    }

    protected Vector3 Cohesion(List<Agent> agents)
    {
        Vector3 desired = Vector3.zero;
        int boidCount = 0;

        foreach (Agent item in agents)
        {
            if (item == enemy) continue;
            Vector3 dist = item.transform.position - enemy.transform.position;
            if (dist.sqrMagnitude > enemy.viewRadius * enemy.viewRadius) continue;
            boidCount++;
            desired += item.transform.position;
        }

        if (desired == Vector3.zero) return Vector3.zero;

        desired /= boidCount;

        return Seek(desired);
    }

    protected Vector3 Separation(List<Agent> agents)
    {
        Vector3 desired = Vector3.zero;
        int boidCount = 0;

        foreach (Agent item in agents)
        {
            if (item == enemy) continue;
            Vector3 dist = item.transform.position - enemy.transform.position;
            if (dist.sqrMagnitude > enemy.separationRadius * enemy.separationRadius) continue;
            boidCount++;
            desired += dist;
        }

        if (desired == Vector3.zero) return Vector3.zero;

        desired *= -1;

        return SteeringToAlignment(desired.normalized * enemy._maxSpeed);
    }

    public void AddForce(Vector3 force)
    {
        force.z = 0;
        enemy.velocity.z = 0;
        enemy.velocity = Vector3.ClampMagnitude(force + enemy.velocity, enemy._maxSpeed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(enemy.transform.position, enemy.viewRadius);
    }

    public void UpdateBoundPosition()
    {
        //transform.position = GameManager.Instance.AdjustPositionsToBounds(transform.position);
    }
    public bool InSight(Vector3 a, Vector3 b)
    {
        return !Physics.Raycast(a, b - a, Vector3.Distance(a, b), GameManager.Instance.WallMask);
    }
}
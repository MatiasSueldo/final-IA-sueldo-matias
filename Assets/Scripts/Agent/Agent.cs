using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Agent : MonoBehaviour
{
    private AgentFiniteStateMachine fsm;
    [SerializeField] public Vector3 velocity;

    [SerializeField] public float speed;
    public float size = 0.5f;
    [SerializeField] public GameObject target;
    [SerializeField] public float _maxSpeed;
    [SerializeField] public float _maxForce;
    [SerializeField] public float dodgeRadius,viewRadius, separationRadius, enemyAttackRadius;
    [SerializeField] public LayerMask obstacleLayer;
    public AgentStates currentState;
    public bool redAgent;
    List<Agent> enemyAgents;
    public GameObject bulletPrefab;
    bool shootCoroutineActive = false;
    bool recoverLifeActive = false;
    public int life = 100;



    // Start is called before the first frame update
    void Start()
    {
        if (redAgent)
        {
            GameManager.Instance.redAgents.Add(this);
        }
        else
        {
            GameManager.Instance.greenAgents.Add(this);
        }
        fsm = new AgentFiniteStateMachine(this);
        fsm.AddState(AgentStates.Chase, new AgentChase());
        fsm.AddState(AgentStates.Follow, new AgentFollow());
        fsm.AddState(AgentStates.Idle, new AgentIdle());

        fsm.AddState(AgentStates.BackToBase, new AgentBackToBase());

        fsm.AddState(AgentStates.Attack, new AgentAttack());
        fsm.ChangeState(AgentStates.Follow, Vector3.zero);
        currentState= AgentStates.Follow;
    }

    // Update is called once per frame
    void Update()
    {
        

        fsm.Update();

    }
    public void Move(Vector3 dir)
    {
        transform.forward = dir;
        transform.position += dir.normalized * speed * Time.deltaTime;
    }

    public bool InSight(Vector3 a, Vector3 b)
    {
        return !Physics.Raycast(a, b - a, Vector3.Distance(a, b), GameManager.Instance.WallMask);
    }


    public void SwitchToAttackState()
    {
        if (redAgent)
        {
            enemyAgents = GameManager.Instance.greenAgents;
        }
        else
        {
            enemyAgents = GameManager.Instance.redAgents;
        }
        foreach (Agent agent in enemyAgents)
        {
            if (Vector3.Distance(transform.position, agent.transform.position) < enemyAttackRadius)
            {
                if (InSight(transform.position, agent.transform.position))
                    fsm.ChangeState(AgentStates.Attack, agent.transform.position);
            }
        }
    }

    public void OutOfAttackState()
    {
        int count = 0;
        foreach (Agent agent in enemyAgents)
        {
            if (Vector3.Distance(transform.position, agent.transform.position) < enemyAttackRadius)
            {
                count++;
            }
        }
        if (count == 0)
        {
            fsm.ChangeState(AgentStates.Follow, target.transform.position);
        }
    }


    public void Hurt()
    {
        Debug.Log("Hurt");
        life -= 10;
        Debug.Log("life is : "+ life);
    }

    public void shoot(Vector3 attackTarget)
    {
        if (!shootCoroutineActive)
        {
            StartCoroutine(shootCoroutine(attackTarget));

        }
    }

    public void recoverLifeCoroutine()
    {
        if (recoverLifeActive == false && life < 100)
        {
            StartCoroutine(recoverLife());
        }
    }
    IEnumerator shootCoroutine(Vector3 attackTarget)
    {
        shootCoroutineActive = true;
        GameObject bullet = GameObject.Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.GetComponent<Bullet>().Initialize(this.redAgent, attackTarget);
        yield return new WaitForSeconds(1.5f);
        shootCoroutineActive = false;
    }

    IEnumerator recoverLife()
    {
        recoverLifeActive = true;
        life += 10;
        yield return new WaitForSeconds(0.5f);
        if (life < 100)
        {
            StartCoroutine(recoverLife());
        }
        recoverLifeActive = false;
        if (life >= 100)
        {
            fsm.ChangeState(AgentStates.Chase, target.transform.position);
        }

    }


}

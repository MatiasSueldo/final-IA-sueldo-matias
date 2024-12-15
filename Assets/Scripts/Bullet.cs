using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int speed;
    Agent agent;
    Vector3 target;
    Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        direction= (target - transform.position).normalized;
    }
    public void Initialize(Agent agentServed,Vector3 attackTarget)
    {
        target = attackTarget;
        agent = agentServed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction.normalized * speed * Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        Agent colliderAgent = other.gameObject.GetComponent<Agent>();
        if (colliderAgent != null)
        {
            if (colliderAgent.redAgent != agent.redAgent)
            {
                colliderAgent.Hurt();
                Destroy(this.gameObject);
            }
        }
        if (other.gameObject.layer == 6)
        {
            Destroy(this.gameObject);
        }
    }
}

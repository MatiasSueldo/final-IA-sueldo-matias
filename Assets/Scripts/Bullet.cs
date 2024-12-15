using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int speed;
    bool _redAgent;
    Vector3 target;
    Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        direction= (target - transform.position).normalized;
    }
    public void Initialize(bool redAgent, Vector3 attackTarget)
    {
        target = attackTarget;
        _redAgent = redAgent;
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
            if (colliderAgent.redAgent != _redAgent)
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

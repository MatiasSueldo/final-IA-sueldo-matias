using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baseScript : MonoBehaviour
{
    List<Agent> agents = new List<Agent>();
    public bool isRed=false;


    // Start is called before the first frame update
    void Start()
    {
        if (isRed)
        {
            agents = GameManager.Instance.redAgents;
        }
        else
        {
            agents = GameManager.Instance.greenAgents;
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        Agent agent = other.gameObject.GetComponent<Agent>();
        if (agent != null && agents.Contains(agent))
        {
            agent.recoverLifeCoroutine();
        }
    }
}

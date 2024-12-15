using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Node _startingNode;
    private Node _goalNode;
    public Pathfinding pf;
    [SerializeField] private List<Node> allNodes = new List<Node>();
    public LayerMask WallMask;
    public List<Agent> redAgents;
    public List<Agent> greenAgents;
    public GameObject redBase;
    public GameObject greenBase;

    public static GameManager Instance;

    public void SetNodes(List<Node> nodes)
    {
        allNodes=nodes;
    }
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
        redAgents = new();

        greenAgents = new();
    }

    public void SetStartingNode(Node node)
    {
        if (_startingNode != null) PaintGameObject(_startingNode.gameObject, Color.white);
        _startingNode = node;
        PaintGameObject(_startingNode.gameObject, Color.green);
    }

    public void SetGoalNode(Node node)
    {
        if(_goalNode!=null) PaintGameObject(_goalNode.gameObject, Color.white);
        _goalNode = node;
        PaintGameObject(_goalNode.gameObject, Color.red);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PaintGameObject(GameObject obj, Color color)
    {
        obj.GetComponent<Renderer>().material.color = color;
    }

    public List<Node> GetNodes()
    {
        return allNodes;
    }
}

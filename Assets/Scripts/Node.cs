using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Node : MonoBehaviour
{
    int _x;
    int _y;
    Grid _grid;
    public List<Node> _neighbors = new List<Node>();
    public float cost = 1;
    public bool isBlocked = false;
    [SerializeField] TextMeshProUGUI _textCost;
    [SerializeField] LayerMask wallLayer;
    public UnityEvent onEditableWallTriggered;
    public EditableWallScript wallScript;

    private void Start()
    {
        EventManager.SubscribeToEvent(EventsType.WALL_UPDATE, HandleGetNewNeighbors);
    }

    public void HandleGetNewNeighbors(params object[] parameters)
    {
        _neighbors = CalculateNeighbors();
    }
    public List<Node> GetNeighbors()
    {
        if(_neighbors.Count > 0)
        {
            return _neighbors;
        }
        return CalculateNeighbors();
    }
    public List<Node> CalculateNeighbors()
    {
        _neighbors = new List<Node>();
        Node neighbor;
        neighbor = _grid.GetNode(_x + 1, _y);
        if (neighbor != null && neighborCheck(neighbor)) _neighbors.Add(neighbor);
        neighbor = _grid.GetNode(_x - 1, _y);
        if (neighbor != null && neighborCheck(neighbor)) _neighbors.Add(neighbor);
        neighbor = _grid.GetNode(_x, _y + 1);
        if (neighbor != null && neighborCheck(neighbor)) _neighbors.Add(neighbor);
        neighbor = _grid.GetNode(_x, _y - 1);
        if (neighbor != null && neighborCheck(neighbor)) _neighbors.Add(neighbor);

        return _neighbors;
    }

    public bool neighborCheck(Node neighbor)
    {
        if (!Physics.Raycast(transform.position, neighbor.transform.position - transform.position, out RaycastHit hit, (int)(transform.position - neighbor.transform.position).magnitude, wallLayer))
        { 
            return true;
        }
        return false;
    }
    public void Initialize(int x, int y, Vector3 pos, Grid grid, LayerMask wallLayer)
    {
        this.wallLayer = wallLayer;
        this._x = x;
        this._y = y;
        transform.position = pos;
        this._grid = grid;
        gameObject.name = "Node: " + _x + "," + _y;

        SetCost(1);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        foreach(Node node in GetNeighbors())
        {
            Gizmos.DrawLine(transform.position, node.transform.position);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isBlocked ? Color.grey : Color.white;
        Gizmos.DrawSphere(transform.position, 0.1f);
    }

    private void OnMouseOver()
    {
        /*
        if (Input.GetMouseButtonDown(0))
        {
            TestGameManager.Instance.SetStartingNode(this);
        }

        if (Input.GetMouseButtonDown(1))
        {
            TestGameManager.Instance.SetGoalNode(this);
        }
        if (Input.GetMouseButtonDown(2) || Input.GetKeyDown(KeyCode.F))
        {
            isBlocked = true;

            GameManager.Instance.PaintGameObject(gameObject, isBlocked? Color.grey : Color.white);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            SetCost(cost + 1);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            SetCost(cost - 1);
        }*/
    }

    public void SetCost(float newCost)
    {
        cost = Mathf.Clamp(newCost,1,99);
       /* _textCost.text = cost.ToString();
        _textCost.enabled = cost == 1?false : true;
    */}
}

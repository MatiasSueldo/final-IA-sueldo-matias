using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EditableWallScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    Debug.Log("Hit");
                    if (gameObject.layer == 0)
                    {
                        gameObject.layer = 3;
                        PaintGameObject(gameObject, Color.red);
                    }
                    else
                    {
                        gameObject.layer = 0;
                        PaintGameObject(gameObject, Color.blue);
                    }
                    EventManager.TriggerEvent(EventsType.WALL_UPDATE);
                    
                }
            }

        }
    }
    public void PaintGameObject(GameObject obj, Color color)
    {
        obj.GetComponent<Renderer>().material.color = color;
    }
}

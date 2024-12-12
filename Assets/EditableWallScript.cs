using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditableWallScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E pressed");
            if (gameObject.layer == 0)
            {
                gameObject.layer = 3;
            }
            else
            {
                gameObject.layer = 0;
            }
        }
    }
}

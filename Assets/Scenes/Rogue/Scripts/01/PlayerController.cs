using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Range(0.1f, 1f)] // use Range attribute to make it easier to adjust in the inspector
    public float moveStep = 0.25f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // !!! TODO: implement player movement based on input !!!
        // Player going up when pressing the up arrow key
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.position += new Vector3(0f, 0f, moveStep);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            //todo: move down (z-axis)
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //todo: move left (x-axis)
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //todo: move right (x-axis)
        }
        

    }

}

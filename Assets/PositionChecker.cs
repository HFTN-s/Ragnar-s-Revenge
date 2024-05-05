using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionChecker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Debug both local and world postions
        Debug.Log("Local Position: " + transform.localPosition);
        Debug.Log("World Position: " + transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

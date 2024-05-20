using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverSlide : MonoBehaviour
{
    [SerializeField] private GameObject lever;
    [SerializeField] private GameObject objectToMove;
    [SerializeField] private float maxAngle = 90;
    [SerializeField] private float force = 10;
    

    // Update is called once per frame
    void Update()
    {
       // if max angle (from joint component) is reached, then send message to object move

       //also add force upwards to lever to reset to min angle

    }
}

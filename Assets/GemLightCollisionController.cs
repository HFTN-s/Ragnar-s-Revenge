using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemLightCollisionController : MonoBehaviour
{
    public GameObject[] gems = new GameObject[12];
    // Start is called before the first frame update
    void Start()
    {
        //add child objects to array
        for (int i = 0; i < 12; i++)
        {
            gems[i] = transform.GetChild(i).gameObject;
        }
    }

    public void HitByLaser(string hitName)
    {
        //if hit by laser, glow gem hit red
        for (int i = 0; i < 12; i++)
        {
            if (gems[i].name == hitName)
            {
                gems[i].GetComponent<Renderer>().material.color = Color.red;
            }
        }
    }

    public void HitByLaserCorrect()
    {
        //if hit by laser, glow gem hit green
        for (int i = 0; i < 12; i++)
        {
            gems[i].GetComponent<Renderer>().material.color = Color.green;
        }
    }
}

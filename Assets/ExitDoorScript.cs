using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoorScript : MonoBehaviour
{
    public bool isUnlocked;
    public bool axeContact;

    public PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    void Update()
    {
        //if door is unlocked and axe is in contact with the door
        if (isUnlocked && axeContact)
        {
            //Stick axe in door and create joint at contact point
            
        }
    }
}

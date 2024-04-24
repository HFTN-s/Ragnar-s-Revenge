using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
// When player grabs the collider , track joint to hand movement not the position
public class TrackHandToJoint : MonoBehaviour
{
    public XRBaseInteractor interactor;
    public Transform joint;
    public Transform hand;
    private bool isSelected = false;
    // Start is called before the first frame update
    void Start()
    {
        interactor.onSelectEntered.AddListener(OnSelectEntered);
        interactor.onSelectExited.AddListener(OnSelectExited);
    }

    // Update is called once per frame
    void Update()
    {
        while (isSelected)
        {
            joint.position = hand.position;
        }
    }

    void OnSelectEntered(XRBaseInteractable interactable)
    {
        isSelected = true;
    }

    void OnSelectExited(XRBaseInteractable interactable)
    {
        isSelected = false;
    }

}

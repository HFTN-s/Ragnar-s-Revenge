using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PuzzleSocket : MonoBehaviour
{
    public int socketID;
    private Vector3 resetVector = new Vector3(0, 0, 0);

    public AudioSource socketAudio;
    public AudioClip pieceAccepted;
    public AudioClip pieceRejected;
    private JigsawController jigsawController;
    public bool isPlaced;

    void Start()
    {
        jigsawController = GameObject.Find("JigsawPuzzleSystem").GetComponent<JigsawController>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "JigsawPiece")
        {
            int pieceIDNumber = other.GetComponent<Rb_AddForce>().pieceID;
            //if piece ID and socketID are the same
            if (pieceIDNumber == socketID)
            {
                PieceAccepted(other.gameObject);
            }
            else
            {
                StartCoroutine(DisableGrabInteractable(other.gameObject));
            }
        }
    }

    //method to disable xr grab interactble then reenable after a delay
    IEnumerator DisableGrabInteractable(GameObject piece)
    {
        //play piece rejected audio
        //*socketAudio.PlayOneShot(pieceRejected);

        //disable xr grab interactable
        piece.GetComponent<XRGrabInteractable>().enabled = false;
        yield return new WaitForSeconds(2);
        //enable xr grab interactable
        piece.GetComponent<XRGrabInteractable>().enabled = true;
    }

    void PieceAccepted(GameObject piece)
    {
        //play piece accepted audio
        //*socketAudio.PlayOneShot(pieceAccepted);
        
        //disable xr grab interactable
        piece.GetComponent<XRGrabInteractable>().enabled = false;
        //set piece to kinematic
        piece.GetComponent<Rigidbody>().isKinematic = true;
        //set local position to zero
        piece.transform.localPosition = resetVector;
        //set local rotation to zero
        piece.transform.localRotation = Quaternion.Euler(resetVector);
        isPlaced = true;
        //activate function in jigsaw controller to check how many are completed
        jigsawController.CheckCompletedPuzzle();

    }

}
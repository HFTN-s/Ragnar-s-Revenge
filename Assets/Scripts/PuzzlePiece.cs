using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class PuzzlePiece : MonoBehaviour
{
    public int pieceID;  // Unique identifier for each puzzle piece
    public bool IsPlaced { get; private set; } = false;
    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;
    private Collider[] colliders;
    private AudioSource audioSource;
    public AudioClip pickupSound;
    public AudioClip placeSound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        grabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
        colliders = GetComponents<Collider>();
        //Debug.Log($"PuzzlePiece {pieceID} initialized.");

        // add listener to the on select enter event
        grabInteractable.onSelectEnter.AddListener(OnSelectEnter);
    }

    public void PlacePiece(Transform slotTransform)
    {
        IsPlaced = true;

        // Disable interactions and make the piece kinematic
        grabInteractable.enabled = false;
        rb.isKinematic = true;
        foreach (var col in colliders)
        {
            col.enabled = false;
        }

        // Set position and rotation
        transform.rotation = Quaternion.Euler(-90,0,0);
        transform.position = new Vector3(0,0,0);
        

        // set local position and rotation
        transform.localRotation = Quaternion.Euler(-90,0,0);
        transform.localPosition = new Vector3(0,0,0);
        


        //Debug.Log($"PuzzlePiece {pieceID} placed in slot {slotTransform.name} with position set to {slotTransform.position} and rotation set to {slotTransform.rotation.eulerAngles}.");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsPlaced && collision.gameObject.CompareTag("Ground"))
        {
            // Apply a small force to the puzzle piece to ensure it is not kept upright
            Vector3 forceDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
            rb.AddForce(forceDirection * 10f, ForceMode.Impulse);

            //Debug.Log($"PuzzlePiece {pieceID} hit the ground and was given a small force.");
            //wait 2 seconds before allowing force again
            StartCoroutine(EnableForce());
        }
    }

    private IEnumerator EnableForce()
    {
        yield return new WaitForSeconds(2f);
        //Debug.Log($"PuzzlePiece {pieceID} can now be given force again.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PuzzleSlot"))
        {
            PuzzleSlot slot = other.GetComponent<PuzzleSlot>();
            if (slot != null && !IsPlaced && slot.slotID == pieceID)
            {
                PlacePiece(other.transform);
                FindObjectOfType<PuzzleController>().PiecePlaced();
                //Debug.Log($"PuzzlePiece {pieceID} correctly placed in slot {slot.slotID}.");
                audioSource.PlayOneShot(placeSound);
            }
            else if (slot != null && !IsPlaced && slot.slotID != pieceID)
            {
                //Debug.Log($"PuzzlePiece {pieceID} entered incorrect slot {slot.slotID}. Disabling interactable temporarily.");
                StartCoroutine(TemporarilyDisableInteractable());
            }
        }
    }

    private IEnumerator TemporarilyDisableInteractable()
    {
        grabInteractable.enabled = false;
        yield return new WaitForSeconds(1f);
        grabInteractable.enabled = true;
        //Debug.Log($"PuzzlePiece {pieceID} interactable re-enabled.");
    }


    private void OnSelectEnter(XRBaseInteractor interactor)
    {
        audioSource.PlayOneShot(pickupSound);
    }
}

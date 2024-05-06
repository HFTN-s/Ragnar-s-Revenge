using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JigsawController : MonoBehaviour
{
    public GameObject gemParent;
    public GameObject jigsawParent;
    public GameObject jigsawSlotParent;
    public GameObject door;
    private GameObject[] gems;
    private GameObject[] jigsawPieces;
    private GameObject[] jigsawSlots;
    public Material gemMaterial;
    public Material jigsawMaterial;
    public int completedPieces;

    void Start()
    {
        InitializePuzzle();
    }

    void InitializePuzzle()
    {
        // Ensure parents are assigned
        if (!gemParent || !jigsawParent || !jigsawSlotParent)
        {
            Debug.LogWarning("Please assign all parent objects in the Inspector");
            return;
        }

        // Fill arrays with corresponding child objects
        gems = GetChildren(gemParent);
        jigsawPieces = GetChildren(jigsawParent);
        jigsawSlots = GetChildren(jigsawSlotParent);

        // Set materials for gems
        foreach (GameObject gem in gems)
        {
            SetGemMaterial(gem);
        }

        // Optionally set materials for jigsaw pieces
        foreach (GameObject piece in jigsawPieces)
        {
            SetJigsawMaterial(piece);
        }

        // Activate box colliders after a delay
        StartCoroutine(ActivateBoxColliders());
    }

    // Helper method to get children as an array
    GameObject[] GetChildren(GameObject parent)
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in parent.transform)
        {
            children.Add(child.gameObject);
        }
        return children.ToArray();
    }

    // Assigns material to a gem
    void SetGemMaterial(GameObject gem)
    {
        Renderer renderer = gem.GetComponent<Renderer>();
        if (renderer != null && gemMaterial != null)
        {
            renderer.material = gemMaterial;
        }
    }

    // Assigns material to a jigsaw piece
    void SetJigsawMaterial(GameObject piece)
    {
        Renderer renderer = piece.GetComponent<Renderer>();
        if (renderer != null && jigsawMaterial != null)
        {
            renderer.material = jigsawMaterial;
        }
    }

    // Coroutine to activate box colliders on jigsaw slots
    private IEnumerator ActivateBoxColliders()
    {
        // Wait for 3 seconds before continuing
        yield return new WaitForSeconds(3);

        // Enable box colliders on all jigsaw slots
        foreach (GameObject slot in jigsawSlots)
        {
            BoxCollider collider = slot.GetComponent<BoxCollider>();
            if (collider != null)
            {
                collider.enabled = true;
            }
        }
    }

    // Function to check how many pieces are in the correct position
    public void CheckCompletedPuzzle()
    {
        completedPieces = 0;
        foreach (GameObject slot in jigsawSlots)
        {
            PuzzleSocket socket = slot.GetComponent<PuzzleSocket>();
            if (socket.isPlaced)
            {
                completedPieces++;
            }
        }

        // If all pieces are in the correct position, do something
        if (completedPieces == jigsawSlots.Length)
        {
            Debug.Log("Puzzle completed!");
            Puzzle1Completed();
        }
    }

    void Puzzle1Completed()
    {
        Debug.Log("Puzzle 1 completed!");
        MoveDoorUpwards();
    }

    void MoveDoorUpwards()
    {
        // Move the door upwards
        door.transform.Translate(Vector3.up * 5);
    }
}

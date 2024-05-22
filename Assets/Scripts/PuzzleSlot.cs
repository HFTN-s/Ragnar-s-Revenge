using UnityEngine;

public class PuzzleSlot : MonoBehaviour
{
    public int slotID;  // Unique identifier for each slot

    private void OnTriggerEnter(Collider other)
    {
        PuzzlePiece piece = other.GetComponent<PuzzlePiece>();
        if (piece != null)
        {
            Debug.Log($"PuzzlePiece {piece.pieceID} entered slot {slotID}.");
            if (!piece.IsPlaced && piece.pieceID == slotID)
            {
                piece.PlacePiece(transform);
                FindObjectOfType<PuzzleController>().PiecePlaced();
                Debug.Log($"PuzzlePiece {piece.pieceID} correctly placed in slot {slotID}.");
            }
        }
    }
}

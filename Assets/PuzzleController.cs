using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    public int totalPieces;
    private int piecesInPlace = 0;
    public GameObject rune;
    public GameObject closet;
    public string correctOrder = "1234";
    private string currentOrder = "";
    private int count = 0;

    void Start()
    {
        Debug.Log("PuzzleController initialized.");
    }

    public void PiecePlaced()
    {
        piecesInPlace++;
        Debug.Log($"Piece placed. Total pieces in place: {piecesInPlace}/{totalPieces}");
        if (piecesInPlace >= totalPieces)
        {
            CompletePuzzle();
        }
    }

    private void CompletePuzzle()
    {
        Debug.Log("Puzzle completed! Activating rune, and waiting for correct order of runes pressed.");
        rune.SetActive(true);  // Activate rune
    }

    private int NextIntNeeded(int count)
    {
        if (count < correctOrder.Length)
        {
            return int.Parse(correctOrder[count].ToString());
        }
        return -1;
    }

    private bool CheckOrder()
    {
        if (count < currentOrder.Length && NextIntNeeded(count) != int.Parse(currentOrder[count].ToString()))
        {
            count = 0;
            currentOrder = "";
            Debug.Log("Wrong order. Resetting count and current order.");
            return false;
        }
        return true;
    }

    public void RunePressed(string runeID)
    {
        currentOrder += runeID;
        Debug.Log($"Rune pressed: {runeID}. Current order: {currentOrder}");
        DisableColliders();
        if (CheckOrder())
        {
            count++;
            Debug.Log($"Correct order. Count: {count}");
            if (count == correctOrder.Length)
            {
                Debug.Log("Correct order entered. Opening closet.");
                closet.SetActive(true);
            }
        }
    }

    public void DisableColliders()
    {
        foreach (Transform child in rune.transform)
        {
            child.GetComponent<Collider>().enabled = false;
        }
        Invoke("EnableColliders", 3f);
    }

    public void EnableColliders()
    {
        foreach (Transform child in rune.transform)
        {
            child.GetComponent<Collider>().enabled = true;
        }
    }
}

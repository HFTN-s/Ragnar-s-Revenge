using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    public int totalPieces;
    private int piecesInPlace = 0;
    public GameObject runeParent;
    private GameObject[] rune = new GameObject[26];
    public GameObject closetDoor;
    public string correctOrder = "1234";
    private string currentOrder = "";
    private int count = 0;
    private GameObject[] runeLetters = new GameObject[26];
    private GameObject[] gemstones = new GameObject[26];
    public GameObject gemstoneParent;

    void Start()
    {
        Debug.Log("PuzzleController initialized.");
        //loop children of runeParent and add to rune array, then add child of each rune to runeLetters array
        for (int i = 0; i < runeParent.transform.childCount; i++)
        {
            rune[i] = runeParent.transform.GetChild(i).gameObject;
            // change rune id to be the same as the index + 1
            rune[i].GetComponent<TouchRune>().runeID = i + 1;
            runeLetters[i] = rune[i].transform.GetChild(0).gameObject;
        }

        //loop children of gemstoneParent and add to gemstones array
        for (int i = 0; i < gemstoneParent.transform.childCount; i++)
        {
            gemstones[i] = gemstoneParent.transform.GetChild(i).gameObject;
        }
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
        rune[1].SetActive(true);  // Activate rune
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
            // remove glow effect from runes
            return false;
        }
        return true;
    }

    public void RunePressed(string runeID)
    {
        currentOrder += runeID;
        Debug.Log($"Rune pressed: {runeID}. Current order: {currentOrder}");
        //Add glow effect to rune
        DisableColliders();
        if (CheckOrder())
        {
            count++;
            Debug.Log($"Correct order. Count: {count}");
            if (count == correctOrder.Length)
            {
                Debug.Log("Correct order entered. Opening closet.");
                // Rotate door
                closetDoor.SetActive(false);
                // Play sound on door
                if (closetDoor.GetComponent<AudioSource>() != null)
                {
                    closetDoor.GetComponent<AudioSource>().Play();
                }
            }
        }
    }

    public void DisableColliders()
    {
        //disable colliders on all gemstones in gemstones array
        foreach (GameObject gemstone in gemstones)
        {
            Collider collider = gemstone.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
            }
        }
        Invoke("EnableColliders", 3f);
    }

    public void EnableColliders()
    {
        foreach (GameObject gemstone in gemstones)
        {
            Collider collider = gemstone.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = true;
            }
        }
    }

    // hit by laser function
    // public void HitByLaser(string gemstoneName)
    // {
    //     Debug.Log("Gemstone hit by laser: " + gemstoneName);
    //  Add Emissive to gemstone

    // Add Emissive Function

    // public void AddEmissive()

    // get child object of rune (Letter) and get the relevant material

    // add intensity to the emissive material

    // Remove Emissive Function

    // public void RemoveEmissive()

    // get child object of all rune objects and get the relevant material (maybe use a reference to the objects rather then get them each time)

    // remove intensity from the emissive material
}

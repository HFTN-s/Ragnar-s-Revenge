using UnityEngine;

public class TouchRune : MonoBehaviour
{
    public int runeID;
    private PuzzleController puzzleController;

    void Start()
    {
        //Get component of PuzzleController from JigsawPuzzleSystem
        puzzleController = GameObject.Find("JigsawPuzzleSystem").GetComponent<PuzzleController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
        {
            Debug.Log("Player has touched the rune: " + runeID);
            if (puzzleController != null)
            {
                puzzleController.RunePressed(runeID); 
            }
            else
            {
                Debug.LogError("PuzzleController reference is null when trying to call RunePressed.");
            }
        }
    }
}

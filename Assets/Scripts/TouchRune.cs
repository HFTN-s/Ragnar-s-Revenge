using UnityEngine;

public class TouchRune : MonoBehaviour
{
    public int runeID;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
        {
            Debug.Log("Player has touched the rune: " + runeID);
            // Send message to the parent object that it has been touched
            transform.parent.SendMessage("RunePressed", runeID.ToString());
        }
    }
}

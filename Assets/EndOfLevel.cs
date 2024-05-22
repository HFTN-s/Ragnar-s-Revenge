using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfLevel : MonoBehaviour
{
    public int nextIndex;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("End of Level collision detected");
            //load next level
            SceneManager.LoadScene(nextIndex);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
public class TutorialLevelTrigger : MonoBehaviour
{
[SerializeField] GameObject stoneStatue;
[SerializeField] GameObject ghostStatue;

    private void Start()
    {
        ghostStatue.SetActive(false);
    }

    async void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            await Task.Delay(3 * 1000); // 1 second delay
            stoneStatue.SetActive(false);
            ghostStatue.SetActive(true);
            SceneManager.LoadScene("TutorialLevel");
        }
    }


}

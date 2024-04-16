using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForgeFuel : MonoBehaviour
{
    public int fuelAmount = 0;
    private bool isBurning = false;
    private Vector3 startPosition;
    public GameObject resetPosition;
    // Start is called before the first frame update

    void Start()
    {
        startPosition = resetPosition.transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Metal" && !isBurning)
        {
            isBurning = true;
            Debug.Log("Fuel added to forge. Current fuel amount: " + fuelAmount);
            // wait .5 seconds
            StartCoroutine(FuelWait());
            fuelAmount++;
            other.transform.position = startPosition;
            isBurning = false;
            Debug.Log("Forge is now burning.");

        }
    }

    IEnumerator FuelWait()
    {
        yield return new WaitForSeconds(2f);
    }
}

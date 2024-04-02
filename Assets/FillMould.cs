using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Fills mould with relevant prefab object by scaling all axis from 0.01 to 1
// This script is attached to the mould object
// The mouldFill prefab is the object that will be filled in the mould
// The endPosition is the position where the mould will be filled
// The speed is the speed at which the mould will be filled
public class FillMould : MonoBehaviour
{
    public GameObject mouldFill;
    public float scaleIncrease = 0.01f;
    public bool isFilling = false;
    public bool isActivated = false;

    public void DoFillMould()
    {
        if (isFilling) return;
        StartCoroutine(ScaleMould());
    }

    IEnumerator ScaleMould()
    {
        // scales the mouldFill object from 0.01 to 1 on all axis smoothly
        Debug.Log("Filling mould");
        isFilling = true;
        while (mouldFill.transform.localScale.x < 1)
        {
            mouldFill.transform.localScale += new Vector3(scaleIncrease, scaleIncrease, scaleIncrease);
            yield return new WaitForSeconds(0.01f);
        }
        isFilling = false;
    }

    public void PauseScaleMould()
    {
        StopCoroutine(ScaleMould());
        isFilling = false;
    }
}

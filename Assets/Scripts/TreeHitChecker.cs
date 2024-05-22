using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class TreeHitChecker : MonoBehaviour
{
    public string axeName;
    public UnityEvent puzzle1CompletedEvent;
    [SerializeField] private float pushForce = 1000f;

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"[OnCollisionEnter] Collision detected with {collision.gameObject.name}, tagged as {collision.gameObject.tag}");

        if (collision.gameObject.CompareTag("Axe"))
        {
            Debug.Log("[OnCollisionEnter] Blade collision detected");

            GameObject axe = collision.gameObject;
            Debug.Log($"[OnCollisionEnter] Axe object: {axe.name}");

            if (axe.name == axeName)
            {
                Debug.Log("[OnCollisionEnter] Correct axe name");

                AxeCheck axeCheck = axe.GetComponent<AxeCheck>();
                if (axeCheck != null)
                {
                    Debug.Log($"[OnCollisionEnter] Axe sharpness: {axeCheck.sharp}");

                    HandleAxe(axe, axeCheck.sharp);
                }
                else
                {
                    Debug.Log("[OnCollisionEnter] AxeCheck script not found");
                    HandleIncorrectAxe(axe);
                }
            }
            else
            {
                Debug.Log("[OnCollisionEnter] Incorrect axe name");
                HandleIncorrectAxe(axe);
            }
        }
        else
        {
            Debug.Log($"[OnCollisionEnter] Not a blade collision. Tag: {collision.gameObject.tag}");
        }
    }

    private void HandleAxe(GameObject axe, int sharpness)
    {
        Debug.Log("[HandleAxe] Handling axe with sharpness " + sharpness);
        StartCoroutine(HandleAxeWithSharpness(axe, sharpness));
    }

    private IEnumerator HandleAxeWithSharpness(GameObject axe, int sharpness)
    {
        XRGrabInteractable axeInteractable = axe.GetComponent<XRGrabInteractable>();
        Rigidbody axeRigidbody = axe.GetComponent<Rigidbody>();

        if (axeInteractable != null)
        {
            axeInteractable.enabled = false;
            Debug.Log("[HandleAxeWithSharpness] XRGrabInteractable disabled");
        }
        else
        {
            Debug.LogError("[HandleAxeWithSharpness] XRGrabInteractable component not found on axe. Cannot disable.");
        }

        if (sharpness < 3)
        {
            int waitTime = sharpness + 1;
            Debug.Log($"[HandleAxeWithSharpness] Waiting for {waitTime} seconds");
            yield return new WaitForSeconds(waitTime);

            if (axeRigidbody != null)
            {
                axeRigidbody.constraints = RigidbodyConstraints.None;
                Debug.Log("[HandleAxeWithSharpness] Rigidbody constraints set to None");
            }
            else
            {
                Debug.LogError("[HandleAxeWithSharpness] Rigidbody component not found on axe.");
            }
        }
        else
        {
            if (axeRigidbody != null)
            {
                axeRigidbody.constraints = RigidbodyConstraints.FreezeAll;
                Debug.Log("[HandleAxeWithSharpness] Rigidbody constraints set to FreezeAll");
            }
            else
            {
                Debug.LogError("[HandleAxeWithSharpness] Rigidbody component not found on axe.");
            }

            puzzle1CompletedEvent.Invoke();
            Debug.Log("[HandleAxeWithSharpness] Puzzle 1 completed with the correct axe.");
        }
    }

    private void HandleIncorrectAxe(GameObject axe)
    {
        Debug.Log("[HandleIncorrectAxe] Handling incorrect axe");
        XRGrabInteractable axeInteractable = axe.GetComponent<XRGrabInteractable>();

        if (axeInteractable != null)
        {
            axeInteractable.enabled = false;
            Debug.Log("[HandleIncorrectAxe] XRGrabInteractable disabled on incorrect axe");
        }
        else
        {
            Debug.LogError("[HandleIncorrectAxe] XRGrabInteractable component not found on axe. Cannot disable.");
        }

        AddForceToAxe(axe);
        StartCoroutine(EnableXRGrabInteractable(axe));
        Debug.Log("[HandleIncorrectAxe] Incorrect or dull axe used. Adding force and enabling XRGrabInteractable after delay.");
    }

    private IEnumerator EnableXRGrabInteractable(GameObject axe)
    {
        Debug.Log("[EnableXRGrabInteractable] Waiting to re-enable XRGrabInteractable");
        yield return new WaitForSeconds(2);
        XRGrabInteractable axeInteractable = axe.GetComponent<XRGrabInteractable>();
        if (axeInteractable != null)
        {
            axeInteractable.enabled = true;
            Debug.Log("[EnableXRGrabInteractable] XRGrabInteractable re-enabled");
        }
        else
        {
            Debug.LogError("[EnableXRGrabInteractable] XRGrabInteractable component not found on axe during re-enable.");
        }
    }

    private void AddForceToAxe(GameObject axe)
    {
        Debug.Log("[AddForceToAxe] Adding force to axe");
        Rigidbody axeRigidbody = axe.GetComponent<Rigidbody>();
        if (axeRigidbody != null)
        {
            axeRigidbody.isKinematic = false;
            axeRigidbody.AddForce(-transform.forward * pushForce);
            Debug.Log("[AddForceToAxe] Force added to axe");
        }
        else
        {
            Debug.LogError("[AddForceToAxe] Rigidbody component not found on axe.");
        }
    }
}

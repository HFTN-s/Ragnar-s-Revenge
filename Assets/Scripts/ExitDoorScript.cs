using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;


public class ExitDoorScript : MonoBehaviour
{
    public bool isUnlocked;
    public GameObject hammer;
    public float doorForce = 1000f;
    public AudioSource doorAudio;
    public AudioClip doorHit;
    private TutorialLevelManager levelManager;
    public GameObject doorPadlock;
    private void Awake()
    {
        hammer = GameObject.Find("Hammer");
    }

    private void Start()
    {
        levelManager = GameObject.Find("TutorialLevelManager").GetComponent<TutorialLevelManager>();
        isUnlocked = true;
        Destroy(doorPadlock);
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hammer")
        {
            Rigidbody hammerRigidbody = other.GetComponent<Rigidbody>();
            bool hammerIsMovingFast = hammerRigidbody.velocity.magnitude > 2;


            if (isUnlocked)
            {
                Debug.Log("Hammer has touched the door and it is unlocked.");
                //unfreeze rigidbody constraints
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                //doorAudio.clip = doorHit;
                doorAudio.Play();
                HingeJoint hinge = GetComponent<HingeJoint>();
                // add force
                GetComponent<Rigidbody>().AddForce(Vector3.forward * doorForce * -1);
                if (hinge != null)
                {
                    doorAudio.Play();
                    GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;


                    JointSpring spring = new JointSpring { spring = 10, damper = 1, targetPosition = -90 };
                    hinge.spring = spring;
                    hinge.useSpring = true;
                }
                else
                {
                    Debug.LogError("No HingeJoint component found on the door.");
                }
            }
            else if (!isUnlocked)
            {
                Debug.Log("The door is locked. The hammer cannot open it.");
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

                hammer.GetComponent<XRGrabInteractable>().enabled = false;
                StartCoroutine(Wait());

            }
        }
    }

    void OnUnlock()
    {
        levelManager.PlayJarlVoiceLine(4);
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
        hammer.GetComponent<XRGrabInteractable>().enabled = true;
    }
}
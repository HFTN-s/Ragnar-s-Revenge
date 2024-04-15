using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private InputDevice Head;
    private InputDevice leftHandController;
    private InputDevice rightHandController;
    [SerializeField] private Camera camera;

    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float rotationAmount = 2.0f;
    private Vector2 turnInput;
    public bool canMove = false;

    void Start()
    {
        SetupHead();
        SetupLeftHandController();
        SetupRightHandController();
        // if current scene is main menu scene, player is not allowed to move
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            canMove = false;
        }
        else
        {
            canMove = true;
        }
    }

    void SetupHead()
    {
        Head = InputDevices.GetDeviceAtXRNode(XRNode.Head);
        if (!Head.isValid)
        {
           // Debug.Log("Head device not found. Retrying...");
            Invoke("SetupHead", 0.7f); // Retry after a delay
        }
        else
        {
            Debug.Log("Head device found");
        }
    }

    void SetupLeftHandController()
    {
        leftHandController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        if (!leftHandController.isValid)
        {
           // Debug.Log("Left hand controller not found. Retrying...");
            Invoke("SetupLeftHandController", 0.7f); // Retry after a delay
        }
        else
        {
            Debug.Log("Left hand controller found");
        }
    }
    void SetupRightHandController()
    {
        rightHandController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        if (!rightHandController.isValid)
        {
          //  Debug.Log("Right hand controller not found. Retrying...");
            Invoke("SetupRightHandController", 0.7f); // Retry after a delay
        }
        else
        {
            Debug.Log("Right hand controller found");
        }
    }

    void FixedUpdate()
    {   
        //if (!leftHandController.isValid) return; // Exit if controller is not valid
        //if (!rightHandController.isValid) return; // Exit if controller is not valid
        if (!canMove) return; // Exit if player is not allowed to move

            //if player presses left stick to left or right rotate player once until stick is released
               // Debug.Log("Attempting to rotate player");
                    RotatePlayer();

            // if player presses primary button, move player forward
            if (rightHandController.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryButtonValue) && secondaryButtonValue)
            {
                transform.position += Vector3.ProjectOnPlane(camera.transform.forward, Vector3.up).normalized * speed * Time.deltaTime;
            }
            // if player moves right stick backwards, move player backwards
             if (rightHandController.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue) && primaryButtonValue)
            {
                transform.position -= Vector3.ProjectOnPlane(camera.transform.forward, Vector3.up).normalized * speed * Time.deltaTime;
            }
    }   

   void RotatePlayer()
    {
        //Use right stick to rotate player
        if (rightHandController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisValue) && primary2DAxisValue != Vector2.zero)
        {
            turnInput = primary2DAxisValue;
            Vector3 turn = new Vector3(0,turnInput.x,0);
            transform.rotation = Quaternion.Slerp(transform.rotation, transform.rotation * Quaternion.Euler(turn * rotationAmount), Time.deltaTime * 10);
        }

        
    }

}

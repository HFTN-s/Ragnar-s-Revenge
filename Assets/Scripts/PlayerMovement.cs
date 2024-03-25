using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private InputDevice leftHandController;
    private InputDevice rightHandController;
    [SerializeField] private Camera camera;

    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float rotationAmount = 2.0f;
    private Vector2 turnInput;
    public bool canMove = false;

    void Start()
    {
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

    void SetupLeftHandController()
    {
        leftHandController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        if (!leftHandController.isValid)
        {
            Debug.Log("Left hand controller not found. Retrying...");
            Invoke("SetupLeftHandController", 0.5f); // Retry after a delay
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
            Debug.Log("Right hand controller not found. Retrying...");
            Invoke("SetupRightHandController", 0.5f); // Retry after a delay
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
            if (rightHandController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 xJoystickValue))
            {
                Debug.Log("Attempting to rotate player");
                if (xJoystickValue.x > 0.5f || xJoystickValue.x < -0.5f)
                {
                    Debug.Log("Rotating player");
                    RotatePlayer(xJoystickValue);
                }
            }

            // if player presses primary button, move player forward
            if (rightHandController.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue) && primaryButtonValue)
            {
                transform.position += Vector3.ProjectOnPlane(camera.transform.forward, Vector3.up).normalized * speed * Time.deltaTime;
            }
            // if player moves right stick backwards, move player backwards
            if (rightHandController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 yJoystickValue))
            {
                if (yJoystickValue.y < -0.5f)
                {
                    transform.position -= Vector3.ProjectOnPlane(camera.transform.forward, Vector3.up).normalized * speed * Time.deltaTime;
                }
            }
    }   

    void RotatePlayer(Vector2 joystickValue)
    {
        if (joystickValue.x > 0.5f)
        {
            transform.Rotate(Vector3.up, rotationAmount);
        }
        else if (joystickValue.x < -0.5f)
        {
            transform.Rotate(Vector3.up, -rotationAmount);
        }
    }
}

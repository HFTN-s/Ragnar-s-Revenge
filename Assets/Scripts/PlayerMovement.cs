using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PlayerMovement : MonoBehaviour
{
    private InputDevice Head;
    public InputDevice leftHandController;
    public InputDevice rightHandController;
    [SerializeField] private Camera camera;

    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float rotationAmount = 2.0f;
    private Vector2 turnInput;
    public bool canMove = false;
    public AudioSource playerSFX;
    public Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
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
            // //Debug.Log("Head device not found. Retrying...");
            Invoke("SetupHead", 0.7f); // Retry after a delay
        }
        else
        {
            //Debug.Log("Head device found");
        }
    }

    void SetupLeftHandController()
    {
        leftHandController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        if (!leftHandController.isValid)
        {
            // //Debug.Log("Left hand controller not found. Retrying...");
            Invoke("SetupLeftHandController", 0.7f); // Retry after a delay
        }
        else
        {
            //Debug.Log("Left hand controller found");
        }
    }
    void SetupRightHandController()
    {
        rightHandController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        if (!rightHandController.isValid)
        {
            //  //Debug.Log("Right hand controller not found. Retrying...");
            Invoke("SetupRightHandController", 0.7f); // Retry after a delay
        }
        else
        {
            //Debug.Log("Right hand controller found");
        }
    }

    void FixedUpdate()
    {
        RotatePlayer(rb);
        // if player presses left stick forward or backward, move player along the camera's forward vector
        if (leftHandController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisValue))
        {
            if (!canMove)
            {
                playerSFX.Stop();
                return;
            }
            // if SFX is not playing,            play the footstep sound
            // Determine the direction and magnitude of the movement
            Vector3 moveDirection = camera.transform.forward * primary2DAxisValue.y * speed * Time.fixedDeltaTime;
            // Ensure we are not moving vertically if the Y axis is locked
            moveDirection.y = 0;

            // Apply the movement to the Rigidbody
            rb.MovePosition(rb.position + moveDirection);
            //Debug.Log("Player is moving");
            //Debug.Log("Player is moving at: " + rb.velocity.magnitude + " m/s");
            //Debug.Log("playerSFX.isPlaying: " + playerSFX.isPlaying);
            if (!playerSFX.isPlaying && moveDirection != Vector3.zero)
            {
                //Debug.Log("Playing footstep sound");
                playerSFX.Play(); // play footstep sound if player is moving
            }
            else if (playerSFX.isPlaying && moveDirection == Vector3.zero)
            {
                //Debug.Log("Stopping footstep sound");
                playerSFX.Stop(); // stop playing footstep sound if player is not moving
            }

            // if player presses in left stick slow down time scale by 0.1 unless already 0.1 , same with right stick to increase time scale
            if (leftHandController.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bool primary2DAxisClickValue))
            {
                if (primary2DAxisClickValue)
                {
                    if (Time.timeScale > 0.1f)
                    {
                        Time.timeScale -= 0.1f;
                    }
                }
            }

            if (rightHandController.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bool primary2DAxisClickValueR))
            {
                if (primary2DAxisClickValueR)
                {
                    if (Time.timeScale < 1.0f)
                    {
                        Time.timeScale += 0.1f;
                    }
                }
            }

            else
            {
                //Debug.Log("Footstep sound is already playing");
            }
        }
    }

    void RotatePlayer(Rigidbody rb)
    {
        if (rightHandController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisValue))
        {
            // The turn value is the input value scaled by rotation amount.
            float turn = primary2DAxisValue.x * rotationAmount;

            // Calculate the new rotation by applying the turn to the current rotation around the Y axis.
            Quaternion newRotation = Quaternion.Euler(0f, turn, 0f) * rb.rotation;

            // Smoothly rotate the player by interpolating to the new rotation.
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, newRotation, Time.fixedDeltaTime * 10));
        }
    }

}

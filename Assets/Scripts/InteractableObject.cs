using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class InteractableObject : MonoBehaviour
{
    private Renderer _renderer;
    public Material glowMaterial;
    private Rigidbody _rigidbody;
    private Material _defaultMaterial;
    private bool _isGlowing = false;
    public float detectionHeight = 2.0f;
    public float lerpSpeed = 0.1f;

    private void Awake()
    {
        // Initialize components
        _renderer = GetComponent<Renderer>();
        _defaultMaterial = _renderer.material;
        _rigidbody = GetComponent<Rigidbody>();
        InitGlowMaterial();

        // Add listeners
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.onSelectEntered.AddListener((interactor) => OnActivate(interactor));
        grabInteractable.onSelectExited.AddListener((interactor) => OnDeactivate(interactor));
    }


    private void InitGlowMaterial()
    {
        if (glowMaterial == null)
        {
            glowMaterial = new Material(Shader.Find("Standard"));
            glowMaterial.EnableKeyword("_EMISSION");
            glowMaterial.SetColor("_EmissionColor", Color.yellow);
        }
    }

    private void Update()
    {
        PerformInteractionCheck();
    }

    private void PerformInteractionCheck()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.up, out hit, detectionHeight))
        {
            DebugRay(true);
            if (hit.collider.CompareTag("PlayerHand"))
            {
                if (!_isGlowing)
                {
                    SetGlow(true);
                }
                FollowHand(hit.collider.transform);
            }
        }
        else if (_isGlowing)
        {
            DebugRay(false);
            SetGlow(false);
        }
    }

    private void FollowHand(Transform handTransform)
    {
        // Only update the y position to follow the hand, keep x and z the same.
        Vector3 newPosition = new Vector3(transform.position.x, handTransform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, newPosition, lerpSpeed);

        // Check if the object is close enough to the hand position to stop moving
        if (Mathf.Abs(transform.position.y - handTransform.position.y) < 0.1f)
        {
            _rigidbody.isKinematic = false;
        }
    }


    private void SetGlow(bool status)
    {
        _isGlowing = status;
        _renderer.material = status ? glowMaterial : _defaultMaterial;
        _rigidbody.useGravity = !status;
    }


    private void DebugRay(bool hit)
    {
#if UNITY_EDITOR
        Color rayColor = hit ? Color.red : Color.green;
        Debug.DrawRay(transform.position, Vector3.up * detectionHeight, rayColor);
#endif
    }

    public void OnActivate(XRBaseInteractor interactor)
    {
        Debug.Log("Object has been activated");
    }

    public void OnDeactivate(XRBaseInteractor interactor)
    {
        Debug.Log("Object has been deactivated");
        //delete script from object
        Destroy(this);
    }


}
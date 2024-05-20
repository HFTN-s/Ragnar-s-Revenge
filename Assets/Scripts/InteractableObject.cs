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
        if (_renderer != null)
        {
            _defaultMaterial = _renderer.material;
        }

        _rigidbody = GetComponent<Rigidbody>();
        InitGlowMaterial();

        // Add listeners
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.onSelectEntered.AddListener(OnActivate);
            grabInteractable.onSelectExited.AddListener(OnDeactivate);
        }
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
        if (_renderer == null) return;

        RaycastHit hit;
        Vector3 rayOrigin = _renderer.bounds.center;
        Vector3 rayDirection = Vector3.up; // Ensure the ray direction is in world space

        bool isHit = Physics.Raycast(rayOrigin, rayDirection, out hit, detectionHeight);
        DebugRay(rayOrigin, rayDirection, isHit);

        if (isHit)
        {
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
            SetGlow(false);
        }
    }

    private void FollowHand(Transform handTransform)
    {
        // Convert hand position to local space if necessary
        Vector3 handPosition = handTransform.position;

        // Only update the y position to follow the hand, keep x and z the same.
        Vector3 newPosition = new Vector3(transform.position.x, handPosition.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, newPosition, lerpSpeed);

        // Check if the object is close enough to the hand position to stop moving
        if (Mathf.Abs(transform.position.y - handPosition.y) < 0.1f)
        {
            _rigidbody.isKinematic = false;
        }
    }

    private void SetGlow(bool status)
    {
        _isGlowing = status;
        if (_renderer != null)
        {
            _renderer.material = status ? glowMaterial : _defaultMaterial;
        }
        if (_rigidbody != null)
        {
            _rigidbody.useGravity = !status;
        }
    }

    private void DebugRay(Vector3 origin, Vector3 direction, bool hit)
    {
#if UNITY_EDITOR
        Color rayColor = hit ? Color.red : Color.green;
        Debug.DrawRay(origin, direction * detectionHeight, rayColor);
#endif
    }

    public void OnActivate(XRBaseInteractor interactor)
    {
        Debug.Log("Object has been activated");
    }

    public void OnDeactivate(XRBaseInteractor interactor)
    {
        Debug.Log("Object has been deactivated");
        Destroy(this);
    }
}

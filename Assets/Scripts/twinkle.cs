using UnityEngine;

public class twinkle : MonoBehaviour
{
    [SerializeField] private string fireGameObjectName = "fx_fire";
    [SerializeField] private string fireMaterialStartName = "mat_torch_01";
    
    private Light torchLight;
    private Renderer fireRenderer;
    private Material fireMaterial;
    
    private float baseIntensity = 1.0f;
    private float flickerVariation = 0.5f;
    private float emissionStrength = 2.3f;
    private float noiseSeed; // Unique seed for Perlin noise

    void Start()
    {
        fireRenderer = GetComponentInChildren<Renderer>(true);
        torchLight = GetComponentInChildren<Light>(true);

        if (fireRenderer != null)
        {
            foreach (Material mat in fireRenderer.materials)
            {
                if (mat.name.StartsWith(fireMaterialStartName))
                {
                    fireMaterial = mat;
                    break;
                }
            }
        }

        if (torchLight != null)
        {
            baseIntensity = torchLight.intensity;
        }

        // Initialize a unique seed for the Perlin noise based on the torch's position
        noiseSeed = Random.Range(0f, 100f); // Alternatively, use gameObject.transform.position or any unique identifier
    }

    void Update()
    {
        if (torchLight != null && fireRenderer != null)
        {
            SimulateFlickering();
        }
    }

    void SimulateFlickering()
    {
        // Apply the noise seed for uniqueness
        float noise = Mathf.PerlinNoise(noiseSeed, Time.time) * flickerVariation;
        torchLight.intensity = baseIntensity + noise;
        
        if (fireMaterial != null)
        {
            float intensityMultiplier = 1.0f + noise;
            Color emissionColor = new Color(emissionStrength * intensityMultiplier, 
                                            emissionStrength * intensityMultiplier, 
                                            emissionStrength * intensityMultiplier);
            fireMaterial.SetColor("_EmissionColor", emissionColor);
        }
    }
}

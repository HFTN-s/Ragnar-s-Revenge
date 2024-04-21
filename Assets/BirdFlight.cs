using UnityEngine;

public class BirdFlight : MonoBehaviour
{
    public float radius = 5.0f; // Radius of the movement circle
    public float baseSpeed = 0.5f; // Base speed of the movement, set to 0.5 as per your request
    public float elevationRange = 2.0f; // Range of elevation change

    private Vector3 startPosition;
    private float angle = 0.0f;
    private float currentSpeed;
    private float speedAdjustmentFrequency = 1.0f; // How often speed adjusts, in seconds
    private float nextSpeedAdjustmentTime = 0.0f;

    void Start()
    {
        startPosition = transform.position;
        currentSpeed = baseSpeed; // Initialize current speed to base speed
    }

    void Update()
    {
        // Check if it's time to adjust the speed
        if (Time.time >= nextSpeedAdjustmentTime)
        {
            currentSpeed = baseSpeed + Random.Range(-0.1f, 0.1f); // smaller variation
            nextSpeedAdjustmentTime = Time.time + speedAdjustmentFrequency;
        }

        // Increment the angle at the current speed
        angle += currentSpeed * Time.deltaTime;

        // Calculate new position
        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;
        float y = Mathf.Sin(Time.time * 0.5f) * elevationRange; // Smooth and slower elevation change
        Vector3 newPosition = startPosition + new Vector3(x, y, z);

        // Calculate the direction by comparing the new position with the current position
        Vector3 direction = newPosition - transform.position;

        // Make the bird face the direction of movement
        if (direction != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Euler(0, newRotation.eulerAngles.y, 0);
        }

        // Update the bird's position
        transform.position = newPosition;
    }
}

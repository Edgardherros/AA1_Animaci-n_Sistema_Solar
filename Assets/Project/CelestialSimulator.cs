using UnityEngine;

public class CelestialSimulator : MonoBehaviour
{
    [Header("Dynamics setup")]
    public Vector3 initialPosition = new Vector3(1, 0, 0);
    public Vector3 initialVelocity = new Vector3(0, 0, 6.28f);
    public float mass = 3e-6f;
    public float rotationSpeed = 10f;

    private Vector3 velocity;
    private Vector3 currentAcceleration;
    private Vector3 nextAcceleration;


    void Start()
    {
        transform.position = initialPosition;
        velocity = initialVelocity;
        currentAcceleration = Vector3.zero;
        nextAcceleration = Vector3.zero;
    }

    public void PrepareStep()
    {
        currentAcceleration = nextAcceleration;
        nextAcceleration = Vector3.zero;
    }

    public void UpdatePosition(float dt)
    {
        transform.position += velocity * dt + 0.5f * currentAcceleration * dt * dt;
    }

    public void AddForce(Vector3 force)
    {
        nextAcceleration += force / mass;
    }

    public void UpdateVelocity(float dt)
    {
        velocity += 0.5f * (currentAcceleration + nextAcceleration) * dt;
    }
    public void UpdateRotation(float dt)
    {
        transform.Rotate(Vector3.up, rotationSpeed * dt * Time.deltaTime);
    }
}
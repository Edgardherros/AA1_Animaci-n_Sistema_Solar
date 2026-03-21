using UnityEngine;

public class CelestialSimulator : MonoBehaviour
{
    [Header("Earth properties")]

    private Vector3 position;
    private Vector3 velocity;
    private Vector3 accelleration;
    private Vector3 prevAcceleration;

    [Header("Dynamics setup")]

    private float gravityMassConstant = 4 * Mathf.PI * Mathf.PI;
    public Vector3 initialPosition = new Vector3(0, 0);
    public Vector3 initialVelocity = new Vector3(0, 0,0);

    public float mass = 0.01f;

    void Start()
    {
        position = initialPosition;
        velocity = initialVelocity;

        transform.position = position;
    }
    public void UpdateVelocityVerlet(float dt)
    {
       // accelleration = CalculateAcceleration(position);
        transform.position += velocity * dt + 0.5f * accelleration * dt * dt;

        velocity += 0.5f * (prevAcceleration + accelleration) * dt;

        prevAcceleration = accelleration;

        accelleration = Vector3.zero;
    }
    public void AddForce(Vector3 force)
    {
        accelleration += force / mass;
    }
    Vector2 CalculateAcceleration(Vector2 position)
    {

        Vector2 newAcceleration;

        float distanceSquared = position.magnitude * position.magnitude;
        Vector2 unitVecor = position.normalized;
        newAcceleration = -(gravityMassConstant / distanceSquared) * unitVecor;
        return newAcceleration;
    }




}

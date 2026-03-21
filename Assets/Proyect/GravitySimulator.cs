using System.Collections.Generic;
using UnityEngine;

public class GravitySimulator : MonoBehaviour
{
    public const float G = 39.478f;

    [Header("Simulación")]
    [Range(1f, 365)]
    public float timeScale = 1f;

    [Range(1, 40)]
    public int stepsPerUpdate = 5;

    [SerializeField] private CelestialSimulator[] planets; // Los astros que participan en la simulación

    void Start()
    {
        ComputeGravity();
    }

    void FixedUpdate()
    {
        float dt = (Time.fixedDeltaTime * timeScale) / 365.25f;
        float subDt = dt / stepsPerUpdate;

        for (int step = 0; step < stepsPerUpdate; step++)
        {
            foreach (var body in planets) body.PrepareStep();
            
            foreach (var body in planets) body.UpdatePosition(subDt);
            
            ComputeGravity();

            foreach (var body in planets) body.UpdateVelocity(subDt);
        }
    }

    private void ComputeGravity()
    {
        for (int i = 0; i < planets.Length; i++)
        {
            for (int j = i + 1; j < planets.Length; j++)
            {
                CelestialSimulator a = planets[i];
                CelestialSimulator b = planets[j];

                Vector3 direction = b.transform.position - a.transform.position;
                float distanceSq = direction.sqrMagnitude;

                if (distanceSq < 1e-6f) continue;

                float forceMagnitude = G * a.mass * b.mass / distanceSq;
                Vector3 force = direction.normalized * forceMagnitude;

                a.AddForce(force);
                b.AddForce(-force);
            }
        }
    }
}
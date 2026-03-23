using System.Collections.Generic;
using UnityEngine;

public class GravitySimulator : MonoBehaviour
{
    public const float G = 39.478f;

    [Header("Simulación")]
    [Range(1f, 365)]
    public int timeScale = 1;
    private int stepsPerUpdate = 20;

    [SerializeField] private CelestialSimulator[] planets; // Los astros que participan en la simulación
    [SerializeField] private UIElements ui;

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
        if (ui != null) ui.UpdateTime(dt);

        for(int i = 0; i < planets.Length; i++)
        {
            planets[i].UpdateRotation(timeScale);
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
    public void ChangeSimulationSpeed(float newTimeScale)
    {
        timeScale = (int)newTimeScale;
        ui.UpdateSpeedTime(newTimeScale);
    }
}
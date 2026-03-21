using System.Collections.Generic;
using UnityEngine;

public class GravitySimulator : MonoBehaviour
{
    /// <summary>G en unidades astronómicas: 4π² UA³/(año²·M☉)</summary>
    public const float G = 39.478f;

    [Header("Simulación")]
    [Tooltip("Multiplicador de velocidad de la simulación (1 = tiempo real escalado).")]
    [Range(0.1f, 100f)]
    public float timeScale = 1f;

    [Tooltip("Número de pasos de integración por FixedUpdate (mayor = más precisión).")]
    [Range(1, 20)]
    public int stepsPerUpdate = 5;

    [SerializeField] private CelestialSimulator[] planets;

    void FixedUpdate()
    {
        // dt en años terrestres: Time.fixedDeltaTime está en segundos,
        // escalamos para que 1 segundo real = timeScale días simulados
        // 1 año = 365.25 días → dt en años = (fixedDeltaTime * timeScale) / 365.25
        float dt = (Time.fixedDeltaTime * timeScale) / 365.25f;
        float subDt = dt / stepsPerUpdate;

        for (int step = 0; step < stepsPerUpdate; step++)
        {
            ComputeGravity();

            foreach (var body in planets)
            {
                body.UpdateVelocityVerlet(subDt);
            }
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
                float distanceSq  = direction.sqrMagnitude;

                // Evitar singularidades si dos cuerpos se superponen
                if (distanceSq < 1e-6f) continue;

                float forceMagnitude = G * a.mass * b.mass / distanceSq;
                Vector3 force = direction.normalized * forceMagnitude;

                a.AddForce(force);   // a es atraído hacia b
                b.AddForce(-force);  // b es atraído hacia a (3ª ley de Newton)
            }
        }
    }


    


 
}

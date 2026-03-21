using UnityEngine;

/// <summary>
/// Controlador de cámara orbital con zoom y seguimiento de planetas.
/// - Clic derecho + arrastrar: rotar cámara.
/// - Scroll de ratón: zoom.
/// - Teclas 0-8: seguir un planeta concreto (0 = vista libre centrada en el Sol).
/// - Teclas +/-: ajustar timeScale de la simulación.
/// </summary>
public class CameraController : MonoBehaviour
{
    [Header("Referencias")]
    public GravitySimulator simulator;

    [Header("Rotación")]
    public float rotationSpeed = 5f;

    [Header("Zoom")]
    public float zoomSpeed   = 5f;
    public float minDistance = 1f;
    public float maxDistance = 80f;

    [Header("Seguimiento")]
    public Transform target; // cuerpo que sigue la cámara

    // Estado interno
    private float distance    = 15f;
    private float yaw         = 0f;
    private float pitch       = 30f;
    private Vector3 offset;

    void Start()
    {
        // Si no hay target explícito, buscar el Sol (el objeto más masivo)
        if (target == null)
        {
            var bodies = FindObjectsOfType<CelestialBody>();
            float maxMass = 0f;
            foreach (var b in bodies)
            {
                if (b.mass > maxMass) { maxMass = b.mass; target = b.transform; }
            }
        }
    }

    void LateUpdate()
    {
        HandleInput();
        FollowTarget();
    }

    private void HandleInput()
    {
        // ── Rotación con clic derecho ────────────────────────────────────────
        if (Input.GetMouseButton(1))
        {
            yaw   += Input.GetAxis("Mouse X") * rotationSpeed;
            pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;
            pitch  = Mathf.Clamp(pitch, 5f, 89f);
        }

        // ── Zoom con rueda del ratón ─────────────────────────────────────────
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distance -= scroll * zoomSpeed;
        distance  = Mathf.Clamp(distance, minDistance, maxDistance);

        // ── Ajustar velocidad de simulación con + / - ────────────────────────
        if (simulator != null)
        {
            if (Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.KeypadPlus))
                simulator.timeScale = Mathf.Min(simulator.timeScale * 2f, 100f);
            if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
                simulator.timeScale = Mathf.Max(simulator.timeScale * 0.5f, 0.1f);
        }
    }

    private void FollowTarget()
    {
        if (target == null) return;

        // Posición de la cámara en coordenadas esféricas alrededor del target
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 dir = rotation * Vector3.back;
        transform.position = target.position + dir * distance;
        transform.LookAt(target.position);
    }

    // ── GUI de información ───────────────────────────────────────────────────
    void OnGUI()
    {
        GUI.color = Color.white;
        GUILayout.BeginArea(new Rect(10, 10, 280, 120));
        GUILayout.Label("<b>Sistema Solar - Simulación</b>", new GUIStyle(GUI.skin.label) { richText = true, fontSize = 14 });
        if (simulator != null)
            GUILayout.Label($"Velocidad: {simulator.timeScale:F1}× (días/s)  [+/-]");
        GUILayout.Label("Clic derecho + arrastrar: rotar");
        GUILayout.Label("Rueda ratón: zoom");
        GUILayout.EndArea();
    }
}

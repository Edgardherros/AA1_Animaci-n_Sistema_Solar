using UnityEngine;

/// <summary>
/// Script de configuración de escena para la AA1.
/// Adjunta este componente a un GameObject vacío llamado "SceneSetup"
/// y pulsa el botón en el Inspector (o llama a BuildScene() desde código).
///
/// Crea automáticamente:
///   • Sol   (esfera amarilla, masa 1 M☉)
///   • Tierra (esfera azul, distancia 1 UA, velocidad 6.28 UA/año)
///   • Luz puntual en el Sol
///   • GravitySimulator
///   • Cámara con CameraController
/// </summary>
public class SceneSetup : MonoBehaviour
{
    [ContextMenu("Construir Escena")]
    public void BuildScene()
    {
        // ── Sol ──────────────────────────────────────────────────────────────
        GameObject sun = CreateSphere("Sol", Vector3.zero, 0.5f, new Color(1f, 0.9f, 0.2f));
        var sunBody = sun.AddComponent<CelestialBody>();
        sunBody.mass            = 1f;            // 1 M☉
        sunBody.initialVelocity = Vector3.zero;  // El Sol está fijo en el origen

        // Luz puntual en el Sol
        var light = sun.AddComponent<Light>();
        light.type      = LightType.Point;
        light.range     = 120f;
        light.intensity = 3f;
        light.color     = new Color(1f, 0.95f, 0.8f);

        // ── Tierra ───────────────────────────────────────────────────────────
        // Posición inicial: 1 UA en el eje X
        // Velocidad inicial: 6.28 UA/año en el eje Z (perpendicular a X en el plano XZ)
        GameObject earth = CreateSphere("Tierra", new Vector3(1f, 0f, 0f), 0.1f, new Color(0.2f, 0.5f, 1f));
        var earthBody = earth.AddComponent<CelestialBody>();
        earthBody.mass            = 3e-6f;              // 3×10⁻⁶ M☉
        earthBody.initialVelocity = new Vector3(0f, 0f, 6.28f); // UA/año

        // ── GravitySimulator ─────────────────────────────────────────────────
        GameObject simGO = new GameObject("GravitySimulator");
        var sim = simGO.AddComponent<GravitySimulator>();
        sim.timeScale      = 10f;   // 10 días simulados por segundo real
        sim.stepsPerUpdate = 5;
        
        // ── Cámara ───────────────────────────────────────────────────────────
        Camera cam = Camera.main;
        if (cam == null)
        {
            var camGO = new GameObject("Main Camera");
            camGO.tag = "MainCamera";
            cam = camGO.AddComponent<Camera>();
        }

        var camCtrl = cam.gameObject.GetComponent<CameraController>();
        if (camCtrl == null) camCtrl = cam.gameObject.AddComponent<CameraController>();
        camCtrl.simulator = sim;
        camCtrl.target    = sun.transform;

        cam.backgroundColor = Color.black;
        cam.clearFlags      = CameraClearFlags.SolidColor;

        Debug.Log("[SceneSetup] Escena construida: Sol + Tierra listos.");
    }

    //==================
    // SHADERS
    //==================

    // ── Ayudante ──────────────────────────────────────────────────────────────
    private GameObject CreateSphere(string name, Vector3 position, float radius, Color color)
    {
        var go       = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.name      = name;
        go.transform.position   = position;
        go.transform.localScale = Vector3.one * (radius * 2f);

        // Material con color emisivo para que se vea sin luz ambiental
        var mat = new Material(Shader.Find("Standard"));
        mat.color = color;
        mat.SetColor("_EmissionColor", color * 0.4f);
        mat.EnableKeyword("_EMISSION");
        go.GetComponent<Renderer>().material = mat;

        return go;
    }
}

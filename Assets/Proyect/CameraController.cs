using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Referencias")]
    public GravitySimulator simulator;

    [Header("Rotación")]
    public float rotationSpeed = 5f;

    [Header("Zoom")]
    public float zoomSpeed = 5f;
    public float minDistance = 1f;
    public float maxDistance = 80f;

    [Header("Seguimiento")]
    public Transform[] target; 
    private Transform actualTarget; // planeta que sigue la cámara
    private int targetIndex = 0;

    // Estado interno
    private float distance = 15f;
    private float yaw = 0f;
    private float pitch = 30f;

    void LateUpdate()
    {
        HandleInput();
        FollowTarget();
    }
    private void Start()
    {
        if (target.Length > 0)
            actualTarget = target[0];
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.D))
        {
            targetIndex = (targetIndex + 1) % target.Length;
            actualTarget = target[targetIndex];
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            targetIndex = (targetIndex - 1 + target.Length) % target.Length;
            actualTarget = target[targetIndex];
        }
    }
    private void HandleInput()
    {
        // Rotación con clic derecho
        if (Input.GetMouseButton(1))
        {
            yaw   += Input.GetAxis("Mouse X") * rotationSpeed;
            pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;
            pitch  = Mathf.Clamp(pitch, 5f, 89f);
        }

        // Zoom con rueda del ratón 
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distance -= scroll * zoomSpeed;
        distance  = Mathf.Clamp(distance, minDistance, maxDistance);

        // Ajustar velocidad de simulación con + / -
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

        // Posición de la cámara en 3D alrededor del target
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 dir = rotation * Vector3.back;
        transform.position = actualTarget.position + dir * distance;
        transform.LookAt(actualTarget.position);
    }

}

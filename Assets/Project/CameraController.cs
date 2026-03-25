using UnityEngine;
using TMPro;

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

    public TextMeshProUGUI currentCameraPlanet;
    private string cameraText = "Camera fixed on: ";

    void LateUpdate()
    {
        HandleInput();
        FollowTarget();
    }
    private void Start()
    {
        if (target.Length > 0)
            actualTarget = target[0];

        currentCameraPlanet.text = cameraText + actualTarget.name;
        
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            targetIndex = (targetIndex + 1) % target.Length;
            actualTarget = target[targetIndex];
            if(actualTarget.CompareTag("BigSatelit"))
            {
                minDistance = 0.75f;
            }
            else
            {
                minDistance = 0.4f;
            }
            currentCameraPlanet.text = cameraText + actualTarget.name;
        }
        if(Input.GetKeyDown(KeyCode.A)|| Input.GetKeyDown(KeyCode.LeftArrow))
        {
            targetIndex = (targetIndex - 1 + target.Length) % target.Length;
            actualTarget = target[targetIndex];
            if (actualTarget.CompareTag("BigSatelit"))
            {
                minDistance = 0.75f;
            }
            else
            {
                minDistance = 0.4f;
            }
            currentCameraPlanet.text = cameraText + actualTarget.name;
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

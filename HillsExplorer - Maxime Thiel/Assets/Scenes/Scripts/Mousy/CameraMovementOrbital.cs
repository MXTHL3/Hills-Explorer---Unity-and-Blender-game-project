using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovementOrbital : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField]
    private float rotationSpeed = 10f;

    [Header("Target Settings")]
    public Transform target;
    [SerializeField]
    private Vector3 offset = new Vector3(0, 7, -15);

    [Header("Game State")]
    public string gameState = "menu";

    [Header("Menu Camera Settings")]
    public Vector3 menuPosition = new Vector3(0, 10, -20);
    public Quaternion menuRotation = Quaternion.Euler(30, 0, 0);

    [Header("Rotation Limits")]
    [SerializeField]
    private float minVerticalAngle = -70f;
    [SerializeField]
    private float maxVerticalAngle = 70f;

    private float horizontalAngle = 0f;
    private float verticalAngle = 15f; 

    private UIManager uiManager;

    [Header("Terrain Settings")]
    [SerializeField]
    private Transform terrainsParent;
    private float raycastOffset = 1f;

    private void Start()
    {
        if (target != null)
        {           
            Vector3 direction = transform.position - target.position;
            horizontalAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            
            transform.position = target.position + offset;
            transform.LookAt(target);
        }
        ResetToMenuState();
        uiManager = FindObjectOfType<UIManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameState = "menu";
            ResetToMenuState();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            gameState = "inGame";
        }

        if (gameState == "menu")
        {
            uiManager.HandleMenuUI();
        }

        else if (gameState == "inGame")
        {
            uiManager.HandleInGameUI();
        }

        else if (gameState == "dead")
        {
            uiManager.HandleDeadScreen();
        }

        else if (gameState == "victory")
        {
            uiManager.HandleVictoryScreen();
        }
    }

    public void Rotate(InputAction.CallbackContext context)
    {
        if (gameState != "inGame" || target == null) return;

        Vector2 input = context.ReadValue<Vector2>();
        float horizontalInput = input.x * rotationSpeed * Time.deltaTime;
        float verticalInput = -input.y * rotationSpeed * Time.deltaTime;
        
        horizontalAngle += horizontalInput;
        verticalAngle += verticalInput;

        verticalAngle = Mathf.Clamp(verticalAngle, minVerticalAngle, maxVerticalAngle);

        Quaternion rotation = Quaternion.Euler(verticalAngle, horizontalAngle, 0);
        Vector3 newOffset = rotation * new Vector3(0, 0, -offset.magnitude);

        Vector3 targetPosition = GetPivotPosition() + newOffset;

        targetPosition = AdjustCameraHeightAboveTerrain(targetPosition);

        transform.position = targetPosition;
        transform.LookAt(GetPivotPosition());
    }

    private void LateUpdate()
    {
        if (target == null || gameState != "inGame") return;
       
        Quaternion rotation = Quaternion.Euler(verticalAngle, horizontalAngle, 0);
        Vector3 newOffset = rotation * new Vector3(0, 0, -offset.magnitude);

        Vector3 targetPosition = GetPivotPosition() + newOffset;
     
        targetPosition = AdjustCameraHeightAboveTerrain(targetPosition);

        transform.position = targetPosition;
        transform.LookAt(GetPivotPosition());
    }

    private Vector3 AdjustCameraHeightAboveTerrain(Vector3 cameraPosition)
    {
        if (terrainsParent == null) return cameraPosition;
       
        Ray ray = new Ray(cameraPosition + Vector3.up * 10f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 20f))
        {
            if (hit.transform.IsChildOf(terrainsParent))
            {             
                cameraPosition.y = Mathf.Max(cameraPosition.y, hit.point.y + raycastOffset);
            }
        }

        return cameraPosition;
    }

    private Vector3 GetPivotPosition()
    {      
        return target.position + Vector3.up * 5f;
    }

    private void ResetToMenuState()
    {
        transform.position = menuPosition;
        transform.rotation = menuRotation;
    }
}

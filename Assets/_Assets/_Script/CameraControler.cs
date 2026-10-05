using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class CameraControler : MonoBehaviour
{
    public BoardManager boardManager;

    [SerializeField] private float zoomSensitivity = 0.01f;
    [SerializeField] private float minOrthographicSize = 4f;
    [SerializeField] private float maxOrthographicSize = 30f;
    [SerializeField] private float minPerspectiveDistance = 5f;
    [SerializeField] private float maxPerspectiveDistance = 40f;

    private Camera controlledCamera;

    void Start()
    {
        controlledCamera = GetComponent<Camera>();

        if (boardManager == null)
            return;

        float boardCenter = (boardManager.boardSize - 1) * 0.5f;
        float cameraHeight = 10f;
        float cameraDistance = cameraHeight / Mathf.Tan(75f * Mathf.Deg2Rad);
        transform.position = new Vector3(boardCenter, cameraHeight, boardCenter - cameraDistance);
    }

    void Update()
    {
        HandleMouseDrag();
        HandleZoom();
    }

    private void HandleZoom()
    {
        if (Mouse.current == null)
            return;

        float scroll = Mouse.current.scroll.ReadValue().y;
        if (Mathf.Approximately(scroll, 0f))
            return;

        if (controlledCamera == null)
            controlledCamera = GetComponent<Camera>();

        if (controlledCamera.orthographic)
        {
            controlledCamera.orthographicSize = Mathf.Clamp(
                controlledCamera.orthographicSize - scroll * zoomSensitivity,
                minOrthographicSize,
                maxOrthographicSize);
            return;
        }

        Vector3 focusPoint = GetBoardCenter();
        Vector3 cameraOffset = transform.position - focusPoint;
        float distance = Mathf.Clamp(
            cameraOffset.magnitude - scroll * zoomSensitivity,
            minPerspectiveDistance,
            maxPerspectiveDistance);

        transform.position = focusPoint + cameraOffset.normalized * distance;
    }

    private void HandleMouseDrag()
    {
        if (Mouse.current == null || !Mouse.current.rightButton.isPressed)
            return;

        Vector2 dragDelta = Mouse.current.delta.ReadValue();
        if (dragDelta.sqrMagnitude == 0f)
            return;

        float worldUnitsPerPixel;
        if (controlledCamera.orthographic)
        {
            worldUnitsPerPixel = controlledCamera.orthographicSize * 2f / Screen.height;
        }
        else
        {
            float distance = Vector3.Distance(transform.position, GetBoardCenter());
            float viewHeight = 2f * distance * Mathf.Tan(controlledCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            worldUnitsPerPixel = viewHeight / Screen.height;
        }

        Vector3 dragMovement = GetCameraRightOnGround() * dragDelta.x
            + GetCameraForwardOnGround() * dragDelta.y;
        transform.position -= dragMovement * worldUnitsPerPixel;
    }

    private Vector3 GetCameraRightOnGround()
    {
        Vector3 direction = Vector3.ProjectOnPlane(transform.right, Vector3.up);
        return direction.sqrMagnitude > 0f ? direction.normalized : Vector3.right;
    }

    private Vector3 GetCameraForwardOnGround()
    {
        Vector3 direction = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
        return direction.sqrMagnitude > 0f ? direction.normalized : Vector3.forward;
    }

    private Vector3 GetBoardCenter()
    {
        if (boardManager == null)
            return Vector3.zero;

        float boardCenter = (boardManager.boardSize - 1) * 0.5f;
        return new Vector3(boardCenter, 0f, boardCenter);
    }
}

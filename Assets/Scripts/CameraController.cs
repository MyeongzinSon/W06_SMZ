using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour, PlayerInputActions.IPlayerActions
{
    [Header("General")]
    [SerializeField] float extraBoundarySize;
    [SerializeField] float smoothTime;
    [Header("Aim View")]
    [SerializeField] float generalAimRatio;
    [SerializeField] float gamepadAimRatio;

    PlayerInput inputManager;
    PlayerInputActions inputs;
    Camera camera;
    GameObject target;

    bool onFocus;
    float mapHalfSize;
    float cameraSizeX, cameraSizeY;
    Vector3 aimVector;
    Vector3 velocity = Vector3.zero;
    Vector3 resultPosition;

    bool IsKeyboardAndMouse => inputManager.currentControlScheme.Equals("Keyboard&Mouse");

    private void Awake()
    {
        inputs = new();
        inputs.Player.SetCallbacks(this);
        inputs.Enable();

        camera = GetComponent<Camera>();
        inputManager = FindObjectOfType<PlayerInput>();
    }
    void OnDisable()
    {
        inputs.Disable();
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(resultPosition, 1f);
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(aimVector + transform.position, 1f);
    }
    private void Start()
    {
        target = GameManager.instance.GetPlayer();
        mapHalfSize = GameManager.MapSize / 2;
        aimVector = new();
    }
    private void FixedUpdate()
    {
        if (IsKeyboardAndMouse)
        {
            var mousePosition = (Vector2)Camera.main.ScreenToWorldPoint((Vector2)Input.mousePosition);
            aimVector = (mousePosition - (Vector2)transform.position);
        }

        cameraSizeY = camera.orthographicSize;
        cameraSizeX = cameraSizeY / camera.pixelHeight * camera.pixelWidth;
        if (onFocus)
        {

        }
        else
        {
            var boundaryX = Mathf.Max(0, mapHalfSize - cameraSizeX + extraBoundarySize);
            var boundaryY = Mathf.Max(0, mapHalfSize - cameraSizeY + extraBoundarySize);

            var targetPosition = target.transform.position;
            resultPosition = Vector2.Lerp(targetPosition, aimVector + targetPosition, generalAimRatio / 2);
            resultPosition.z = transform.position.z;

            if (Mathf.Abs(resultPosition.x) > boundaryX) { resultPosition.x = boundaryX * Mathf.Sign(resultPosition.x); }
            if (Mathf.Abs(resultPosition.y) > boundaryY) { resultPosition.y = boundaryY * Mathf.Sign(resultPosition.y); }

            transform.position = Vector3.SmoothDamp(transform.position, resultPosition, ref velocity, smoothTime);
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
    }

    public void OnRoll(InputAction.CallbackContext context)
    {
    }

    public void OnAction(InputAction.CallbackContext context)
    {
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        var inputValue = context.ReadValue<Vector2>();
        aimVector.x = inputValue.x * cameraSizeX * gamepadAimRatio;
        aimVector.y = inputValue.y * cameraSizeY * gamepadAimRatio;
    }

    public void OnAimOnMouse(InputAction.CallbackContext context)
    {
        var inputValue = context.ReadValue<Vector2>();
    }
}

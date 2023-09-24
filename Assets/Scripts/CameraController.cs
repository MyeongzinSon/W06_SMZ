using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class CameraVibrationInfo
{
    Vector2 direction;
    float amplitude;
    float frequency;
    float duration;

    float currentDuration;
    public bool isEnd => currentDuration >= duration;

    public CameraVibrationInfo(Vector2 _direction, float _amplitude, float _frequency, float _duration)
    {
        direction = _direction;
        amplitude = _amplitude;
        frequency = _frequency;
        duration = _duration;

        currentDuration = 0;
    }
    public Vector2 OnUpdate()
    {
        currentDuration += Time.deltaTime;
        return direction.normalized
            * Mathf.Cos(frequency * currentDuration * 2 * Mathf.PI)
            * amplitude * Mathf.Exp(-5 * currentDuration / duration);
    }
}

public class CameraController : MonoBehaviour, PlayerInputActions.IPlayerActions
{
    [Header("General")]
    [SerializeField] float extraBoundarySize;
    [SerializeField] float smoothTime;
    [Header("Aim View")]
    [SerializeField] float generalAimRatio;
    [SerializeField] float gamepadAimRatio;
    [Header("Vibration")]
    [SerializeField] float standardAmplitude;
    [SerializeField] float standardFrequency;
    [SerializeField] float standardDuration;
    [Header("Focus")]
    [SerializeField] float focusDuration;
    [SerializeField] float focusAngleThreshold;
    [SerializeField] float focusCameraSize;
    [SerializeField] float focusTimeScale;

    PlayerInput inputManager;
    PlayerInputActions inputs;
    Camera camera;
    GameObject target;

    List<CameraVibrationInfo> cameraVibrationInfos;
    float mapHalfSize;
    float cameraSizeX, cameraSizeY;
    float defaultCameraSize;
    Vector3 aimVector;
    Vector3 velocity = Vector3.zero;
    Vector3 resultPosition;
    Vector2 vibrationResult;
    Vector3 focusStartPosition;
    Vector3 focusTarget;
    float focusTimer = -1;
    float focusAngle;



    bool IsKeyboardAndMouse => inputManager.currentControlScheme.Equals("Keyboard&Mouse");
    bool onFocus => focusTimer > 0;

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
    private void Start()
    {
        target = GameManager.instance.GetPlayer();
        mapHalfSize = GameManager.MapSize / 2;
        aimVector = new();
        cameraVibrationInfos = new();
    }
    void Update()
    {
        if (!vibrationResult.Equals(Vector2.zero))
        {
            transform.Translate(-vibrationResult);
        }

        vibrationResult = Vector2.zero;
        foreach (var v in cameraVibrationInfos.ToList())
        {
            vibrationResult += v.OnUpdate();
            if (v.isEnd)
            {
                cameraVibrationInfos.Remove(v);
            }
        }

        if (onFocus)
        {
            focusTimer -= Time.unscaledDeltaTime;
            SetFocus(focusTimer / focusDuration);
            if (!onFocus)
            {
                EndFocus();
            }
        }

        transform.Translate(vibrationResult);
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
        if (!onFocus)
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

    public void AddCameraVibration(Vector2 _direction, float _amplitude, float _frequency, float _duration)
    {
        cameraVibrationInfos.Add(new CameraVibrationInfo(_direction, _amplitude, _frequency, _duration));
    }
    public void AddCameraVibration(Vector2 _direction)
    {
        AddCameraVibration(_direction, standardAmplitude, standardFrequency, standardDuration);
    }
    public void StartFocus(Vector3 _focusTarget)
    {
        if (!onFocus)
        {
            focusTarget = _focusTarget;
            defaultCameraSize = camera.orthographicSize;
            focusStartPosition = transform.position;
            focusTimer = focusDuration;
            focusAngle = (Random.value > 0.5 ? 1 : -1) * focusAngleThreshold;
            //Debug.Log($"{defaultCameraSize} = {camera.orthographicSize}");
        }
    }
    void EndFocus()
    {
        if (onFocus)
        {
            SetFocus(0);
        }
    }
    void SetFocus(float value)
    {
        var t = 1 - Mathf.Pow(2 * value - 1, 4);
        Vector3 pos = Vector2.Lerp(focusStartPosition, focusTarget, t);
        pos.z = transform.position.z;
        transform.position = pos;
        transform.rotation = Quaternion.Lerp(Quaternion.identity, Quaternion.Euler(0, 0, focusAngle), t);
        Time.timeScale = Mathf.Lerp(1, focusTimeScale, t);
        camera.orthographicSize = Mathf.Lerp(defaultCameraSize, focusCameraSize, t);
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

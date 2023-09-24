using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerState { Idle, Roll, Action, Death }
public class PlayerController : MonoBehaviour, PlayerInputActions.IPlayerActions
{
    [SerializeField] float rollInputBuffer;
    [SerializeField] float actionInputBuffer;

    PlayerInput inputManager;
    PlayerInputActions inputs;
    PlayerMove playerMove;
    PlayerAction playerAction;
    PlayerHealth playerHealth;
    AimIndicator aimIndicator;

    Vector2 moveDirection;
    Vector2 aimDirection;
    bool desiredRoll;
    bool desiredAction;
    float rollInputBufferCounter;
    float actionInputBufferCounter;

    public Vector2 MoveDirection => moveDirection.normalized;
    public Vector2 AimDirection => aimDirection.normalized;
    public PlayerState CurrentState { get; private set; }
    public PlayerMove MoveComponent => playerMove;

    bool IsKeyboardAndMouse => inputManager.currentControlScheme.Equals("Keyboard&Mouse");

    public void OnMove(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }
    public void OnRoll(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            desiredRoll = true;
            rollInputBufferCounter = rollInputBuffer;
        }
    }
    public void OnAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            desiredAction = true;
            actionInputBufferCounter = actionInputBuffer;
        }

    }
    public void OnAim(InputAction.CallbackContext context)
    {
        var inputVector = context.ReadValue<Vector2>();
        if (!IsKeyboardAndMouse)
        {
            if (inputVector != Vector2.zero)
            {
                aimDirection = inputVector.normalized;
            }
        }
    }

    void Awake()
    {
        inputs = new();
        inputs.Player.SetCallbacks(this);
        inputs.Enable();

        inputManager = FindObjectOfType<PlayerInput>();
        playerMove = GetComponent<PlayerMove>();
        playerAction = GetComponent<PlayerAction>();
        playerHealth = GetComponent<PlayerHealth>();
        aimIndicator = GetComponentInChildren<AimIndicator>();
    }
    void OnDisable()
    {
        inputs.Disable();
    }
    void Start()
    {
        playerMove.CanMove = true;
        CurrentState = PlayerState.Idle;
    }

    void Update()
    {
        if (IsKeyboardAndMouse)
        {
            var mousePosition = (Vector2)Camera.main.ScreenToWorldPoint((Vector2)Input.mousePosition);
            aimDirection = (mousePosition - (Vector2)transform.position).normalized;
        }
        aimIndicator.SetDirection(aimDirection);

        if (desiredRoll && rollInputBufferCounter > 0)
        {
            rollInputBufferCounter -= Time.deltaTime;
            if (rollInputBufferCounter < 0)
            {
                desiredRoll = false;
            }
        }
        if (desiredAction && actionInputBufferCounter > 0)
        {
            actionInputBufferCounter -= Time.deltaTime;
            if (actionInputBufferCounter < 0)
            {
                desiredAction = false;
            }
        }
    }
    void FixedUpdate()
    {
        switch (CurrentState)
        {
            case PlayerState.Idle: FixedUpdateIdle(); break;
            case PlayerState.Roll: FixedUpdateRoll(); break;
            case PlayerState.Action: FixedUpdateAction(); break;
            case PlayerState.Death: FixedUpdateDeath(); break;
        }
        if (rollInputBuffer == 0) desiredRoll = false;
        if (actionInputBuffer == 0) desiredAction = false;
        if (!playerHealth.IsAlive && CurrentState != PlayerState.Death)
        {
            OnDeath();
        }
    }

    void FixedUpdateIdle()
    {
        playerMove.SetDireciton(moveDirection);

        TryRoll();
        TryAction();
    }
    void FixedUpdateRoll()
    {
        if (playerMove.IsRollEnded)
        {
            playerMove.EndRoll();
            CurrentState = PlayerState.Idle;
        }
        if (playerAction.CurrentAction.CanActionCancelRoll)
        {
            playerMove.EndRoll();
            TryAction();
        }
    }
    void FixedUpdateAction()
    {
        playerMove.SetDireciton(moveDirection);

        if (playerAction.IsActionEnded)
        {
            playerAction.EndAction();
            CurrentState = PlayerState.Idle;
        }
        if (playerAction.CurrentAction.CanRollCancelAction)
        {
            playerAction.EndAction();
            TryRoll();
        }
    }

    void FixedUpdateDeath()
    {
        playerMove.CanMove = false;
    }

    bool TryRoll()
    {
        if (desiredRoll)
        {
            playerMove.StartRoll(moveDirection);
            CurrentState = PlayerState.Roll;
            desiredRoll = false;
            return true;
        }
        return false;
    }
    bool TryAction()
    {
        if (desiredAction)
        {
            if (!playerAction.CanAction) { return false; }

            playerAction.StartAction();
            CurrentState = PlayerState.Action;
            desiredAction = false;
            return true;
        }
        return false;
    }
    void OnDeath()
    {
        if (CurrentState == PlayerState.Roll) playerMove.EndRoll();
        if (CurrentState == PlayerState.Action) playerAction.EndAction();

        CurrentState = PlayerState.Death;
        playerMove.OnDeath();
        playerAction.OnDeath();
        UIManager.Instance.EnableGameOverUI();
        GameManager.instance.GetBoss().GetComponent<Boss>().ShutdownAction();
        
    }
}

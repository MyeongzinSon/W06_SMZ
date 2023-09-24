using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] float speed;
    [SerializeField] float accelDuration;
    [SerializeField] float decelDuration;
    [Header("Roll")]
    [SerializeField] float rollDistance;
    [SerializeField] float rollDuration;
    [SerializeField] float rollDecelDuration;

    PlayerController player;
    PlayerAction action;
    Rigidbody2D playerRigidbody;

    Vector2 moveDirection;
    Vector2 rollDirection;
    float rollStartTime;
    Vector2 targetVelocity;
    Vector2 acceleration;
    Vector2 deceleration;
    float targetSpeed;

    public bool CanMove { get; set; }
    public bool IsRollEnded { get; private set; }
    public float SpeedModifier { get; private set; }
    public Vector2 CurrentVelocity => playerRigidbody.velocity;
    public bool IsMoving => playerRigidbody.velocity.magnitude > 0;
    public bool IsRolling => player.CurrentState == PlayerState.Roll;
    
    float FinalSpeed => speed * SpeedModifier;


    private void Awake()
    {
        player = GetComponent<PlayerController>();
        action = GetComponent<PlayerAction>();
        playerRigidbody = GetComponent<Rigidbody2D>();    
    }
    private void Start()
    {
        IsRollEnded = false;
        SpeedModifier = 1;
    }
    private void FixedUpdate()
    {
        if (!CanMove)
        {
            return;
        }

        switch (player.CurrentState)
        {
            case PlayerState.Idle: FixedUpdateIdle(); break;
            case PlayerState.Roll: FixedUpdateRoll(); break;
            case PlayerState.Action: FixedUpdateAction(); break;
            case PlayerState.Death: FixedUpdateDeath(); break;
        }

        SetVelocity();
    }
    void FixedUpdateIdle()
    {
        targetSpeed = FinalSpeed;
        targetVelocity = moveDirection * targetSpeed;
    }
    void FixedUpdateRoll()
    {
        if (Time.time > rollStartTime + rollDuration)
        {
            IsRollEnded = true;
        }
    }
    void FixedUpdateAction()
    {
        targetSpeed = FinalSpeed * action.CurrentAction.MoveSpeedMultiplier;
        targetVelocity = moveDirection * targetSpeed;
    }
    void FixedUpdateDeath()
    {
    }

    void SetVelocity()
    {
        acceleration = (targetVelocity - CurrentVelocity).normalized * (FinalSpeed * Time.fixedDeltaTime / accelDuration);
        deceleration = acceleration * (accelDuration / decelDuration);

        if (Vector2.Dot(targetVelocity, CurrentVelocity) < 0)
        {
            playerRigidbody.velocity += deceleration;
        }

        playerRigidbody.velocity += acceleration;
        //if (targetSpeed > 0 && CurrentVelocity.magnitude > targetSpeed)
        //{
        //    playerRigidbody.velocity = CurrentVelocity.normalized * targetSpeed;
        //}
        if (CurrentVelocity.magnitude < acceleration.magnitude / 2)
        {
            playerRigidbody.velocity = Vector2.zero;
        }
        //Debug.Log(CurrentVelocity);
    }

    public void SetDireciton(Vector2 direction)
    {
        moveDirection = direction;
        if (!direction.Equals(Vector2.zero)) { rollDirection = direction; }
    }
    public void SetSpeedModifier(float value)
    {
        SpeedModifier = value;
    }

    public void StartRoll(Vector2 _direction)
    {
        rollStartTime = Time.time;
        var direction = (_direction.Equals(Vector2.zero) ? rollDirection : _direction).normalized;
        playerRigidbody.velocity = direction * FinalSpeed;
        targetSpeed = SpeedModifier * rollDistance / rollDuration;
        targetVelocity = direction * targetSpeed;
    }
    public void ApplyStiff()
    {
        playerRigidbody.velocity = rollDirection.normalized * (speed * -2);
    }
    public void EndRoll()
    {
        if (!IsRolling) { return; }
        IsRollEnded = false;
    }
    public void OnDeath()
    {
        CanMove = false;
        playerRigidbody.velocity = Vector2.zero;
    }

}

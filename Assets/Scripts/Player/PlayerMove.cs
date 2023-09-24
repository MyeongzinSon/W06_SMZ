using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] float speed;
    [Header("Roll")]
    [SerializeField] float rollDistance;
    [SerializeField] float rollDuration;

    PlayerController player;
    PlayerAction action;
    Rigidbody2D playerRigidbody;

    Vector2 moveDirection;
    Vector2 rollDirection;
    float rollStartTime;

    public bool CanMove { get; set; }
    public bool IsRollEnded { get; private set; }
    public float SpeedModifier { get; private set; }
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
    }
    void FixedUpdateIdle()
    {
        playerRigidbody.velocity = moveDirection * FinalSpeed;
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
        var actionMuliplier = action.CurrentAction.MoveSpeedMultiplier;
        playerRigidbody.velocity = moveDirection * FinalSpeed * actionMuliplier;
    }
    void FixedUpdateDeath()
    {
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
        var rollSpeed = SpeedModifier * rollDistance / rollDuration;
        playerRigidbody.velocity = direction * (rollSpeed);
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

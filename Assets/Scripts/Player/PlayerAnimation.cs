using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] Color deathColor;
    [Header("Invunerable Color")]
    [SerializeField] Color invunerableColor;
    [SerializeField] int invunerableBlinkNum;
    [SerializeField] Ease invunerableBlinkEaseFunction;

    PlayerController player;
    PlayerMove playerMove;
    SpriteRenderer spriteRenderer;
    Animator animator;

    
    PlayerState previousState;
    Color defaultColor;
    bool isInvunerable;
    float invunerableDuration;
    float invunerableTimer;
    bool wasMoving;

    bool IsMoving => playerMove.IsMoving;


    void Awake()
    {
        player = GetComponent<PlayerController>();
        playerMove = GetComponent<PlayerMove>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        defaultColor = spriteRenderer.color;
        previousState = player.CurrentState;
        isInvunerable = false;
    }
    void Update()
    {
        switch (player.CurrentState)
        {
            case PlayerState.Idle: UpdateIdle(); break;
            case PlayerState.Roll: UpdateRoll(); break;
            case PlayerState.Action: UpdateAction(); break;
            case PlayerState.Death: UpdateDeath(); break;
        }
        if (isInvunerable)
        {
            UpdateInvunerableColor();
        }
        wasMoving = IsMoving;
    }


    void UpdateIdle()
    {
        if (previousState != PlayerState.Idle)
        {
            previousState = PlayerState.Idle;
            animator.Play(IsMoving ? "Run" : "Idle");
        }

        if (IsMoving && !wasMoving)
        {
            animator.Play("Run");
        }
        else if (!IsMoving && wasMoving)
        {
            animator.Play("Idle");
        }
    }
    void UpdateRoll()
    {
        if (previousState != PlayerState.Roll)
        {
            previousState = PlayerState.Roll;

            animator.Play("Dash_coke");
        }
    }
    void UpdateAction()
    {
        if (previousState != PlayerState.Action)
        {
            previousState = PlayerState.Action;

            animator.Play("attack_coke");
        }
    }
    void UpdateDeath()
    {
        if (previousState != PlayerState.Death)
        {
            previousState = PlayerState.Death;

            //temp code
            spriteRenderer.color = deathColor;
        }
    }

    public void OnStartInvunerable(float effectDuration)
    {
        isInvunerable = true;
        invunerableDuration = effectDuration;
        invunerableTimer = invunerableDuration;
    }
    public void OnEndInvunerable()
    {
        isInvunerable = false;
        spriteRenderer.color = defaultColor;
    }
    public void UpdateInvunerableColor()
    {
        invunerableTimer -= Time.deltaTime;

        var blinkDuration = invunerableDuration / invunerableBlinkNum;
        var interpolatedValue = ((invunerableDuration - invunerableTimer) % blinkDuration) / blinkDuration;
        //if (interpolatedValue > 0.5f)
        //{
        //    interpolatedValue = 1 - interpolatedValue;
        //}
        //var easedValue = DOVirtual.EasedValue(0, 1, interpolatedValue * 2, invunerableBlinkEaseFunction);
        //spriteRenderer.color = Color.Lerp(defaultColor, invunerableColor, easedValue);

        var result = Color.HSVToRGB(interpolatedValue, 1, 1);
        spriteRenderer.color = result;
    }
}

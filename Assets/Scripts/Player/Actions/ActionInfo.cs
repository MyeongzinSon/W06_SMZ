using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionInfo : MonoBehaviour
{
    [Header("General")]
    [SerializeField] protected float moveSpeedMultiplier;
    [SerializeField] protected bool canRollCancelAction;
    [SerializeField] protected bool canActionCancelRoll;

    protected PlayerController player;
    protected Vector2 aimDirection;

    public float MoveSpeedMultiplier => moveSpeedMultiplier;
    public bool CanRollCancelAction => canRollCancelAction;
    public bool CanActionCancelRoll => canActionCancelRoll;
    public bool IsActionEnded { get; protected set; }

    public abstract bool CanAction { get; }

    protected virtual void Awake()
    {
        player = transform.parent.GetComponent<PlayerController>();
    }
    protected virtual void Start()
    {

    }
    public void SetAimDirection(Vector2 direction)
    {
        aimDirection = direction;
    }
    public virtual void Initialize()
    {
    }
    public virtual void OnStartAction()
    {

    }
    public void OnUpdate(bool isActioning)
    {
        if (isActioning)
        {
            OnUpdateAction();
        }
        else
        {
            OnUpdateNotAction();
        }
    }
    public abstract void OnUpdateAction();
    public abstract void OnUpdateNotAction();
    public virtual void OnEndAction()
    {
        IsActionEnded = false;
    }
    public virtual void OnDeath()
    {
        OnEndAction();
    }
}

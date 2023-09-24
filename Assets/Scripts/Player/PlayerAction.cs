using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [SerializeField] ActionInfo currentAction;

    PlayerController player;

    Vector2 aimDirection;

    public ActionInfo CurrentAction => currentAction;
    public bool CanAction => currentAction.CanAction;
    public bool IsActionEnded => currentAction.IsActionEnded;

    bool IsActioning => player.CurrentState == PlayerState.Action;

    void Awake()
    {
        player = GetComponent<PlayerController>();
    }
    private void Start()
    {
        LoadAction();
    }
    private void Update()
    {
        SetActionDirection();
        currentAction.OnUpdate(IsActioning);
    }

    public void StartAction()
    {
        if (currentAction == null)
        {
            Debug.LogWarning("Player has no action!");
            return;
        }
        SetActionDirection();
        currentAction.OnStartAction();
    }
    public void EndAction()
    {
        if (!IsActioning) { return; }
        currentAction.OnEndAction();
    }
    public void OnDeath()
    {
        currentAction.OnDeath();
    }
    void SetActionDirection()
    {
        currentAction.SetAimDirection(player.AimDirection);
    }

    void LoadAction(ActionInfo currentAction = null)
    {
        if (currentAction == null) 
        { 
            currentAction = this.currentAction; 
        }
        else
        {
            this.currentAction = currentAction;
        }
        currentAction.Initialize();
    }

}

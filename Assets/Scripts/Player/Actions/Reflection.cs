using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflection : ActionInfo
{
    [SerializeField] protected float actionDuration;
    public float ActionDuration => actionDuration;
    float actionStartTime;
    public override bool CanAction => true;

    public override void Initialize()
    {
        base.Initialize();
        actionStartTime = Time.time;
    }
    public override void OnStartAction()
    {
        base.OnStartAction();
        actionStartTime = Time.time;
    }
    public override void OnUpdateAction()
    {
        if (Time.time > actionStartTime + actionDuration)
        {
            IsActionEnded = true;
        }

    }
    public override void OnUpdateNotAction()
    {
    }
    public override void OnEndAction()
    {
        base.OnEndAction();
    }
}

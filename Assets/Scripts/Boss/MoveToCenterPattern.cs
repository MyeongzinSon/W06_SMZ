using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveToCenterPattern : BossPattern
{
    [SerializeField] private float moveSpeed;
    protected override void ActionContext()
    {
        MoveToCneter();
    }

    private void MoveToCneter()
    {
        Vector3 center = Vector3.zero;
        main.transform.DOMove(center, moveSpeed);
    }
}

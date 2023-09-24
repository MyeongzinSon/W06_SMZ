using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashToPlayerPattern : BossPattern
{
    [SerializeField] private float dashForce;
    private Rigidbody2D rigid;

    protected override void Awake()
    {
        base.Awake();
        rigid = main.GetComponent<Rigidbody2D>();
    }
    protected override void PreProcessing()
    {
        base.PreProcessing();
        //애니메이션 실행시키기
    }
    protected override void ActionContext()
    {
        rigid.velocity = Vector2.zero;
        Dash();
    }
    protected override void PostProcessing()
    {
        base.PostProcessing();
    }

    private void Dash()
    {
        Vector3 playerDirection = (GameManager.instance.GetPlayer().transform.position - transform.position).normalized;
        rigid.velocity = playerDirection * dashForce;
        Debug.Log($"Pattern:Dash : {playerDirection} * {dashForce} = {rigid.velocity}");
    }
}

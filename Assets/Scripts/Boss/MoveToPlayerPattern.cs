using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPlayerPattern : BossPattern
{

    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveTime;
    [SerializeField] private float maxSpeed;
    [SerializeField] private bool isAct;
    private Rigidbody2D rigid;
    private float timer;

    private void Start()
    {
        rigid = main.GetComponent<Rigidbody2D>();
    }
    protected override void ActionContext()
    {
        isAct = true;
    }

    private void MoveToPlayer()
    {
        if (isAct)
        {
            if (timer <= moveTime)
            {
                timer += Time.deltaTime;
                Vector3 playerPos = GameManager.instance.GetPlayer().transform.position;
                Vector3 playerDirection = (playerPos-transform.position).normalized;
                if (rigid.velocity.magnitude <= maxSpeed)
                {
                    rigid.velocity += (Vector2)playerDirection * moveSpeed * Time.deltaTime;
                }
                else
                {
                    rigid.velocity = playerDirection * maxSpeed;
                }
                
                
            }
            else
            {
                rigid.velocity = Vector2.zero;
                timer = 0;
                isAct = false;
            }
        }
    }

    private void Update()
    {
        MoveToPlayer();
    }
    public override void ShutdownAction()
    {
        base.ShutdownAction();
        isAct = false;
        rigid.velocity = Vector2.zero;
    }
}

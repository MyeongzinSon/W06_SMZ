using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WalnutPattern : BossPattern
{
    [SerializeField] int walnutNum;
    [SerializeField] float walnutRadius;
    [SerializeField] float walnutAngularSpeed;
    [SerializeField] GameObject shadowPrefab;
    [SerializeField] GameObject walnutPrefab;

    Rigidbody2D rigid;

    float maxSpeed = 9;
    float moveSpeed = 9f;
    float moveTimer = -1;


    float currentAngle;
    float currentAngularSpeed;
    float angularAccel = 60;
    GameObject shadow;
    List<Walnut> spawnedWalnuts = new();

    protected override void Awake()
    {
        base.Awake();
        rigid = main.GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (spawnedWalnuts.Count > 0)
        {
            currentAngularSpeed -= angularAccel * Time.deltaTime;
            currentAngle += currentAngularSpeed * Time.deltaTime;
            ForeachWalnut((index, radian) =>
            {
                spawnedWalnuts[index].UpdateRotate(transform.position, radian + currentAngle * Mathf.Deg2Rad);
            });
        }
        
        if (moveTimer > 0)
        {
            moveTimer -= Time.deltaTime;
            if (moveTimer < postDelaySeconds - 1)
            {

                Vector3 playerPos = GameManager.instance.GetPlayer().transform.position;
                Vector3 playerDirection = (playerPos - transform.position).normalized;
                if (rigid.velocity.magnitude <= maxSpeed)
                {
                    rigid.velocity += (Vector2)playerDirection * moveSpeed * Time.deltaTime;
                }
                else
                {
                    rigid.velocity = playerDirection * maxSpeed;
                }
            }
        }
        else
        {
            rigid.velocity = Vector2.zero;
            moveTimer = -1;
        }
    }

    protected override void PreProcessing()
    {
        spawnedWalnuts.Clear();
        base.PreProcessing();
        currentAngularSpeed = walnutAngularSpeed;
        CastShadow();
    }
    protected override void ActionContext()
    {
        SpawnWalnuts();
        moveTimer = postDelaySeconds;
    }
    protected override void PostProcessing()
    {
        base.PostProcessing();
        foreach (var w in spawnedWalnuts.ToList())
        {
            spawnedWalnuts.Remove(w);
            w.EndRotate();
        }
    }
    void CastShadow()
    {
        shadow = Instantiate(shadowPrefab, transform.position, Quaternion.identity);
        shadow.transform.localScale = Vector3.one * walnutRadius * 2;
        Destroy(shadow, preDelaySeconds);
    }
    void SpawnWalnuts()
    {
        ForeachWalnut((index, radian) =>
        {
            var walnut = Instantiate(walnutPrefab, transform.position, Quaternion.identity).GetComponent<Walnut>();
            walnut.Initialize(transform.position, walnutRadius, radian);
            spawnedWalnuts.Add(walnut);
        });
    }

    void ForeachWalnut(System.Action<int, float> action)
    {
        for (int i = 0; i < walnutNum; i++)
        {
            var radian = (2 * Mathf.PI * i) / (walnutNum + 1);
            action?.Invoke(i, radian);
        }
    }
    public override void ShutdownAction()
    {
        base.ShutdownAction();
        foreach (var w in spawnedWalnuts.ToList())
        {
            spawnedWalnuts.Remove(w);
            w.EndRotate();
        }
        rigid.velocity = Vector2.zero;
        moveTimer = -1;
    }
}

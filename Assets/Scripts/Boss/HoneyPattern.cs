using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HoneyPattern : BossPattern
{
    [SerializeField] private GameObject honeyPrefab;
    [SerializeField] private GameObject honeyBarragePrefab;
    [SerializeField] private float barrageSpeed;

    protected override void ActionContext()
    {
        ShootHoney();
    }

    private void ShootHoney()
    {
        Vector3 playerPos = GameManager.instance.GetPlayer().transform.position;
        var honeyBarrage = Instantiate(honeyBarragePrefab, transform.position, Quaternion.identity).transform;
        honeyBarrage.gameObject.SetActive(true);
        honeyBarrage.position = transform.position;
        honeyBarrage.DOMove(playerPos, barrageSpeed).SetEase(Ease.OutQuint)
            .OnComplete(() =>
            {
                Instantiate(honeyPrefab, playerPos, Quaternion.identity);
                honeyBarrage.gameObject.SetActive(false);
            });
    }
}

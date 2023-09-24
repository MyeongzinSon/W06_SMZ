using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DropPumpkinPattern : BossPattern
{
    [SerializeField] bool withOpposite = false;
    [SerializeField] private GameObject pumpkinPrefab;
    [SerializeField] private GameObject pumpkinShadowPrefab;
    [SerializeField] private float pumpkinOffsetY;
    [SerializeField] private float dropDuration;
    [SerializeField] private float delayAttackTime;

    protected override void ActionContext()
    {
        StartCoroutine(DropPumpkin(false));
        if (withOpposite)
        {
            StartCoroutine(DropPumpkin(true));
        }
    }

    private IEnumerator DropPumpkin(bool onOpposite = false)
    {
        Vector3 playerPos = GameManager.instance.GetPlayer().transform.position * (onOpposite ? -1 : 1);
        GameObject pumpkinShadow = Instantiate(pumpkinShadowPrefab, playerPos, Quaternion.identity);
        yield return new WaitForSeconds(delayAttackTime);

        Vector3 pumpkinDefaultPos = playerPos;
        pumpkinDefaultPos.y += pumpkinOffsetY;
        GameObject pumpkin = Instantiate(pumpkinPrefab, pumpkinDefaultPos, Quaternion.identity);
        Pumpkin pumpkinComponent = pumpkin.GetComponent<Pumpkin>();

        pumpkin.transform.DOMoveY(playerPos.y, dropDuration).SetEase(Ease.OutExpo)
            .OnComplete(() =>
            {
                pumpkinComponent.EnableCollider();
                Destroy(pumpkinShadow);
            });
    }
}

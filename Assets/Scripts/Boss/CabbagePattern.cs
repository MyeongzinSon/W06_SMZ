using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabbagePattern : BossPattern
{
    [SerializeField] float spawnDistance;
    [SerializeField] GameObject cabbagePrefab;

    Vector3 playerDirection;
    Cabbage cabbage;

    protected override void PreProcessing()
    {
        base.PreProcessing();
        SpawnCabbage();
    }
    protected override void ActionContext()
    {
        cabbage.Shoot(playerDirection);
    }
    void SpawnCabbage()
    {
        playerDirection = (GameManager.instance.GetPlayer().transform.position - transform.position).normalized;
        cabbage = Instantiate(cabbagePrefab, transform.position + playerDirection * spawnDistance, Quaternion.identity).GetComponent<Cabbage>();
    }

    public override void ShutdownAction()
    {
        base.ShutdownAction();
        if (cabbage)
        {
            Destroy(cabbage.gameObject);
        }
    }
}

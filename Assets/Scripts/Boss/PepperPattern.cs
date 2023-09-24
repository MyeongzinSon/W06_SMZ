using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PepperPattern : BossPattern
{
    [SerializeField] private GameObject pepperPrefab;
    [SerializeField] private int rotate = 0;

    private Vector3[] cornerPositions = new Vector3[4];

    private void Start()
    {
        float halfMapSize = (GameManager.MapSize) / 2;
        cornerPositions[0] = new Vector3(halfMapSize, halfMapSize, 0);
        cornerPositions[1] = new Vector3(-halfMapSize, halfMapSize, 0);
        cornerPositions[2] = new Vector3(-halfMapSize, -halfMapSize, 0);
        cornerPositions[3] = new Vector3(halfMapSize, -halfMapSize, 0);
    }

    protected override void ActionContext()
    {
        SpawnPepper();
    }

    private void SpawnPepper()
    {
        Vector3 playerPos = GameManager.instance.GetPlayer().transform.position;
        var sortedByMagnitude = cornerPositions.OrderBy(v => (playerPos - v).magnitude);
        var minPoint = sortedByMagnitude.First();

        var index = System.Array.FindIndex(cornerPositions, v => v.Equals(minPoint));
        var result = (index + rotate) % 4;
        Vector3 pepperSpawnPoint = cornerPositions[result];
        Instantiate(pepperPrefab, pepperSpawnPoint, Quaternion.identity);
    }
    
}

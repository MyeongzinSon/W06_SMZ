using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoPattern : BossPattern
{
    [SerializeField] private GameObject potatoPrefab;
    [SerializeField] private float potatoCount;
    [SerializeField] private float potatoSpeed;
    [SerializeField] private float delayTime;
    [SerializeField] private int forcedDirection = -1;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }
    protected override void ActionContext()
    {
        StartCoroutine(nameof(SpawnPotato));
    }

    private IEnumerator SpawnPotato()
    {
        float mapSize = GameManager.MapSize;
        int rand = forcedDirection == -1 ? Random.Range(0, 4) : forcedDirection;
        Vector2 potatoDirection = Vector2.zero;
        Vector3 potatoStartPos = Vector3.zero;
        Vector3 space = Vector3.zero;

        switch (rand)
        {
            //위로 전진
            case 0:
                potatoDirection = Vector2.up;
                potatoStartPos = new Vector3(-mapSize / 2, -mapSize / 2, 0);
                space = new Vector3(mapSize / potatoCount, 0, 0);
                break;

            // 아래로 전진
            case 1:
                potatoDirection = Vector2.down;
                potatoStartPos = new Vector3(-mapSize / 2, mapSize / 2, 0);
                space = new Vector3(mapSize / potatoCount, 0, 0);
                break;

            // 왼쪽으로 전진
            case 2:
                potatoDirection = Vector2.left;
                potatoStartPos = new Vector3(mapSize / 2, -mapSize / 2, 0);
                space = new Vector3(0, mapSize / potatoCount, 0);
                break;

            //오른쪽으로 전진
            case 3:
                potatoDirection = Vector2.right;
                potatoStartPos = new Vector3(-mapSize / 2, -mapSize / 2, 0);
                space = new Vector3(0, mapSize / potatoCount, 0);
                break;


        }

        Vector3 potatoPos = potatoStartPos;
        List<Potato> potatoList = new();
        for (int i = 0; i < potatoCount + 1; i++)
        {
            GameObject potato = Instantiate(potatoPrefab, potatoPos, Quaternion.identity);
            Potato potatoComponent = potato.GetComponent<Potato>();
            potatoComponent.InitPotato(potatoDirection, potatoSpeed);
            potatoPos += space;
            potatoList.Add(potatoComponent);
        }

        yield return new WaitForSeconds(delayTime);

        for(int i=0;i< potatoCount + 1; i++)
        {
            potatoList[i].CanMove();
        }
    }
   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBossPattern1 : BossPattern
{
    [SerializeField] private GameObject barragePrefab;
    [SerializeField] private float radius;
    [SerializeField] private int barrageCount;
    [SerializeField] private float startAngle;
    [SerializeField] private float endAngle;
    private float angle;
    protected override void ActionContext()
    {
        Debug.Log("nowPattern");
        ShootBarrage();
    }

    
    private void ShootBarrage()
    {
        float barrageSpace = (endAngle - startAngle) / (barrageCount-1);
        angle = 90 - startAngle;
        
        for (int i = 0; i < barrageCount; i++)
        {
            float radian = angle * Mathf.Deg2Rad;
            Vector3 barragePos = new Vector3(Mathf.Cos(radian) * radius, Mathf.Sin(radian) * radius, 0);
            Instantiate(barragePrefab, barragePos, Quaternion.Euler(0,0,angle));
            angle -= barrageSpace;
           
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPhase4 : Boss
{
    [SerializeField] private int nowBossHP;
    [SerializeField] private string deadAnimationName;
    public override void OnDamage(int damage = 1)
    {
        StartNextPhase();
    }

    private void StartNextPhase()
    {
        if (isDeal)
        {
            ResetStatusOnNextPhase();
            UIManager.Instance.SetBossHP(nowBossHP);
            ShutdownAction();
            anim.Play(deadAnimationName);
            GameManager.instance.StartEnding();
            Debug.Log("Game Clear");
        }
    }
}

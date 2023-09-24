using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEAttackPattern : BossPattern
{
    [SerializeField] private List<AOEAttack> attack = new();
    [SerializeField] private float attackInterval;

    protected override void ActionContext()
    {
        StartCoroutine(nameof(AOEAttack));
    }

    private IEnumerator AOEAttack()
    {
        for(int i = 0; i < attack.Count; i++)
        {
            attack[i].StartAttack();
            yield return new WaitForSeconds(attackInterval);
        }
    }

}

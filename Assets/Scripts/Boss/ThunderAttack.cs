using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderAttack : MonoBehaviour
{
    [SerializeField] private float cautionTime;
    [SerializeField] private SpriteRenderer cautionEffect;
    private Collider2D attackCollider;
    private SpriteRenderer attackEffect;
    [SerializeField] private float attackTime;


    private void Start()
    {
        attackCollider = GetComponent<Collider2D>();
        attackEffect = GetComponent<SpriteRenderer>();
    }

    public void StartAttack()
    {
        StartCoroutine(nameof(AttackPlay));
    }
    private IEnumerator AttackPlay()
    {
        cautionEffect.enabled = true;
        yield return new WaitForSeconds(cautionTime);
        cautionEffect.enabled = false;

        attackEffect.enabled = true;
        attackCollider.enabled = true;
        yield return new WaitForSeconds(attackTime);
        attackEffect.enabled = false;
        attackCollider.enabled = false;
    }
}

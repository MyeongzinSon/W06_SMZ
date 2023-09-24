using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AOEAttack : MonoBehaviour
{
    [SerializeField] private GameObject cautionEffect;
    [SerializeField] private float cautionTime;
    private WaitForSeconds waitCautionTime;
    [SerializeField] private float blinkTime;
    [SerializeField] private SpriteRenderer effect;
    [SerializeField] private Collider2D attackCollider;
    [SerializeField] private float attackTime;
    private WaitForSeconds waitAttackTime;
    private Sequence blinkSequence;

    private void Start()
    {

        waitCautionTime = new WaitForSeconds(cautionTime);
        waitAttackTime = new WaitForSeconds(attackTime);

        blinkSequence = DOTween.Sequence();
        blinkSequence.SetAutoKill(false);
        blinkSequence.Append(DOTween.ToAlpha(() => cautionEffect.GetComponent<SpriteRenderer>().color,
                color => cautionEffect.GetComponent<SpriteRenderer>().color = color, 0f, blinkTime / 2));
        blinkSequence.Append(DOTween.ToAlpha(() => cautionEffect.GetComponent<SpriteRenderer>().color,
            color => cautionEffect.GetComponent<SpriteRenderer>().color = color, 0.5f, blinkTime / 2));
        blinkSequence.SetLoops(-1);
        blinkSequence.Pause();


    }

    public void StartAttack()
    {
        StartCoroutine(AttackPlay());
    }
    private IEnumerator AttackPlay()
    {
        
        cautionEffect.SetActive(true);
        blinkSequence.Play();
        yield return waitCautionTime;
        cautionEffect.SetActive(false);

        attackCollider.enabled = true;
        effect.enabled = true;
        yield return waitAttackTime;

        attackCollider.enabled = false;
        effect.enabled = false;
        blinkSequence.Rewind();
    }

}

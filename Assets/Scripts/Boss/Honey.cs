using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Honey : MonoBehaviour,IDamageable
{
    [SerializeField] private float duration;
    private float nowduration;
    [SerializeField] private float damage;
    private SpriteRenderer honeySprtie;
    [SerializeField] private float defaultSpeed;
    [SerializeField] private float downSpeed;
    [SerializeField] private float honeyTime;

    private void Start()
    {
        honeySprtie = GetComponent<SpriteRenderer>();
        nowduration = duration;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SetDownSpeed(collision.GetComponent<PlayerMove>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SetDefalutSpeed(collision.GetComponent<PlayerMove>());
        }
    }

    private void SetDownSpeed(PlayerMove player)
    {
        player.SetSpeedModifier(downSpeed);
    }

    private void SetDefalutSpeed(PlayerMove player)
    {
        player.SetSpeedModifier(defaultSpeed);
    }
    private void FadeOutHoney()
    {
        if (nowduration <= 0)
        {
            DestroyObject();
        }
        nowduration -= Time.deltaTime;
        float nowAlpha = Mathf.Lerp(0, 1, nowduration / duration);
        Color alpha = Color.white;
        alpha.a = nowAlpha;
        honeySprtie.color = alpha;
    }

    private void DestroyObject()
    {

        Destroy(gameObject);
    }
    private void Update()
    {
        FadeOutHoney();
    }

    public void OnDamage(int damage = 1)
    {
        nowduration -= this.damage;
    }
}

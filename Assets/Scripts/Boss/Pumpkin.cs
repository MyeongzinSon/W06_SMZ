using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pumpkin : MonoBehaviour,IDamageable
{
    [SerializeField] private float maxHP;
    private float nowHP;
    [SerializeField] private Sprite crackPumpkin;
    [SerializeField] private Sprite pumpkin;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask playerLayer;
    private Collider2D collider;

    void Start()
    {
        collider = GetComponent<Collider2D>();
        collider.enabled = false;
        nowHP = maxHP;
    }
    public void OnDamage(int damage = 1)
    {
        if (collider.enabled)
        {
            nowHP -= 1;
            if (nowHP == 1)
            {
                GetComponent<SpriteRenderer>().sprite = crackPumpkin;
                return;
            }

            if (nowHP == 0)
            {
                gameObject.SetActive(false);
                DOTween.Complete(transform);
                Destroy(gameObject);
            }

            
        }
    }

    public void EnableCollider()
    {
        collider.enabled = true;
        var result = new List<Collider2D>();
        var filter = new ContactFilter2D();
        filter.SetLayerMask(playerLayer);

        collider.OverlapCollider(filter, result);
        result.ForEach(c =>
        {
            if (c.gameObject.TryGetComponent<IDamageable>(out var player))
            {
                player.OnDamage();
            }
        });
    }

}

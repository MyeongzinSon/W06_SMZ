using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potato : MonoBehaviour, IDamageable
{
    [SerializeField] private LayerMask potatoLayer;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float raycastDistance;
    [SerializeField] private float moveSpeed;
    private bool isMove = false;
    private Vector2 direction = Vector2.right;


    public void InitPotato(Vector2 dir, float speed)
    {
        direction = dir;
        moveSpeed = speed;
    }

    public void OnDamage(int damage = 1)
    {
        Destroy(gameObject);
    }


    public void CanMove() => isMove = true;
    private void MoveForward()
    {
        if (isMove)
        {
            transform.Translate(direction * moveSpeed * Time.deltaTime);

            if (RaycastCollision())
            {
                // 뛰는 애니메이션 실행
                //Debug.Log($"Potato(ID={transform.GetInstanceID()}) get jumped!");
                return;
            }
        }
        
    }
    
    private bool RaycastCollision()
    {
        Vector2 playerPos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(playerPos, direction, raycastDistance, potatoLayer);

        if (hit.collider != null)
        {
            return true;
        }
        return false;
    }

    private void Update()
    {
        MoveForward();
        if (Mathf.Max(Mathf.Abs(transform.position.x), Mathf.Abs(transform.position.y)) > GameManager.MapSize / 2)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isMove && collision.gameObject.TryGetComponent<PlayerHealth>(out var player))
        {
            player.OnDamage();
        }
    }
}

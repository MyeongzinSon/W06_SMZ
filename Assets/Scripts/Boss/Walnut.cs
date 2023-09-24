using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walnut : MonoBehaviour
{
    float traceRadius;
    float angularSpeed;
    
    public void Initialize(Vector2 origin, float radius, float angleRadian)
    {
        traceRadius = radius;
        SetLocalPosition(origin, angleRadian);
    }
    public void UpdateRotate(Vector2 origin, float angleRadian)
    {
        SetLocalPosition(origin, angleRadian);
    }
    public void EndRotate()
    {
        Destroy(gameObject);
    }

    void SetLocalPosition(Vector2 origin, float radian)
    {
        var direction = Vector2.right * Mathf.Sin(radian) + Vector2.up * Mathf.Cos(radian);
        transform.position = origin + direction * traceRadius;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerHealth playerHealth))
        {
            playerHealth.OnDamage();
        }
    }
}

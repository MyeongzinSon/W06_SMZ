using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrage : MonoBehaviour
{
    [SerializeField] private float activeTime;
    [SerializeField] private float moveSpeed;
    private void Start()
    {
        Invoke(nameof(DestroyObject), activeTime);
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }

    private void ShootBarrage()
    {
        transform.Translate(Vector3.right * moveSpeed);
    }

    private void Update()
    {
        ShootBarrage();
    }
}

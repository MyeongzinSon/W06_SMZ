using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimIndicator : MonoBehaviour
{
    [SerializeField] private float nearDistance = 1;
    [SerializeField] private float farDinstance = 1.5f;

    LineRenderer lineRenderer;
    Vector3 aimDirection;


    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        var position = transform.parent.position;
        var direction = aimDirection;
        var startPos = position + direction * nearDistance;
        var endPos = position + direction * farDinstance;

        var positions = new Vector3[] { startPos, endPos };
        lineRenderer.SetPositions(positions);
    }

    public void SetDirection(Vector2 direction)
    {
        aimDirection = direction;
    }
}

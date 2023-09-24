using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CokeGeyser : MonoBehaviour
{
    [Header("Coke Can Object")]
    [SerializeField] GameObject cokeObject;

    new BoxCollider2D boxCollider;
    new PolygonCollider2D polygonCollider;
    LayerMask targetLayer;
    List<IDamageable> damaged;
    Vector3 currentDirection;
    Vector2 defaultColliderSize;
    float maxAngularSpeed;
    float width;

    public Quaternion CurrentRotation => Quaternion.FromToRotation(Vector2.right, currentDirection);
    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        polygonCollider = GetComponent<PolygonCollider2D>();
        defaultColliderSize = boxCollider.size;
        boxCollider.enabled = false;
        damaged = new();
    }
    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
        cokeObject.SetActive(value);
    }
    public void StartGeyser(LayerMask targetLayer, Vector2 startPoint, Vector2 aimDirection, float angularSpeed, float width)
    {
        this.width = width;
        this.targetLayer = targetLayer;
        maxAngularSpeed = angularSpeed;
        currentDirection = aimDirection;
        transform.localScale = Vector3.zero;
        damaged.Clear();
        SetActive(true);
    }
    public void EndGeyser()
    {
        SetActive(false);
    }
    public void UpdatePoint(Vector2 startPoint, Vector2 targetVector)
    {
        var direction = targetVector.normalized;
        var length = targetVector.magnitude;

        currentDirection = Vector3.RotateTowards(currentDirection, direction, maxAngularSpeed * Mathf.Deg2Rad * Time.deltaTime, 0);


        var filter = new ContactFilter2D();
        filter.useTriggers = false;
        filter.SetLayerMask(targetLayer);

        var raycastResult = new List<RaycastHit2D>();
        if (Physics2D.Raycast(startPoint, currentDirection, filter, raycastResult, length) > 0)
        {
            length = raycastResult.Select(r => Vector2.Distance(startPoint, r.point)).Min();
        }

        filter.useTriggers = true;
        var overlapResult = new List<Collider2D>();
        polygonCollider.OverlapCollider(filter, overlapResult);

        SetGeyser(startPoint, length);

        overlapResult.
            Select(c => c.GetComponent<IDamageable>()).
            Where(d => d != null && !damaged.Contains(d)).
            ForEach((d) =>
        {
            d.OnDamage();
            damaged.Add(d);
        });

        SetGeyser(startPoint, length);
    }

    void SetGeyser(Vector2 startPoint, float length)
    {
        transform.position = startPoint + (Vector2)currentDirection * length / 2;
        transform.rotation = CurrentRotation;
        transform.localScale = Vector2.right * length / defaultColliderSize.x + Vector2.up * width / defaultColliderSize.y;
        cokeObject.transform.rotation = CurrentRotation;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float extraBoundarySize;
    [SerializeField] float smoothTime;

    Camera camera;
    GameObject target;

    float mapHalfSize;
    Vector3 velocity = Vector3.zero;


    private void Awake()
    {
        camera = GetComponent<Camera>();
    }
    private void Start()
    {
        target = GameManager.instance.GetPlayer();
        mapHalfSize = GameManager.MapSize / 2;
    }
    private void LateUpdate()
    {
        var cameraSizeY = camera.orthographicSize;
        var cameraSizeX = cameraSizeY / camera.pixelHeight * camera.pixelWidth;

        var boundaryX = Mathf.Max(0, mapHalfSize - cameraSizeX + extraBoundarySize);
        var boundaryY = Mathf.Max(0, mapHalfSize - cameraSizeY + extraBoundarySize);

        var targetPosition = target.transform.position;
        targetPosition.z = transform.position.z;

        if (Mathf.Abs(targetPosition.x) > boundaryX) { targetPosition.x = boundaryX * Mathf.Sign(targetPosition.x); }
        if (Mathf.Abs(targetPosition.y) > boundaryY) { targetPosition.y = boundaryY * Mathf.Sign(targetPosition.y); }

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}

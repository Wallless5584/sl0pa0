using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LightningTarget : MonoBehaviour
{
    Camera camera;
    [SerializeField] LineRenderer lineRenderer;
    float lastPointX;

    void Start()
    {
        camera = Camera.main;
        //LineRenderer lineRenderer = transform.parent.GetComponentInChildren<LineRenderer>();
        //int lineLastPointIndex = lineRenderer.positionCount - 1;
        lastPointX = lineRenderer.GetPosition(2).x;
    }

    void Update()
    {
        TargetUpdatePos();
    }

    void TargetUpdatePos()
    {
        transform.position = camera.transform.position;
        transform.localPosition = new Vector3(x: Mathf.Clamp(transform.localPosition.x, 0, lastPointX), y: 0, z: 0);
    }
}

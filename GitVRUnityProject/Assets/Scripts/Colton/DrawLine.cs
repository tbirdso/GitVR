using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{

    private LineRenderer lineRenderer;
    private float counter;
    private float dist;

    public Transform origin;
    public Transform destination;

    public float lineDrawSpeed = 6f;
    public float lineWidth = 0.45f;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, origin.position);
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        dist = Vector3.Distance(origin.position, destination.position);
    }

    // Update is called once per frame
    void Update()
    {
        if(counter < dist)
        {
            counter += 0.1f / lineDrawSpeed;

            float x = Mathf.Lerp(0, dist, counter);

            Vector3 pointA = origin.position;
            Vector3 pointB = destination.position;

            Vector3 pointAlongLine = x * Vector3.Normalize(pointB - pointA) + pointA;

            lineRenderer.SetPosition(1, pointAlongLine);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererIsAss : MonoBehaviour
{
    public List<GameObject> lines;
    public GameObject lineRendererPrefab;
    private LineRenderer lineRenderer;
    public float lineWidth = 0.45f;

    // Start is called before the first frame update
    void Start()
    {
        DrawLineBetweenTwoObjects(new Vector3(0,0,0), new Vector3(0, 10, 0));
        DrawLineBetweenTwoObjects(new Vector3(0, 0, 0), new Vector3(5, 10, 5));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AddLine()
    {
        lines.Add(lineRendererPrefab);
    }

    private void DrawLineBetweenTwoObjects(Vector3 origin, Vector3 pointB)
    {
        print("AAA");
        GameObject tempLineRenderer = Instantiate(lineRendererPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        lines.Add(tempLineRenderer);
        lineRenderer = tempLineRenderer.GetComponent<LineRenderer>();
        
        lineRenderer.SetPosition(0, origin);
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.SetPosition(1, pointB);
        
    }
}

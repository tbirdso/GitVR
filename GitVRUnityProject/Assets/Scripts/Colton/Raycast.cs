using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

// C# example.

public class Raycast : MonoBehaviour
{
    private ToolTipScript toolTipScript;
    public float selectedSize;
    public float normalSize;
    public List<GameObject> hitters;
    public GameObject cursor;

    void Update()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");

            if (!hitters.Contains(hit.transform.gameObject))
            {
                hitters.Add(hit.transform.gameObject);
            }
            
            toolTipScript = hit.transform.gameObject.GetComponent<ToolTipScript>();
            toolTipScript.setToolTipDisplayBool(true);
            toolTipScript.transform.localScale = new Vector3(selectedSize, selectedSize, selectedSize);

            cursor.transform.position = hit.transform.position;
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
            toolTipScript.setToolTipDisplayBool(false);
            toolTipScript.transform.localScale = new Vector3(normalSize, normalSize, normalSize);
        }
    }
}
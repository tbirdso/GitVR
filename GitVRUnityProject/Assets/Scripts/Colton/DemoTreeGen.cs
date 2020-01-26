using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoTreeGen : MonoBehaviour
{

    public GameObject node;
    public int number_to_generate;
    public int distance;
    public Vector3 origin = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {

        for(int i = 0; i < number_to_generate; i++)
        {
            Instantiate(node, new Vector3(0, i * distance, 0), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shovel : Tool
{
    public GameObject dirt;
    
    // Use this for initialization
    void Start()
    {
        toolID = 2;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void  UseTool() 
    {
     //   base.UseTool();
        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

        Debug.Log("Shovel");
        if (Physics.Raycast(ray, out hit, rayMaxDist))
        {
            if (hit.transform.CompareTag("Ground"))
            {
                Instantiate(dirt, hit.point, transform.rotation);
            }
        }
    }
}

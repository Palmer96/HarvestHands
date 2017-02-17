using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shovel : Item
{
    public GameObject dirt;
    
    // Use this for initialization
    void Start()
    {
        itemID = 4;
        itemCap = 1;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void PrimaryUse() 
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

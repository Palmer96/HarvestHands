using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Tool
{

    public GameObject wood;

    // Use this for initialization
    void Start()
    {
        toolID = 1;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void UseTool()
    {
        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

        Debug.Log("Axe");
        if (Physics.Raycast(ray, out hit, rayMaxDist))
        {
            if (hit.transform.CompareTag("Tree"))
            {
                hit.transform.GetComponent<Tree>().Harvest();
                Instantiate(wood, hit.transform.position, transform.rotation);
            }
        }

    }

    }

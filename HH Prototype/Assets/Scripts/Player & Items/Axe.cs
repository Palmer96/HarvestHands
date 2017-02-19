using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Item
{

    public int level = 1;
    public GameObject wood;

    // Use this for initialization
    void Start()
    {
        itemID = 3;
        itemCap = 1;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void PrimaryUse()
    {
        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

        Debug.Log("Axe");
        if (Physics.Raycast(ray, out hit, rayMaxDist))
        {
            if (hit.transform.CompareTag("Tree"))
            {
                hit.transform.GetComponent<Tree>().Harvest();
                if (level > 1)
                    hit.transform.GetComponent<Tree>().Harvest();
                if (level > 2)
                    hit.transform.GetComponent<Tree>().Harvest();
                //    Instantiate(wood, hit.point, transform.rotation);
            }
        }

    }

    }

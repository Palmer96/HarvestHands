﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickaxe : Item
{

    // Use this for initialization
    void Start()
    {
        itemID = 7;
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
            if (hit.transform.CompareTag("Rock"))
            {
                hit.transform.GetComponent<Rock>().Harvest();

                //    Instantiate(wood, hit.point, transform.rotation);
            }
        }

    }
}
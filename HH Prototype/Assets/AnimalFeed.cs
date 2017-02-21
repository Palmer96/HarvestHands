﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalFeed : Item
{
    public int hungerIncrease = 10;
	// Use this for initialization
	void Start ()
    {
        if (!dontUpdate)
        {
            if (singleMesh == null)
                singleMesh = GetComponent<MeshFilter>().mesh;
            if (multiMesh == null)
                multiMesh = GetComponent<MeshFilter>().mesh;
            if (singleMaterial == null)
                singleMaterial = GetComponent<MeshRenderer>().material;
            if (multiMaterial == null)
                multiMaterial = GetComponent<MeshRenderer>().material;
        }
        if (itemCap == 0)
        {
            itemCap = 20;
        }
        UpdateMesh();
    }

    public override void PrimaryUse()
    {
        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        if (Physics.Raycast(ray, out hit, rayMaxDist))
        {
            if (hit.transform.CompareTag("Livestock"))
            {
                quantity--;
                hit.transform.GetComponent<Livestock>().Feed(hungerIncrease);
            }
        }
        if (quantity <= 0)
        {
            PlayerInventory.instance.DestroyItem();
        }
    }

    public override void SecondaryUse()
    {
        
    }


}
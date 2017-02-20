using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalFeed : Item
{
    public int happinessIncrease = 10;
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
        if (Physics.Raycast(ray, out hit, rayMaxDist))
        {
            if (hit.transform.CompareTag("Livestock"))
            {
                hit.transform.GetComponent<Livestock>().IncreaseHappiness(happinessIncrease);
            }
        }
    }

    public override void SecondaryUse()
    {
        
    }


}

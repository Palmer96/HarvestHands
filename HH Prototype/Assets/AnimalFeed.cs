using System.Collections;
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

    public override void PrimaryUse(ClickType click)
    {
        if (click == Item.ClickType.Hold)
            PrimaryUse();
    }

    public override void PrimaryUse()
    {
        Debug.Log("Inside animal feed use"); 
        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        if (Physics.Raycast(ray, out hit, rayMaxDist))
        {
            Debug.Log("Inside hit");
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
}

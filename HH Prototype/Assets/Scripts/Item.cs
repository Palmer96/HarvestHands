using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{


    public int itemID;
    public string itemName;
    public int quantity;
    public int value;
    public bool sellable;
    public int itemCap;

    public float rayMaxDist = 5;

    public RaycastHit hit;
    public Ray ray;

    public Mesh singleMesh;
    public Mesh multiMesh;

    public Material singleMaterial;
    public Material multiMaterial;

    public bool dontUpdate;

    // Use this for initialization
    void Start()
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
    }

    public virtual void PrimaryUse()
    {

        Debug.Log("Use Item");
        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

        Debug.Log("Axe");
        if (Physics.Raycast(ray, out hit, rayMaxDist))
        {
            if (hit.transform.CompareTag("Building"))
            {
                hit.transform.GetComponent<Building>().AddResource(gameObject);
            }
        }

    }

    public virtual void PrimaryUse(GameObject gameObj)
    {
        Debug.Log("Use Item");
        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

        Debug.Log("Axe");
        if (Physics.Raycast(ray, out hit, rayMaxDist))
        {
            if (hit.transform.CompareTag("Building"))
            {
                hit.transform.GetComponent<Building>().AddResource(gameObject);
            }
        }
    }

    public virtual void SecondaryUse()
    {
        Debug.Log("Use Item");
    }

    public virtual void SecondaryUse(GameObject gameObj)
    {
        Debug.Log("Use Item");
    }



    public virtual void UpdateMesh()
    {
        if (!dontUpdate)
        {
            if (quantity > 1)
            {
                GetComponent<MeshFilter>().mesh = multiMesh;
                GetComponent<MeshRenderer>().material = multiMaterial;
                if (GetComponent<MeshCollider>() != null)
                    GetComponent<MeshCollider>().sharedMesh = multiMesh;
            }
            else
            {
                GetComponent<MeshFilter>().mesh = singleMesh;
                GetComponent<MeshRenderer>().material = singleMaterial;
                if (GetComponent<MeshCollider>() != null)
                    GetComponent<MeshCollider>().sharedMesh = singleMesh;
            }
        }
    }

    public virtual void IncreaseQuantity()
    {
        quantity++;

        UpdateMesh();

        GetComponent<Collider>().enabled = false;
    }

    public virtual void IncreaseQuantity(int amount)
    {
        quantity += amount;

        UpdateMesh();

        GetComponent<Collider>().enabled = false;
    }

    public virtual void DecreaseQuantity()
    {
        quantity--;
        if (quantity == 1)
        {
            UpdateMesh();

            GetComponent<Collider>().enabled = false;
        }

        if (quantity < 0)
        {
            Destroy(gameObject);
        }
    }

    public virtual void DecreaseQuantity(int amount)
    {
        quantity -= amount;
        if (quantity == 1)
        {
            UpdateMesh();

            GetComponent<Collider>().enabled = false;
        }

        if (quantity < 0)
        {
            Destroy(gameObject);
        }
    }
}

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

    Mesh ownMesh;
    Material ownMaterial;
    MeshCollider ownMeshCollider;

    public Mesh singleMesh;
    public Mesh multiMesh;

    public Material singleMaterial;
    public Material multiMaterial;

    public bool dontUpdate;

    // Use this for initialization
    void Start()
    {
        if (GetComponent<MeshFilter>().mesh != null)
            ownMesh = GetComponent<MeshFilter>().mesh;
        if (ownMesh == null)
            ownMesh = transform.GetChild(0).GetComponent<MeshFilter>().mesh;

        if (GetComponent<MeshRenderer>().material != null)
            ownMaterial = GetComponent<MeshRenderer>().material;
        if (ownMaterial == null)
            ownMaterial = transform.GetChild(0).GetComponent<MeshRenderer>().material;

        if (GetComponent<MeshCollider>() != null)
            ownMeshCollider = GetComponent<MeshCollider>();
        if (ownMeshCollider == null)
            ownMeshCollider = transform.GetChild(0).GetComponent<MeshCollider>();

        if (!dontUpdate)
        {
            if (singleMesh == null)
                singleMesh = ownMesh;
            if (multiMesh == null)
                multiMesh = ownMesh;
            if (singleMaterial == null)
                singleMaterial = ownMaterial;
            if (multiMaterial == null)
                multiMaterial = ownMaterial;
        }

        if (itemCap == 0)
        {
            itemCap = 20;
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
                ownMesh = multiMesh;
                ownMaterial = multiMaterial;
                if (ownMeshCollider != null)
                    ownMeshCollider.sharedMesh = multiMesh;
            }
            else
            {
                ownMesh = singleMesh;
                ownMaterial = singleMaterial;
                if (ownMeshCollider != null)
                    ownMeshCollider.sharedMesh = singleMesh;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ClickType
    {
        Single,
        Hold
    };

    public int itemID;
    public string itemName;
    public int quantity;
    public int value;
    public bool sellable;
    public int itemCap;

    public float rayMaxDist = 5;

    public RaycastHit hit;
    public Ray ray;

    public bool used = false;
    public float useTimer = 0.25f;
    public float useRate = 0.25f;

    //  protected MeshFilter ownMesh;
    //  protected Material ownMaterial;
    //  protected MeshCollider ownMeshCollider;

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

            UpdateMesh();
        }
        if (itemCap == 0)
        {
            itemCap = 20;
        }
    }

    public virtual void PrimaryUse()
    {
        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

        if (Physics.Raycast(ray, out hit, rayMaxDist))
        {
            if (hit.transform.CompareTag("Building"))
            {
                hit.transform.GetComponent<Building>().AddResource(gameObject);
            }
        }
    }

    public virtual void PrimaryUse(ClickType click)
    {
        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

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
        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

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

    }

    public virtual void SecondaryUse(ClickType click)
    {

    }

    public virtual void SecondaryUse(GameObject gameObj)
    {

    }




    public virtual void UpdateMesh()
    {
        bool colliderEnabled = false;

        if (GetComponent<MeshCollider>() != null)
        {
            colliderEnabled = GetComponent<MeshCollider>().enabled;
            GetComponent<MeshCollider>().enabled = true;
        }
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
            if (GetComponent<MeshCollider>() != null)
                GetComponent<MeshCollider>().enabled = colliderEnabled;
        }
    }

    public virtual void IncreaseQuantity()
    {
        quantity++;
        UpdateMesh();
    }

    public virtual void IncreaseQuantity(int amount)
    {
        quantity += amount;
        UpdateMesh();
    }

    public virtual void DecreaseQuantity()
    {
        quantity--;
        if (quantity == 1)
        {
            UpdateMesh();
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
        }

        if (quantity < 0)
        {
            Destroy(gameObject);
        }
    }
}

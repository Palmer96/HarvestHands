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


    public float rayMaxDist = 5;

    public RaycastHit hit;
    public Ray ray;

    public Mesh singleMesh;
    public Mesh multiMesh;

    public Material singleMaterial;
    public Material multiMaterial;

    // Use this for initialization
    void Start()
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

    // Update is called once per frame
    void Update()
    {

    }


    public virtual void UseItem()
    {
        Debug.Log("Use Item");
    }

    public virtual void UseItem(GameObject gameObj)
    {
        Debug.Log("Use Item");
    }

    public virtual void UpdateMesh()
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

    public virtual void IncreaseQuantity()
    {
        quantity++;
        GetComponent<MeshFilter>().mesh = multiMesh;
        GetComponent<MeshRenderer>().material = multiMaterial;

        if (GetComponent<MeshCollider>() != null)
            GetComponent<MeshCollider>().sharedMesh = multiMesh;

        GetComponent<Collider>().enabled = false;
    }

    public virtual void IncreaseQuantity(int amount)
    {
        quantity += amount;
        GetComponent<MeshFilter>().mesh = multiMesh;
        GetComponent<MeshRenderer>().material = multiMaterial;

        if (GetComponent<MeshCollider>() != null)
            GetComponent<MeshCollider>().sharedMesh = multiMesh;

        GetComponent<Collider>().enabled = false;
    }

    public virtual void DecreaseQuantity()
    {
        quantity--;
        if (quantity == 1)
        {
            GetComponent<MeshFilter>().mesh = singleMesh;
            GetComponent<MeshRenderer>().material = singleMaterial;

            if (GetComponent<MeshCollider>() != null)
                GetComponent<MeshCollider>().sharedMesh = singleMesh;

            GetComponent<Collider>().enabled = false;
        }
    }

    public virtual void DecreaseQuantity(int amount)
    {
        quantity -= amount;
        if (quantity == 1)
        {
            GetComponent<MeshFilter>().mesh = singleMesh;
            GetComponent<MeshRenderer>().material = singleMaterial;

            if (GetComponent<MeshCollider>() != null)
                GetComponent<MeshCollider>().sharedMesh = singleMesh;

            GetComponent<Collider>().enabled = false;
        }
    }
}

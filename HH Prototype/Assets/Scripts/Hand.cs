using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : Item
{

    public GameObject heldItem;
    // Use this for initialization
    void Start()
    {
        itemID = 1;
        itemCap = 1;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void PrimaryUse(GameObject item)
    {
        if (heldItem != null)
        {
            Drop();
        }
        else
        {
            ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
            if (Physics.Raycast(ray, out hit, rayMaxDist))
            {
                if (hit.transform.CompareTag("Item"))
                {
                    PickUp(item);
                }
            }
        }
    }

    public override void SecondaryUse()
    {
        Throw();
    }

    public void PickUp(GameObject item)
    {
        if (!heldItem)
        {
            heldItem = item;
            heldItem.transform.SetParent(transform.parent);
            heldItem.transform.localPosition = new Vector3(1, 0, 2);
            heldItem.GetComponent<Rigidbody>().isKinematic = true;

            heldItem.layer = 2;

            if (heldItem.GetComponent<UnityEngine.AI.NavMeshAgent>() != null)
            {
                //Destroy(heldItem.GetComponent<NavMeshAgent>());
                // heldItem.GetComponent<NavMeshAgent>().updatePosition = false;
                // heldItem.GetComponent<NavMeshAgent>().updateRotation = false;
                heldItem.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
            }
        }
    }

    public void Throw()
    {
        if (heldItem)
        {
            heldItem.GetComponent<Rigidbody>().isKinematic = false;
            heldItem.GetComponent<Rigidbody>().AddForce(transform.parent.forward * 500, ForceMode.Force);
            heldItem.GetComponent<Rigidbody>().transform.parent = null;
            heldItem.layer = 0;

            heldItem = null;
        }
    }

    public void Drop()
    {
        if (heldItem != null)
        {
            heldItem.GetComponent<Rigidbody>().isKinematic = false;
            heldItem.GetComponent<Rigidbody>().transform.parent = null;
            heldItem.layer = 0;

            heldItem = null;
        }
    }
}

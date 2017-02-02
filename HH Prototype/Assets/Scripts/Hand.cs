using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : Tool
{

    GameObject heldItem;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void UseTool(GameObject item)
    {
        PickUp(item);
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
}

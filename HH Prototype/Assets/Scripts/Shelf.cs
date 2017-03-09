using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelf : MonoBehaviour
{
    public GameObject storedObject;

    [Header("Optional")]
    public Transform displayPosition;
    public Vector3 rotationSpeed = new Vector3();
    public ParticleSystem particleSystem;
    public Collider pickupCollider;

    // Use this for initialization
    void Start ()
    {
        if (particleSystem != null)
            particleSystem.Stop();
        if (storedObject == null)
        {
            pickupCollider.enabled = false;
        }
        else
        {

        }
    }

    void Update()
    {
        if (storedObject != null)
        {
            Vector3 temp = storedObject.transform.rotation.eulerAngles;
            temp += rotationSpeed;
            //storedObject.transform.rotation = Quaternion.Euler(temp);
        }
    }
	
    public bool StoreItem(GameObject thingToStore)
    {
        if (storedObject == null)
        {
            if (particleSystem != null)
                particleSystem.Play();
        //    PlayerInventory.instance.RemoveItem();
            PlayerInventory.instance.heldObjects[PlayerInventory.instance.selectedItemNum] = null;
            storedObject = thingToStore;
            if (displayPosition != null)
                storedObject.transform.SetParent(displayPosition);
            else
                storedObject.transform.SetParent(transform);

            storedObject.transform.localPosition = new Vector3(0, 0, 0);
            storedObject.transform.rotation = Quaternion.identity;
           // if (storedObject.GetComponent<Rigidbody>() != null)
           //     storedObject.GetComponent<Rigidbody>().isKinematic = true;
            pickupCollider.enabled = true;
            return true;
        }

        Item storedItem = storedObject.GetComponent<Item>();
        Item itemToStore = thingToStore.GetComponent<Item>();
        if (storedItem == null || itemToStore == null)
            return false;

        if (itemToStore.itemID == storedItem.itemID)
        {
            //If quantity enough to fit
            if ((itemToStore.quantity + storedItem.quantity) <= storedItem.itemCap)
            {
                storedItem.IncreaseQuantity(itemToStore.quantity);
                itemToStore.DecreaseQuantity(itemToStore.quantity);
                //storedItem.quantity += itemToStore.quantity;
                //itemToStore.quantity -= itemToStore.quantity;

                PlayerInventory.instance.RemoveItem(thingToStore);
                Debug.Log("Shelf is destroying " + thingToStore.name);
                //Destroy(thingToStore);
                return true;
            }
            //Add as much quantity to fit
            else
            {
                int neededForCap = storedItem.itemCap - storedItem.quantity;
                storedItem.quantity += neededForCap;
                itemToStore.quantity -= neededForCap;
                return true;
            }
        }        
        return false;
    }

    public GameObject TakeOutItem()
    {
        if (storedObject == null)
            return null;
        if (particleSystem != null)
            particleSystem.Stop();

        GameObject theItem = storedObject;
        if (storedObject.GetComponent<Rigidbody>() != null)
            storedObject.GetComponent<Rigidbody>().isKinematic = false;
        storedObject = null;
        pickupCollider.enabled = false;

        return theItem;
    }
}

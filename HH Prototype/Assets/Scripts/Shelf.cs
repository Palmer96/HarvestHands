using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelf : MonoBehaviour
{
    public GameObject storedObject;
    public bool highlighted = false;

    [Header("Optional")]
    public Transform displayPosition;
    public Vector3 rotationSpeed = new Vector3();
    public ParticleSystem particleSystem;
    public Collider pickupCollider;
    public TextMesh quantityText;

    private Vector3 storedScale = new Vector3(1, 1, 1);

    // Use this for initialization
    void Start ()
    {
        if (particleSystem != null)
            particleSystem.Stop();
        if (storedObject == null)
        {
            if (pickupCollider != null)
                pickupCollider.enabled = false;
            if (quantityText != null)
                quantityText.text = "";
        }
        UpdateQuantityText();
    }

    void Update()
    {
        if (storedObject != null)
        {
            Vector3 temp = storedObject.transform.rotation.eulerAngles;
            temp += rotationSpeed;
            //storedObject.transform.rotation = Quaternion.Euler(temp);
            storedScale = storedObject.transform.lossyScale;
        }

        if (highlighted)
        {
            highlighted = false;
            GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            GetComponent<MeshRenderer>().enabled = false;
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

            storedScale = thingToStore.transform.lossyScale;
            storedObject.transform.SetParent(null);
            if (displayPosition != null)
            {
                storedObject.transform.rotation = displayPosition.transform.rotation;
                storedObject.transform.position = displayPosition.transform.position;
            }
            else
            {
                storedObject.transform.rotation = transform.rotation;
                storedObject.transform.position = transform.position;
            }
            //if (displayPosition != null)
            //    storedObject.transform.SetParent(displayPosition);
            //else
            //    storedObject.transform.SetParent(transform);

            //storedObject.transform.localPosition = new Vector3(0, 0, 0);
            storedObject.transform.rotation = Quaternion.identity;
           // if (storedObject.GetComponent<Rigidbody>() != null)
           //     storedObject.GetComponent<Rigidbody>().isKinematic = true;
           if (pickupCollider != null)
                pickupCollider.enabled = true;
            UpdateQuantityText();
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
                UpdateQuantityText();
                return true;
            }
            //Add as much quantity to fit
            else
            {
                int neededForCap = storedItem.itemCap - storedItem.quantity;
                storedItem.quantity += neededForCap;
                itemToStore.quantity -= neededForCap;
                UpdateQuantityText();
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
        if (pickupCollider != null)
            pickupCollider.enabled = false;

        UpdateQuantityText();
        //theItem.transform.SetParent(null);
        theItem.transform.localScale = storedScale;
        storedScale = new Vector3(1, 1, 1);
        return theItem;
    }

    public void UpdateQuantityText()
    {
        if (storedObject != null)
        {
            if (quantityText != null)
            {
                Item storedItem = storedObject.GetComponent<Item>();
                if (storedItem != null)
                {
                    if (storedItem.quantity > 1)
                    {
                        quantityText.text = storedItem.quantity.ToString();
                    }
                    else
                    {
                        quantityText.text = "";
                    }
                }
            }
        }
        else
        {
            if (quantityText != null)
            {
                quantityText.text = "";
            }                
        }
    }

}

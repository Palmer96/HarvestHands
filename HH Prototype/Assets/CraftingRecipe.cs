using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingRecipe : MonoBehaviour
{
    public string recipeName = "";
    public GameObject result;
    public List<CraftingManager.ResourceRequirement> requiredItems = new List<CraftingManager.ResourceRequirement>();

	// Use this for initialization
	void Start ()
    {
        if (result == null)
            Debug.Log(name + " has no result for it's CraftingRecipe");
	}

    public void Craft()
    {
        foreach (CraftingManager.ResourceRequirement requirement in requiredItems)
        {
            bool hasItem = false;
            foreach (GameObject loadedObject in PlayerInventory.instance.heldObjects)
            {
                Item loadedItem = loadedObject.GetComponent<Item>();
                if (loadedItem == null)
                    return;

                //If have the item
                if (loadedItem.itemName == requirement.resourceName)
                {
                    hasItem = true;
                    //If dont have enough, return
                    if (loadedItem.quantity < requirement.numRequired)
                    {
                        return;
                    }
                }
            }
            //If have 0 of the resources
            if (hasItem == false)
                return;
        }
        //remove requirements
        foreach (CraftingManager.ResourceRequirement requirement in requiredItems)
        {
            foreach (GameObject loadedObject in PlayerInventory.instance.heldObjects)
            {
                Item loadedItem = loadedObject.GetComponent<Item>();
                if (loadedItem.itemName == requirement.resourceName)
                {
                    loadedItem.quantity -= requirement.numRequired;
                }
            }
        }
        GameObject newObject = (GameObject)Instantiate(result, PlayerInventory.instance.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
    }
}
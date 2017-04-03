using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingRecipe : MonoBehaviour
{
    public enum RecipeType
    {
        UNASSIGNED = 0,
        TOOL = 1,
        CONSTRUCT = 2,
        RESOURCE = 3,
    }
    public RecipeType recipeType = RecipeType.UNASSIGNED;
    public string recipeName = "";
    public GameObject result;
    public int quantity = 1;
    public string itemDescription = "";
    public List<CraftingManager.ResourceRequirement> requiredItems = new List<CraftingManager.ResourceRequirement>();

    // Use this for initialization
    void Start()
    {
        if (result == null)
            Debug.Log(name + " has no result for it's CraftingRecipe");
    }

    public GameObject Craft()
    {
        foreach (CraftingManager.ResourceRequirement requirement in requiredItems)
        {
            bool hasItem = false;
            int haveAmount = 0;
            foreach (GameObject loadedObject in PlayerInventory.instance.heldObjects)
            {
                if (loadedObject == null)
                    continue;
                Item loadedItem = loadedObject.GetComponent<Item>();
                if (loadedItem == null)
                    return null;

                //If have the item
                if (loadedItem.itemName == requirement.resourceName)
                {
                    hasItem = true;
                    //If dont have enough, continue looking
                    if (loadedItem.quantity < requirement.numRequired)
                    {
                        haveAmount += loadedItem.quantity;
                    }
                    if (haveAmount >= loadedItem.quantity)
                    {
                        hasItem = true;
                        continue;
                    }
                }
            }
            //If have 0 of the resources
            if (hasItem == false)
                return null;
        }
        //remove requirements
        foreach (CraftingManager.ResourceRequirement requirement in requiredItems)
        {
            int neededAmount = requirement.numRequired;
            foreach (GameObject loadedObject in PlayerInventory.instance.heldObjects)
            {
                if (loadedObject == null)
                    continue;
                Item loadedItem = loadedObject.GetComponent<Item>();
                if (loadedItem.itemName == requirement.resourceName)
                {
                    if (loadedItem.quantity <= neededAmount)
                    {
                        neededAmount -= loadedItem.quantity;
                        Destroy(loadedItem.gameObject);
                        PlayerInventory.instance.DestroyItem(loadedItem.gameObject);
                        loadedItem = null;
                    }
                    //if have quantity > neededamount, reduce quanitty
                    else
                    {
                        loadedItem.quantity -= neededAmount;
                        neededAmount = 0;
                    }


                    //loadedItem.quantity -= requirement.numRequired;
                    //if (loadedItem.quantity == 0)
                    //{
                    //    DestroyImmediate(loadedItem.gameObject);
                    //    loadedItem = null;
                    //}
                }
                if (neededAmount == 0)
                {
                    break;
                }
            }
        }
        GameObject newObject = (GameObject)Instantiate(result, PlayerInventory.instance.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        newObject.GetComponent<Item>().quantity = quantity;
        return newObject;
    }

    public bool HaveResources()
    {
        foreach (CraftingManager.ResourceRequirement requirement in requiredItems)
        {
            bool hasItem = false;
            int haveAmount = 0;
            foreach (GameObject loadedObject in PlayerInventory.instance.heldObjects)
            {
                if (loadedObject == null)
                    continue;
                Item loadedItem = loadedObject.GetComponent<Item>();
                if (loadedItem == null)
                    continue;

                //If have the item
                if (loadedItem.itemName == requirement.resourceName)
                {
                    //Add quantity to have amount
                    haveAmount += loadedItem.quantity;
                }
                //Check if have enough
                if (haveAmount >= requirement.numRequired)
                {
                    hasItem = true;
                    break;
                }
            }
            //If dont have enough of a requirement
            if (hasItem == false)
                return false;
        }
        return true;
    }


}


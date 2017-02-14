﻿using System.Collections;
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
    public string itemDescription = "";
    public List<CraftingManager.ResourceRequirement> requiredItems = new List<CraftingManager.ResourceRequirement>();

	// Use this for initialization
	void Start ()
    {
        if (result == null)
            Debug.Log(name + " has no result for it's CraftingRecipe");
	}

    public GameObject Craft()
    {
        foreach (CraftingManager.ResourceRequirement requirement in requiredItems)
        {
            bool hasItem = false;
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
                    //If dont have enough, return
                    if (loadedItem.quantity < requirement.numRequired)
                    {
                        return null;
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
            foreach (GameObject loadedObject in PlayerInventory.instance.heldObjects)
            {
                if (loadedObject == null)
                    continue;
                Item loadedItem = loadedObject.GetComponent<Item>();
                if (loadedItem.itemName == requirement.resourceName)
                {
                    loadedItem.quantity -= requirement.numRequired;
                    if (loadedItem.quantity == 0)
                    {
                        Destroy(loadedItem.gameObject);
                        loadedItem = null;
                    }
                }
            }
        }
        GameObject newObject = (GameObject)Instantiate(result, PlayerInventory.instance.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        return newObject;
    }

    //public static string SortByName(CraftingRecipe recipe1, CraftingRecipe recipe2)
    //{
    //    return recipe1.recipeName.CompareTo(recipe2.recipeName);
    //}
    public class RecipeSort : IComparer<CraftingRecipe>
    {
        int IComparer<CraftingRecipe>.Compare(CraftingRecipe _objA, CraftingRecipe _objB)
        {
            string t1 = _objA.GetComponent<CraftingRecipe>().recipeName;
            string t2 = _objB.GetComponent<CraftingRecipe>().recipeName;
            return t1.CompareTo(t2);
        }
    }
}

//class RecipeSort : IComparer<CraftingRecipe>
//{    int IComparer<CraftingRecipe>.Compare(CraftingRecipe _objA, CraftingRecipe _objB)
//    {
//        string t1 = _objA.GetComponent<CraftingRecipe>().recipeName;
//        string t2 = _objB.GetComponent<CraftingRecipe>().recipeName;
//        return t1.CompareTo(t2);
//    }
//}
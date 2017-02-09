using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingBench : MonoBehaviour
{
    public int currentlySelceted = 0;
    public TextMesh recipeNameText;
    public TextMesh recipeRequirementsText;
    public List<Item> loadedResources = new List<Item>();
    public Transform spawnPoint;
       

	// Use this for initialization
	void Start ()
    {
        Invoke("DisplayRecipeChoice", .1f);
        //Debug.Log("Recipes = " + CraftingManager.instance.knownRecipes.Count);
        //NextRecipeChoice();

    }	

    void OnTriggerEnter(Collider col)
    {
        Item item = col.GetComponent<Item>();
        if (item != null)
        {
            //Check if some of the item is stored already
            foreach (Item storedItem in loadedResources)
            {
                if (item.itemID == storedItem.itemID)
                {
                    storedItem.quantity += item.quantity;
                    Destroy(item.gameObject);
                    DisplayRecipeChoice();
                    return;
                }
            }
            //if not already stored add to list
            loadedResources.Add(item);
            item.gameObject.SetActive(false);
            DisplayRecipeChoice();
            //Destroy(item.gameObject);
        }
    }


    public void DisplayRecipeChoice()
    {
        if (CraftingManager.instance.knownRecipes.Count < 1)
            return;

        recipeNameText.text = CraftingManager.instance.knownRecipes[currentlySelceted].recipeName;
        string recipeRequirements = "";
        //Hopefully make text show as "0/10 Wood" if you have 0 wood and the recipe reqires 10 wood
        //And do a resource per line
        foreach (CraftingManager.ResourceRequirement requirement in CraftingManager.instance.knownRecipes[currentlySelceted].requiredItems)
        {
            int sameResourceLoaded = 0;
            foreach (Item loadedItem in loadedResources)
            {
                if (loadedItem.itemName == requirement.resourceName)
                {
                    sameResourceLoaded += loadedItem.quantity;
                }
            }
            recipeRequirements += sameResourceLoaded.ToString() + "/" + requirement.numRequired.ToString() + " " + requirement.resourceName + "\n"; 
        }
        recipeRequirementsText.text = recipeRequirements;
    }

    public void NextRecipeChoice()
    {
        currentlySelceted++;
        if (currentlySelceted >= CraftingManager.instance.knownRecipes.Count)
            currentlySelceted = 0;
        DisplayRecipeChoice();
    }

    public void PreviousRecipeChocie()
    {
        currentlySelceted--;
        if (currentlySelceted < 0)
            currentlySelceted = CraftingManager.instance.knownRecipes.Count -1;
        DisplayRecipeChoice();
    }

    public void MakeItem()
    {
        if (CraftingManager.instance.knownRecipes.Count == 0)
            return;
        foreach (CraftingManager.ResourceRequirement requirement in CraftingManager.instance.knownRecipes[currentlySelceted].requiredItems)
        {
            bool hasItem = false;
            foreach (Item loadedItem in loadedResources)
            {
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
        foreach (CraftingManager.ResourceRequirement requirement in CraftingManager.instance.knownRecipes[currentlySelceted].requiredItems)
        {
            foreach (Item loadedItem in loadedResources)
            {
                if (loadedItem.itemName == requirement.resourceName)
                {
                    loadedItem.quantity -= requirement.numRequired;                    
                }
            }
        }
        GameObject newObject = (GameObject)Instantiate(CraftingManager.instance.knownRecipes[currentlySelceted].result, spawnPoint.position, Quaternion.identity);
        //Update sign with new reduced resources
        DisplayRecipeChoice();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingMenu : MonoBehaviour
{
    public enum DisplayItemType
    {
        Default = -1,
        ALL = 0,
        TOOLS = 1,
        BUILDINGS = 2,
    }
    public DisplayItemType currentRecipeType = DisplayItemType.ALL;
    public Color sufficientResourceColour = Color.green;
    public Color insufficientResourceColour = Color.gray;

    public List<CraftingRecipe> recipes = new List<CraftingRecipe>();
    public List<CraftingRecipe> toolRecipes = new List<CraftingRecipe>();
    public List<CraftingRecipe> buildingRecipes = new List<CraftingRecipe>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public List<CraftingRecipe> GetDisplayList()
    {
        if (currentRecipeType == DisplayItemType.ALL)
            return recipes;
        else if (currentRecipeType == DisplayItemType.TOOLS)
            return toolRecipes;
        else if (currentRecipeType == DisplayItemType.BUILDINGS)
            return buildingRecipes;


        return recipes;
    }

    public void SetDisplayRecipeType(DisplayItemType type)
    {
        currentRecipeType = type;
        GetDisplayList();
    }

    public void DisplayList(List<CraftingRecipe> recipes)
    {
        foreach (CraftingRecipe recipe in recipes)
        {
            string recipeName = recipe.recipeName;
            string recipeResources = "";
            foreach (CraftingManager.ResourceRequirement requirement in recipe.requiredItems)
            {
                bool hasResource = false;          
                foreach (GameObject heldItem in PlayerInventory.instance.heldObjects)
                {
                    Item item = heldItem.GetComponent<Item>();
                    if (item == null)
                        continue;
                    if (item.itemName == requirement.resourceName)
                        if (item.quantity >= requirement.numRequired)
                        {
                            hasResource = true;
                            recipeResources += "<color = sufficientResourceColour>" + requirement.numRequired + requirement.resourceName + ", </color>";
                            break;
                        }

                }
                if (!hasResource)
                    recipeResources += "<color = insufficientResourceColour>" + requirement.numRequired + requirement.resourceName + ", </color>";
            }
            //Display list prefab thing
            //Set name to = recipeName
            //Set required resoures = recipeResources
            
        }



    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingMenuButton : ScrollMenuButton
{
    public Text requirementText;
    public CraftingRecipe recipe;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	public override void UpdateSelectedButton ()
    {
        if (CraftingMenu.instance.selectedButton != null)
            CraftingMenu.instance.selectedButton.UnselectButton();
        CraftingMenu.instance.selectedButton = this;
        CraftingMenu.instance.UpdateSelectedItemInfo();
        SelectButton();
	}

    public override void UnselectButton()
    {
        nameText.text = recipe.recipeName;
    }

    public override void SelectButton()
    {
        nameText.text = "-> " + recipe.recipeName + " <-";
    }

    public override void UpdateDisplay()
    {
        string recipeName = "";
        string recipeResources = "";
        if (CraftingMenu.instance.selectedButton == this)
        {
            recipeName += "-> " + recipe.recipeName + " <-";
        }
        else
            recipeName = recipe.recipeName;
        bool hasResource = true;


        for (int i = 0; i < recipe.requiredItems.Count; i++)
        {
            int haveAmount = 0;

            recipeResources += recipe.requiredItems[i].numRequired + " " + recipe.requiredItems[i].resourceName.ToString();
            if (i < recipe.requiredItems.Count - 1)
                recipeResources += ", ";
    
          //  recipeResources += requirement.numRequired + requirement.resourceName + ", ";

            foreach (GameObject heldItem in PlayerInventory.instance.heldObjects)
            {
                if (heldItem == null)
                    continue;
                Item item = heldItem.GetComponent<Item>();
                if (item == null)
                    continue;
                if (item.itemName == recipe.requiredItems[i].resourceName)
                {
                    haveAmount += item.quantity;

                    if (haveAmount >= recipe.requiredItems[i].numRequired)
                        break;
                }
            }
            if (haveAmount < recipe.requiredItems[i].numRequired)
                hasResource = false;

        }
        //Display list prefab thing
        nameText.text = recipeName;
        requirementText.text = recipeResources;
        if (hasResource)
        {
            nameText.color = CraftingMenu.instance.canMakeColor;
            requirementText.color = CraftingMenu.instance.sufficientResourceColour;
        }
        else
        {
            nameText.color = CraftingMenu.instance.cantMakeColor;
            requirementText.color = CraftingMenu.instance.insufficientResourceColor;
        }
    }
}

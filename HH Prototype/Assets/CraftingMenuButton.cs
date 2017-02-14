using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingMenuButton : MonoBehaviour
{
    public int recipeIndex = -1;
    public Button button;
    public Text nameText;
    public Text requirementText;
    public Image iconImage;
    public CraftingMenu craftingMenu;
    public CraftingRecipe recipe;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	public void UpdateSelectedButton ()
    {
        if (CraftingMenu.instance.selectedButton != null)
            CraftingMenu.instance.selectedButton.UnselectButton();
        CraftingMenu.instance.selectedButton = this;
        CraftingMenu.instance.UpdateSelectedItemInfo();
        SelectButton();
	}

    public void UnselectButton()
    {
        nameText.text = recipe.recipeName;
    }

    public void SelectButton()
    {
        nameText.text = "-> " + recipe.recipeName + " <-";
    }

    public void UpdateDisplay()
    {
        string recipeName = "";
        string recipeResources = "";
        if (CraftingMenu.instance.selectedButton == this)
        {
            recipeName += "-> " + recipe.recipeName + " <-";
        }
        else
            recipeName = recipe.recipeName;
        bool hasResource = false;

        foreach (CraftingManager.ResourceRequirement requirement in recipe.requiredItems)
        {
            foreach (GameObject heldItem in PlayerInventory.instance.heldObjects)
            {
                if (heldItem == null)
                    continue;
                Item item = heldItem.GetComponent<Item>();
                if (item == null)
                    continue;
                if (item.itemName == requirement.resourceName)
                    if (item.quantity >= requirement.numRequired)
                    {
                        hasResource = true;
                        recipeResources += requirement.numRequired + requirement.resourceName + ", ";
                        //recipeResources += "<color = sufficientResourceColour>" + requirement.numRequired + requirement.resourceName + ", </color>";
                        break;
                    }

            }
            if (!hasResource)
                recipeResources += requirement.numRequired + requirement.resourceName + ", ";
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
            requirementText.color = CraftingMenu.instance.insufficientResourceColour;
        }
    }
}

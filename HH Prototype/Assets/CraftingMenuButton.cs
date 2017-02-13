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
	public void SetMenuIndex ()
    {
        craftingMenu.selectedButton = this;
	}

    public void UpdateDisplay()
    {
        string recipeName = recipe.recipeName;
        string recipeResources = "";
        bool hasResource = false;

        foreach (CraftingManager.ResourceRequirement requirement in recipe.requiredItems)
        {
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
        nameText.text = recipeName;
        requirementText.text = recipeResources;
        if (hasResource)
            nameText.color = CraftingMenu.instance.canMakeColor;
        else
            nameText.color = CraftingMenu.instance.cantMakeColor;
    }
}

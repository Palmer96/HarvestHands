using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionMenuButton : ScrollMenuButton
{
    public Text requirementText;
    public Construct recipe;

    public override void UpdateSelectedButton()
    {
        if (ConstructionMenu.instance.selectedButton != null)
            ConstructionMenu.instance.selectedButton.UnselectButton();
        ConstructionMenu.instance.selectedButton = this.GetComponent<ConstructionMenuButton>();
        ConstructionMenu.instance.UpdateSelectedItemInfo();
        SelectButton();
    }

    public override void UnselectButton()
    {
        nameText.text = recipe.constructName;
    }

    public override void SelectButton()
    {
        nameText.text = "-> " + recipe.constructName + " <-";
    }

    public override void UpdateDisplay()
    {
        string recipeName = "";
        string recipeResources = "";
        if (CraftingMenu.instance.selectedButton == this)
        {
            recipeName += "-> " + recipe.constructName + " <-";
        }
        else
            recipeName = recipe.constructName;

        foreach (Building.ResourceRequired requirement in recipe.selfObject.GetComponent<Building>().resources)
        {
            //recipeResources += requirement.numRequired + Building.EnumToString(requirement.resource) + ", ";
            recipeResources += requirement.numRequired + requirement.resource.ToString() + ", ";
        }
        //Display list prefab thing
        Debug.Log("nameText.text = recipe name, recipename == " + recipeName);
        nameText.text = recipeName;
        requirementText.text = recipeResources;
        nameText.color = CraftingMenu.instance.canMakeColor;
        requirementText.color = CraftingMenu.instance.sufficientResourceColour;
        
    }

}

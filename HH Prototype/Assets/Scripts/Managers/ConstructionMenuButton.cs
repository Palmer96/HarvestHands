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

        for (int i = 0; i < recipe.selfObject.GetComponent<Building>().resources.Count; i++)
        {
            recipeResources += recipe.selfObject.GetComponent<Building>().resources[i].numRequired + " " + recipe.selfObject.GetComponent<Building>().resources[i].resource.ToString();
            if (i < recipe.selfObject.GetComponent<Building>().resources.Count - 1)
                recipeResources += ", ";
        }
        
        //Display list prefab thing
      //  Debug.Log("nameText.text = recipe name, recipename == " + recipeName);
        nameText.text = recipeName;
        requirementText.text = recipeResources;
        nameText.color = ConstructionMenu.instance.canMakeColor;
        requirementText.color = ConstructionMenu.instance.sufficientResourceColour;
        
    }

}

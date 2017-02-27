using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestMenuButton : ScrollMenuButton
{
    public QuestPrototype recipe;
    public Text questDescription;
    public Text questObjective;
       

    public override void UpdateSelectedButton()
    {
        if (QuestMenu.instance.selectedButton != null)
            QuestMenu.instance.selectedButton.UnselectButton();
        QuestMenu.instance.selectedButton = this;
        QuestMenu.instance.UpdateSelectedItemInfo();
        SelectButton();
    }

    public override void UpdateDisplay()
    {
        string recipeName = "";
        //string recipeResources = "";
        if (QuestMenu.instance.selectedButton == this)
        {
            recipeName += "-> " + recipe.questName + " <-";
        }
        else
            recipeName = recipe.questName;

        //Display list prefab thing
        nameText.text = recipeName;
        questObjective.text = recipe.objectives[recipe.currentObjective].objectiveDescription;

        nameText.color = QuestMenu.instance.canMakeColor;
        questObjective.color = QuestMenu.instance.sufficientResourceColour;        
    }
}

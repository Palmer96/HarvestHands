using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeCraftObjective : QuestProtoypeObjective
{
    public string itemName = "";
    public int goalAmount = 0;
    public int currentAmount = 0;

	// Use this for initialization
	void Start ()
    {    
        type = objectiveType.Craft;
	}

    public override void ActivateObjective()
    {
        Debug.Log("Activating craft objective");
        EventManager.OnCraft += CheckComplete;
    }

    public override void DectivateObjective()
    {
        EventManager.OnCraft -= CheckComplete;
    }

    void CheckComplete(string plantType)
    {
        currentAmount++;
        if (currentAmount >= goalAmount)
        {
            objectiveDone = true;
            PrototypeQuestManager.UpdateQuests();
        }

        //if (plantType == plantName)
        //{
        //    currentAmount++;
        //    if (currentAmount >= goalAmount)
        //    {
        //        objectiveDone = true;
        //        Debug.Log(currentAmount + "/" + goalAmount + " " + plantType + " harvested!");
        //        //GenerateRewards();
        //        PrototypeQuestManager.UpdateQuests();
        //    }
        //}
    }

    //Helps with save/load
    public override int GetCurrentObjectiveValue()
    {
        return goalAmount;
    }

    //Helps with save/load
    public override void SetCurrentObjectiveValue(int amount)
    {
        goalAmount = amount;
    }
}

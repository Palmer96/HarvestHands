using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeWaterObjective : QuestProtoypeObjective
{
    public int goalAmount = 0;
    public int currentNumber = 0;
        
    // Use this for initialization
    void Start ()
    {
        type = objectiveType.Water;
	}

    public override void ActivateObjective()
    {
        Debug.Log("Activating harvest objective");
        EventManager.OnWater += CheckComplete;
    }

    public override void DectivateObjective()
    {
        EventManager.OnWater -= CheckComplete;
    }

    void CheckComplete(string plantType)
    {
        currentNumber++;
        if (currentNumber >= goalAmount)
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
}

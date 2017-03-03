using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypePlantObjective : QuestProtoypeObjective
{
    public string plantName = "WhatToPlant";
    public int goalAmount = 1;
    public int currentAmount = 0;

    void Start()
    {
        type = objectiveType.Plant;
    }

    public override void ActivateObjective()
    {
        Debug.Log("Activating on plant objective");
        EventManager.OnPlant += CheckComplete;
    }

    public override void DectivateObjective()
    {
        EventManager.OnPlant -= CheckComplete;
    }

    void CheckComplete(string objectType)
    {
        if (plantName == objectType)
        {
            currentAmount++;
            if (currentAmount >= goalAmount)
            {
                objectiveDone = true;
                Debug.Log(objectType + " planted!");
                //GenerateRewards();
                PrototypeQuestManager.UpdateQuests();
            }
        }

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeHarvestObjective : QuestProtoypeObjective
{
    public string plantName = "WhatToHarvest";
    public int goalAmount = 1;
    public int currentAmount = 0;

    void Start()
    {
        type = objectiveType.Construct;
    }

    public override void ActivateObjective()
    {
        Debug.Log("Activating harvest objective");
        EventManager.OnHarvest += CheckComplete;
    }

    public override void DectivateObjective()
    {
        EventManager.OnHarvest -= CheckComplete;
    }

    void CheckComplete(string plantType)
    {
        if (plantType == plantName)
        {
            currentAmount++;
            if (currentAmount >= goalAmount)
            {
                objectiveDone = true;
                Debug.Log(currentAmount + "/" + goalAmount + " " + plantType + " harvested!");
                //GenerateRewards();
                PrototypeQuestManager.UpdateQuests();
            }
        }
    }
}

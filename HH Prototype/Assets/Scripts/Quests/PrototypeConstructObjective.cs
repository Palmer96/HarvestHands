using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeConstructObjective : QuestProtoypeObjective
{
    public string constructName = "WhatToBuild";
    public int goalAmount = 1;
    public int currentAmount = 0;

    void Start()
    {
        type = objectiveType.Construct;
    }

    public override void ActivateObjective()
    {
        //Debug.Log("Activating construct objective");
        EventManager.OnConstruct += CheckComplete;
    }

    public override void DectivateObjective()
    {
        EventManager.OnConstruct -= CheckComplete;
    }

    void CheckComplete(string buildingName)
    {
        if (constructName == buildingName)
        {
            currentAmount++;
            if (currentAmount >= goalAmount)
            {
                objectiveDone = true;
                Debug.Log(currentAmount + "/" + goalAmount + " " + constructName + " constructed!");
                PrototypeQuestManager.UpdateQuests();
            }
        }
    }
}

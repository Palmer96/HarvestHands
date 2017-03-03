using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeSellObjective : QuestProtoypeObjective
{
    public string objectName = "WhatToSell";
    public int goalAmount = 1;
    public int currentAmount = 0;

    void Start()
    {
        type = objectiveType.Sell;
    }

    public override void ActivateObjective()
    {
        Debug.Log("Activating on sell objective");
        EventManager.OnSell += CheckComplete;
    }

    public override void DectivateObjective()
    {
        EventManager.OnSell -= CheckComplete;
    }

    void CheckComplete(string objectType)
    {
        if (objectName == objectType)
        {
            currentAmount++;
            if (currentAmount >= goalAmount)
            {
                objectiveDone = true;
                Debug.Log(objectType + " sold!");
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

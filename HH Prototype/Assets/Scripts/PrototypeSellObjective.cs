using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeSellObjective : QuestProtoypeObjective
{
    public string objectName = "WhatToSell";

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
            objectiveDone = true;
            Debug.Log(objectType + " sold!");
            //GenerateRewards();
            PrototypeQuestManager.UpdateQuests();
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypePlantObjective : QuestProtoypeObjective
{
    public string plantName = "WhatToPlant";

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
            objectiveDone = true;
            Debug.Log(objectType + " planted!");
            //GenerateRewards();
            PrototypeQuestManager.UpdateQuests();
        }

    }
}

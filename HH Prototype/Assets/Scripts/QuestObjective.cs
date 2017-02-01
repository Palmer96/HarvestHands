using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "Data", menuName = "QuestObjectives", order = 2)]
public class QuestObjective : ScriptableObject
{
    public enum ObjectiveType
    {
        Harvest,
        Sell,
        TalkTo,
    }

    public ObjectiveType objectiveType;
    public bool objectiveDone = false;
    public string objectiveDescription = "";
               
    public virtual void ActivateObjective()
    {

    }

    public virtual void DectivateObjective()
    {

    }
}

[CreateAssetMenu(fileName = "Data", menuName = "Quest/Objective/Harvest", order = 1)]
public class HarvestObjective : QuestObjective
{
    public string plantName = "WhatToHarvest";
    public int goalAmount = 1;
    public int currentAmount = 0;

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
                QuestManager.UpdateQuests();
            }
        }
    }
}

[CreateAssetMenu(fileName = "Data", menuName = "Quest/Objective/Talk", order = 2)]
public class TalkObjective : QuestObjective
{
    public string goalName = "NPCName";

    public override void ActivateObjective()
    {
        Debug.Log("Activating talk to objective");
        EventManager.OnTalk += CheckComplete;
    }

    public override void DectivateObjective()
    {
        EventManager.OnTalk -= CheckComplete;
    }

    void CheckComplete(string npcName)
    {
        if (goalName == npcName)
        {
            objectiveDone = true;
            Debug.Log(npcName + " talked to!");
            
            //GenerateRewards();
            QuestManager.UpdateQuests();
        }

    }
}

[CreateAssetMenu(fileName = "Data", menuName = "Quest/Objective/Sell", order = 3)]
public class SellObjective : QuestObjective
{
    public string objectName = "WhatToSell";
    
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
            QuestManager.UpdateQuests();
        }

    }
}
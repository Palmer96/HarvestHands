using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeTalkObjective : QuestProtoypeObjective
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
            PrototypeQuestManager.UpdateQuests();
        }

    }
}

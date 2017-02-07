using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPrototype : MonoBehaviour
{
    public string questName = "";
    public string questDescription = "";
    public bool questAccepted = false;
    public bool questComplete = false;
    public int currentObjective = 0;

    public List<QuestProtoypeObjective> objectives = new List<QuestProtoypeObjective>();
    public List<PrototypeQuestReward> rewards = new List<PrototypeQuestReward>();

    public void StartQuest(int atObjective = 0)
    {
        Debug.Log("inside start quest - objectives.count = " + objectives.Count);
        questAccepted = true;
        objectives[atObjective].ActivateObjective();
    }

    public void ActivateObjective(int objective = 0)
    {
        objectives[currentObjective].DectivateObjective();
        currentObjective = objective;
        objectives[currentObjective].ActivateObjective();
    }

    public void NextObjective()
    {
        Debug.Log("NextObjective");
        //Unsubscribe completed objective
        objectives[currentObjective].DectivateObjective();
        //Activate new objective
        currentObjective++;

        //If quest is completed
        if (currentObjective < objectives.Count)
        {
            objectives[currentObjective].ActivateObjective();
        }
        //If quest not complete
        else
        {
            questComplete = true;
            GenerateRewards();
            //QuestManager.instance.completedQuests.Add(this);
            Debug.Log("CONGRATULATIONS! QUEST COMPLETED!");
        }
    }

    public void GenerateRewards()
    {
        Debug.Log("Giving Reward");
        foreach (PrototypeQuestReward reward in rewards)
        {
            reward.GenerateReward();
        }
    }
}

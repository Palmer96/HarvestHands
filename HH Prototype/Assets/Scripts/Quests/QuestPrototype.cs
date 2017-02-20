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
    public List<PrototypeQuestPrerequisite> prerequisites = new List<PrototypeQuestPrerequisite>();

    public void StartQuest(int atObjective = 0)
    {
        //Debug.Log("inside start quest - objectives.count = " + objectives.Count);
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
        //Unsubscribe completed objective
        objectives[currentObjective].objectiveDone = true;
        objectives[currentObjective].DectivateObjective();
        
        //Activate new objective
        currentObjective++;

        //If quest not completed
        if (currentObjective < objectives.Count)
        {
            objectives[currentObjective].ActivateObjective();
        }
        //If quest is complete
        else
        {
            questComplete = true;
            GenerateRewards();
            //QuestManager.instance.completedQuests.Add(this);            
            Debug.Log("CONGRATULATIONS! QUEST COMPLETED!");
        }
        //PrototypeQuestManager.UpdateQuests();
        //PrototypeQuestManager.instance.UpdateQuestText();
    }

    public void GenerateRewards()
    {
        Debug.Log("Giving Reward");
        foreach (PrototypeQuestReward reward in rewards)
        {
            reward.GenerateReward();
        }
    }

    public void CompleteTalkObjective()
    {
        //Check for objectives to talk to people
        if (objectives[currentObjective].type == QuestProtoypeObjective.objectiveType.Talk)
        {
            PrototypeTalkObjective objective = (PrototypeTalkObjective)objectives[currentObjective];
            if (objective != null)
            {
                objective.objectiveDone = true;
                PrototypeQuestManager.UpdateQuests();
            }
        }
    }

    public bool CheckPrerequisitesMet()
    {
        foreach (PrototypeQuestPrerequisite requirement in prerequisites)
        {
            if (!requirement.CheckPrerequisiteMet())
                return false;
        }
        return true;
    }
}

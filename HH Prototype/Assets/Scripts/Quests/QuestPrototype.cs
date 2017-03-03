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

    void Start()
    {
        SaveAndLoadManager.OnSave += Save;
    }

    public void StartQuest(int atObjective = 0)
    {
        //Debug.Log("inside start quest - objectives.count = " + objectives.Count);
        PrototypeQuestManager.instance.activeQuests.Add(this);
        questAccepted = true;
        objectives[atObjective].ActivateObjective();
        PrototypeQuestManager.instance.UpdateQuestText();
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

    void OnDestroy()
    {
        SaveAndLoadManager.OnSave -= Save;
    }

    public virtual void Save()
    {
        SaveAndLoadManager.instance.saveData.questSaveList.Add(new QuestSave(this));
        //Debug.Log("Saved item = " + name);
    }
}


[System.Serializable]
public class QuestSave
{
    string questName;
    bool questAccepted;
    bool questComplete;
    int currentObjective;
    int currenObjectiveGoalValue;

    public QuestSave(QuestPrototype quest)
    {

        questName = quest.questName;
        questAccepted = quest.questAccepted;
        questComplete = quest.questComplete;
        currentObjective = quest.currentObjective;
        if (!quest.questComplete) //if complete, index is > objectives.count (i think)
            currenObjectiveGoalValue = quest.objectives[currentObjective].GetCurrentObjectiveValue();
    }

    public GameObject LoadObject()
    {
        foreach (QuestPrototype questPrefab in QuestGrabber.questList)
        {
            if (questPrefab == null)
                continue;

            //Debug.Log("questPrefab.questName(" + questPrefab.questName + ") vs questName(" + questName + ")");
            if (questPrefab.questName == questName)
            {
                //Debug.Log("Loading Item");
                questPrefab.questAccepted = questAccepted;
                questPrefab.questComplete = questComplete;
                questPrefab.currentObjective = currentObjective;
                if (!questComplete) //if complete, index is > objectives.count (i think)
                {
                    questPrefab.objectives[currentObjective].SetCurrentObjectiveValue(currenObjectiveGoalValue);
                    for (int i = 0; i < currentObjective; ++i)
                    {
                        questPrefab.objectives[i].objectiveDone = true;
                    }
                }
                else
                {
                    for (int i = 0; i < questPrefab.objectives.Count; ++i)
                    {
                        questPrefab.objectives[i].objectiveDone = true;
                    }
                }
                if (questAccepted)
                {
                    if (!questComplete)
                    {
                        PrototypeQuestManager.instance.activeQuests.Add(questPrefab);
                    }
                }

                return null;
            }
        }
        Debug.Log("Failed to load Quest, questName = " + questName.ToString());
        return null;
    }
}
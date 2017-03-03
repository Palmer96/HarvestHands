using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrototypeQuestManager : MonoBehaviour
{
    public static PrototypeQuestManager instance;
    public int activeQuestIndex = 0;
    public List<QuestPrototype> activeQuests = new List<QuestPrototype>();
    public List<QuestPrototype> completedQuests = new List<QuestPrototype>();


    public Quest startingQuest;
    public Text questDescriptionText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }       
        UpdateQuestText();
    }

    void Start()
    {
        SaveAndLoadManager.OnSave += Save;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            ChangeActiveQuest();
        }
    }

    public static void UpdateQuests()
    {
        List<QuestPrototype> newCompletedQuests = new List<QuestPrototype>();
        foreach (QuestPrototype quest in instance.activeQuests)
        {
            if (quest == null)
                continue;
            //If current objective is done
            if (quest.objectives[quest.currentObjective].objectiveDone)
            {
                quest.NextObjective();
            }
            //Skip completed quests
            if (quest.questComplete)
            {
                Debug.Log("Inside if (quest.questcomplete");
                newCompletedQuests.Add(quest);
                continue;
            }

            //Debug.Log("outside currentobjective done");
        }

        //Debug.Log("activequests After = " + instance.activeQuests.Count);
        foreach (QuestPrototype quest in newCompletedQuests)
        {
            instance.completedQuests.Add(quest);
            instance.activeQuests.Remove(quest);
            instance.ChangeActiveQuest();
        }


        instance.UpdateQuestText();
    }

    public void UpdateQuestText()
    {
        if (questDescriptionText == null)
            return;

        if (activeQuests.Count > 0)
        {
            questDescriptionText.text = "Quest: " + activeQuests[activeQuestIndex].questName + "\n"
                + "Objective: " + activeQuests[activeQuestIndex].objectives[activeQuests[activeQuestIndex].currentObjective].objectiveDescription;
        }
        else
            questDescriptionText.text = "";
    }

    public void ChangeActiveQuest()
    {
        if (activeQuests.Count == 0)
        {
            return;
        }

        if (activeQuestIndex >= activeQuests.Count - 1)
            activeQuestIndex = 0;
        else
            activeQuestIndex++;

        UpdateQuestText();
    }

    public int CheckTalkChat(string npcName)
    {
        foreach (QuestPrototype quest in activeQuests)
        {
            //Check for objectives to talk to people
            if (quest.objectives[quest.currentObjective].type == QuestProtoypeObjective.objectiveType.Talk)
            {
                PrototypeTalkObjective objective = (PrototypeTalkObjective)quest.objectives[quest.currentObjective];
                if (objective != null)
                {
                    //if have to talk to person
                    if (objective.goalName == npcName)
                        //return where to start conversation and hope it does the quest stuff in dialogue actions :)
                        return objective.startConversationNode;
                }
            }
        }

        return -1;
    }

    void OnDestroy()
    {
        SaveAndLoadManager.OnSave -= Save;
    }

    public void Save()
    {
        SaveAndLoadManager.instance.saveData.questManagerSave = new QuestManagerSave(this);
        //Debug.Log("Saved item = " + name);
    }
}

[System.Serializable]
public class QuestManagerSave
{
    List<string> activeQuests;
    List<string> completedQuests;
    int activeQuestIndex;

    public QuestManagerSave(PrototypeQuestManager questManager)
    {
        activeQuestIndex = questManager.activeQuestIndex;
        activeQuests = new List<string>();
        completedQuests = new List<string>();
        foreach (QuestPrototype quest in questManager.activeQuests)
        {
            activeQuests.Add(quest.questName);
        }
        foreach (QuestPrototype quest in questManager.completedQuests)
        {
            completedQuests.Add(quest.questName);
        }
    }

    public GameObject LoadObject()
    {
        PrototypeQuestManager.instance.activeQuestIndex = activeQuestIndex;

        PrototypeQuestManager.instance.activeQuests = new List<QuestPrototype>();
        PrototypeQuestManager.instance.completedQuests = new List<QuestPrototype>();
        foreach (QuestPrototype quest in QuestGrabber.questList)
        {
            foreach (string questName in activeQuests)
            {
                if (questName == quest.questName)
                {
                    PrototypeQuestManager.instance.activeQuests.Add(quest);
                    break;
                }
            }
            foreach (string questName in completedQuests)
            {
                if (questName == quest.questName)
                {
                    PrototypeQuestManager.instance.completedQuests.Add(quest);
                    break;
                }
            }
//    Debug.Log("Failed to add active or completed saveData quest to questManager active/completed list, questName = " + quest.questName.ToString());
        }
        PrototypeQuestManager.instance.UpdateQuestText();

        return null;
    }
}
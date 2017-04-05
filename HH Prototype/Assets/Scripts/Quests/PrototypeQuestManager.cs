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
                //Debug.Log("Inside if (quest.questcomplete");
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

        instance.UpdateNPCQuestMarkers();
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

    public void UpdateNPCQuestMarkers()
    {        
        for (int i = 0; i < NPC.npcList.Count; ++i)
        {
            //Turn off npc marks
            NPC.npcList[i].SetQuestMarkerVisible(false);
            //Turn on marker if they have a quest to give
            if (NPC.npcList[i].acceptableQuests.Count > 0)
            {
                NPC.npcList[i].SetQuestMarkerVisible(true);
                Debug.Log("Turning on questmarker for " + NPC.npcList[i] + ": has quest to give");
            }
        }       
        //Turn on npc marker if they are a goal of a quest which is not started yet but prerequisites are met
        for (int i = 0; i < QuestGrabber.questList.Count; ++i)
        {
            //skip accepted quests        
            if (!QuestGrabber.questList[i].questAccepted)
            {
                //if prerequisites completed
                if (QuestGrabber.questList[i].prerequisites.Count > 0)
                {

                    if (QuestGrabber.questList[i].CheckPrerequisitesMet())
                    {
                        if (QuestGrabber.questList[i].objectives[0].type == QuestProtoypeObjective.objectiveType.Talk)
                        {
                            for (int j = 0; j < NPC.npcList.Count; ++j)
                            {
                                if (NPC.npcList[j].npcName == (QuestGrabber.questList[i].objectives[0] as PrototypeTalkObjective).goalName)
                                {
                                    NPC.npcList[j].SetQuestMarkerVisible(true);
                                    Debug.Log("Turning on questmarker for " + NPC.npcList[i] + ": " + QuestGrabber.questList[i] + " prerequisites met");
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        //Turn on npc marker if they are currently a talk objective goal
        for (int i = 0; i < activeQuests.Count; ++i)
        {
            if (activeQuests[i].objectives[activeQuests[i].currentObjective].type == QuestProtoypeObjective.objectiveType.Talk)
            {
                for (int j = 0; j < NPC.npcList.Count; ++j)
                {
                    if ((activeQuests[i].objectives[activeQuests[i].currentObjective] as PrototypeTalkObjective).goalName == NPC.npcList[j].npcName)
                    {
                        if (NPC.npcList[j].npcName == "Melissa")
                        {
                            NPC.npcList[j].SetQuestMarkerVisible(true, "¿");
                        }
                        else
                        {
                            NPC.npcList[j].SetQuestMarkerVisible(true, "?");
                            Debug.Log("Turning on questmarker for " + NPC.npcList[j] + ": talk objective goal: " + activeQuests[i].questName);
                        }
                    }
                }
            }
        }

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
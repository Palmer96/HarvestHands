using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Quest/QuestBase", order = 0)]
public class Quest : ScriptableObject
{
    public string questName = "questName";
    public bool questAccepted = false;
    public bool questComplete = false;
    public int currentObjective;
    public List<QuestObjective> objectives = new List<QuestObjective>();
    public List<QuestReward> rewards = new List<QuestReward>();

    public string questDescription = "";
    //public List<QuestReward> rewards = new List<QuestReward>();

	// Use this for initialization
	void Start ()
    {
        QuestManager.instance.activeQuests.Add(this);
        Debug.Log("Active Quest Size = " + QuestManager.instance.activeQuests.Count);

        //QuestObjective objective = QuestObjective.CreateInstance(objectives[currentObjective].name) as QuestObjective;

        Debug.Log("Number of objectives = " + objectives.Count);

        Debug.Log("calling start Quest");
        StartQuest();
	}

    public void StartQuest()
    {
        Debug.Log("inside start quest");
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
        foreach (QuestReward reward in rewards)
        {
            reward.GiveReward();
        }
    }

    public static Quest LoadQuest(string questPath)
    {
        //Debug.Log("Quest path = '" + "Quest/FindMountainMan" + "' Works");
        //Debug.Log("Quest path = '" + questPath + "' DoesntWork");
        Object[] assets = Resources.LoadAll(questPath) as Object[];
        Debug.Log("There are " + assets.Length + " assets");
        //Quest quest = new Quest();
        Quest quest = CreateInstance<Quest>();
        List<QuestReward> rewards = new List<QuestReward>();
        List<QuestObjective> objectives = new List<QuestObjective>();

        foreach (Object asset in assets)
        {
            if (asset is QuestObjective)
            {
                objectives.Add(asset as QuestObjective);
                //Debug.Log("Objective asset loaded");
            }
            else if (asset is QuestReward)
            {
                rewards.Add(asset as QuestReward);
                //Debug.Log("Reward asset loaded");
            }
            else if (asset is Quest)
            {
                quest = asset as Quest;
                //Debug.Log("Quest asset loaded");
            }
        }

        quest.objectives = objectives;
        quest.rewards = rewards;

        Debug.Log("Recreated questName = " + quest.questName);
        Debug.Log("Recreated has objectives = " + quest.objectives.Count);
        Debug.Log("Recreated has rewards = " + quest.rewards.Count);
        return quest;
    }
}

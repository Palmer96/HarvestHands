using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Quest/QuestBase", order = 0)]
public class Quest : ScriptableObject
{
    public bool questAccepted = false;
    public bool questComplete = false;
    public List<QuestObjective> objectives = new List<QuestObjective>();
    public int currentObjective;

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
	
	// Update is called once per frame
	void Update ()
    {
        
    }

    public void StartQuest()
    {
        Debug.Log("inside start quest");
        objectives[currentObjective].ActivateObjective();
    }

    public void NextObjective()
    {
        Debug.Log("NextObjective");
        //Give reward and unsubscribe completed objective
        objectives[currentObjective].GenerateRewards();
        objectives[currentObjective].DectivateObjective();
        //Activate new objective
        currentObjective++;

        if (currentObjective < objectives.Count)
        {
            objectives[currentObjective].ActivateObjective();
        }
        else
        {
            questComplete = true;
            QuestManager.instance.completedQuests.Add(this);
            Debug.Log("CONGRATULATIONS! QUEST COMPLETED!");
        }
    }
}

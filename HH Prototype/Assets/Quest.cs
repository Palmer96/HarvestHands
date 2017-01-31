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
    //public List<QuestReward> rewards = new List<QuestReward>();

	// Use this for initialization
	void Start ()
    {
        QuestManager.instance.activeQuests.Add(this);
        Debug.Log("Active Quest Size = " + QuestManager.instance.activeQuests.Count);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void NextObjective()
    {     
        //Unsubscribe completed objective
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
        }
    }
}

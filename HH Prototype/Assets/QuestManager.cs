using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "Data", menuName = "Quests", order = 1)]
public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    public List<Quest> activeQuests = new List<Quest>();

    public Quest startingQuest;

    // Use this for initialization
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
        //activeQuests.Add(ScriptableObject.CreateInstance("HarvestPlant1"));
        if (startingQuest != null)
        {
            //activeQuests.Add(Quest.CreateInstance(typeof(Quest));
            //Quest quest = Quest.CreateInstance(typeof(Quest));
            activeQuests.Add(startingQuest);
        }
        
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public static void UpdateQuests()
    {
        foreach (Quest quest in instance.activeQuests)
        {
            //Skip completed quests
            if (quest.questComplete)
                continue;

            //If current objective is done
            if (quest.objectives[quest.currentObjective].objectiveDone)
            {
                quest.NextObjective();
            }
        }
    }
}

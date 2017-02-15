using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public static List<NPC> npcList = new List<NPC>();

    public struct QuestArousalThing
    {
        int arousalRequirement;
        Quest quest;
    }

    public string npcName = "xXxPussySlayer69xXx";
    public int arousalValue = 0;

	// Use this for initialization
	void Start ()
    {
        npcList.Add(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeConversationStartNode(int i)
    {
        if (GetComponent<VIDE_Assign>() != null)
        {
            GetComponent<VIDE_Assign>().overrideStartNode = i;
        }
    }

    public void AddArousal(int value)
    {
        arousalValue += value;
    }

    public List<QuestPrototype> questPool = new List<QuestPrototype>();
    public List<QuestPrototype> acceptableQuests = new List<QuestPrototype>();

    public void CheckForNewPotentialQuests()
    {
        if (questPool.Count < 1)
            return;

        for (int i = questPool.Count; i > 0; --i)
        {
            if (questPool[i - 1].CheckPrerequisitesMet())
            {
                acceptableQuests.Add(questPool[i - 1]);
                questPool.RemoveAt(i - 1);
            }
        }
    }

    public QuestPrototype AcceptQuest()
    {        
        if (acceptableQuests.Count < 1)
            return null;
        int index = Random.Range(0, acceptableQuests.Count);
        QuestPrototype quest = acceptableQuests[index];
        quest.StartQuest();
        PrototypeQuestManager.instance.activeQuests.Add(quest);
        acceptableQuests.RemoveAt(index);
        return quest;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBoard : MonoBehaviour
{
    public List<Quest> potentialQuests = new List<Quest>();
    public List<Quest> acceptedQuests = new List<Quest>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GetRandomQuest()
    {
        if (potentialQuests.Count < 1)
            return;

        int index = Random.Range(0, potentialQuests.Count);
        //QuestManager.instance.activeQuests.Add(potentialQuests[index]);

        Quest newQuest = Quest.CreateInstance<Quest>();
        newQuest = potentialQuests[index];
        QuestManager.instance.activeQuests.Add(Instantiate(newQuest));
        newQuest.StartQuest();

        acceptedQuests.Add(potentialQuests[index]);
        potentialQuests.Remove(potentialQuests[index]);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBoard : MonoBehaviour
{
    public List<Quest> potentialQuests = new List<Quest>();
    public List<Quest> acceptedQuests = new List<Quest>();
    public GameObject questOfferPrefab;
    public Canvas uiCanvas;

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
        //Choose random quest
        int index = Random.Range(0, potentialQuests.Count);
        Quest newQuest = Quest.CreateInstance<Quest>();
        newQuest = potentialQuests[index];

        GameObject questOfferObject = Instantiate(questOfferPrefab);
        QuestOffer questOffer = questOfferObject.GetComponent<QuestOffer>();
        questOffer.questOffered = newQuest;
        questOffer.questBoardSource = this;
        questOffer.questBoardIndex = index;
        potentialQuests.Remove(potentialQuests[index]);
        questOfferObject.transform.SetParent(uiCanvas.transform);
        questOffer.ShowOffer();


        //QuestManager.instance.activeQuests.Add(Instantiate(newQuest));
        //newQuest.StartQuest();
        //acceptedQuests.Add(potentialQuests[index]);
        //
        //QuestManager.instance.UpdateQuestText();
    }
}

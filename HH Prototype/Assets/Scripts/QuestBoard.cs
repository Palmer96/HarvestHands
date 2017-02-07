using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBoard : MonoBehaviour
{
    public List<Quest> potentialQuests = new List<Quest>();
    public List<Quest> acceptedQuests = new List<Quest>();
    public GameObject questOfferPrefab;
    public Canvas uiCanvas;

    public List<QuestObjective> testList = new List<QuestObjective>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GetRandomQuest()
    {
        //if (potentialQuests.Count < 1)
        //    return;
        ////Choose random quest
        //int index = Random.Range(0, potentialQuests.Count);
        //Debug.Log("RandIndex = " + index.ToString());
        //Quest newQuest = Quest.LoadQuest("Quest/" + potentialQuests[index].questName.ToString());
        //GameObject questOfferObject = Instantiate(questOfferPrefab);
        //QuestOffer questOffer = questOfferObject.GetComponent<QuestOffer>();
        //questOffer.questOffered = newQuest;
        //questOffer.questBoardSource = this;
        //questOffer.questBoardIndex = index;
        //questOfferObject.transform.SetParent(uiCanvas.transform);
        //questOffer.ShowOffer();
        //
        //Debug.Log("NoticeBoardVesion " + potentialQuests[index].questName.ToString() + " - has " + potentialQuests[index].objectives.Count + " objectives");
        //Debug.Log("NoticeBoardVesion " + potentialQuests[index].questName.ToString() + " - has " + potentialQuests[index].rewards.Count + " rewards");
        //Debug.Log("QuestOfferVersion " + questOffer.questOffered.questName.ToString() + " - has " + questOffer.questOffered.objectives.Count + " objectives");
        //Debug.Log("QuestOfferVersion " + questOffer.questOffered.questName.ToString() + " - has " + questOffer.questOffered.rewards.Count + " rewards");
        //
        //potentialQuests.Remove(potentialQuests[index]);
        //
        ////QuestManager.instance.activeQuests.Add(Instantiate(newQuest));
        ////newQuest.StartQuest();
        ////acceptedQuests.Add(potentialQuests[index]);
        ////
        //QuestManager.instance.UpdateQuestText();
    }
}

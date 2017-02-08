using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeObjectiveBoard : MonoBehaviour
{
    public List<QuestPrototype> potentialQuests = new List<QuestPrototype>();
    public List<QuestPrototype> acceptedQuests = new List<QuestPrototype>();
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
        GameObject questOfferObject = Instantiate(questOfferPrefab);
        PrototypeQuestOffer questOffer = questOfferObject.GetComponent<PrototypeQuestOffer>();
        questOffer.questOffered = potentialQuests[index];
        questOffer.questBoardSource = this;
        questOffer.questBoardIndex = index;
        questOfferObject.transform.SetParent(uiCanvas.transform);
        questOffer.ShowOffer();       
        potentialQuests.Remove(potentialQuests[index]);
        
        PrototypeQuestManager.instance.UpdateQuestText();
    }
}

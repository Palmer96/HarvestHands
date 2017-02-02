using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestOffer : MonoBehaviour
{
    public Quest questOffered;
    public QuestBoard questBoardSource;
    public KeyCode acceptButton = KeyCode.Z;
    public KeyCode declineButton = KeyCode.X;

    public Text questTitleText;
    public Text questDescriptionText;
    public Text acceptButtonText;
    public Text declineButtonText;

    public int questBoardIndex;
    

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetKeyDown(acceptButton))
        {
            QuestManager.instance.activeQuests.Add(Instantiate(questOffered));
            questBoardSource.acceptedQuests.Add(questOffered);
            questOffered.StartQuest();
            QuestManager.instance.UpdateQuestText();



            //QuestManager.instance.activeQuests.Add(Instantiate(newQuest));
            //newQuest.StartQuest();
            //
            //acceptedQuests.Add(potentialQuests[index]);
            //potentialQuests.Remove(potentialQuests[index]);
            //QuestManager.instance.UpdateQuestText();
            Destroy(gameObject);
        }
        else if (Input.GetKeyDown(declineButton))
        {
            questBoardSource.potentialQuests.Add(questOffered);
            Destroy(gameObject);
        }
    }

    public void ShowOffer()
    {
        questTitleText.text = questOffered.questName;
        questDescriptionText.text = questOffered.questDescription;
        acceptButtonText.text = "(" + acceptButton.ToString() + ") Accept";
        declineButtonText.text = "(" + declineButton.ToString() + ") Decline"; 
    }
}

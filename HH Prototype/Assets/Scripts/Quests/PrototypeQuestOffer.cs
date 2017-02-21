﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrototypeQuestOffer : MonoBehaviour
{
    public QuestPrototype questOffered;
    public PrototypeObjectiveBoard questBoardSource;
    public KeyCode acceptButton = KeyCode.Z;
    public KeyCode declineButton = KeyCode.X;

    public Text questTitleText;
    public Text questDescriptionText;
    public Text acceptButtonText;
    public Text declineButtonText;

    public int questBoardIndex;

    void Update()
    {
        if (Input.GetKeyDown(acceptButton))
        {
            //PrototypeQuestManager.instance.activeQuests.Add(questOffered);
            questBoardSource.acceptedQuests.Add(questOffered);
            questOffered.StartQuest();
            //PrototypeQuestManager.instance.UpdateQuestText();
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

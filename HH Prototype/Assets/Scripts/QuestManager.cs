using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[CreateAssetMenu(fileName = "Data", menuName = "Quests", order = 1)]
public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    public int activeQuestIndex = 0;
    public List<Quest> activeQuests = new List<Quest>();
    public List<Quest> completedQuests = new List<Quest>();
    

    public Quest startingQuest;
    public Text questDescriptionText;

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
        if (startingQuest != null)
        {
            Quest startQuest = Quest.CreateInstance<Quest>();
            startQuest = startingQuest;
            activeQuests.Add(Instantiate(startQuest));
            Debug.Log("Quest Description: " + startQuest.questDescription);
            activeQuests[0].StartQuest();
        }
        UpdateQuestText();
    }
	
	// Update is called once per frame
	void Update ()
    {
<<<<<<< HEAD

=======
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Quest[0] current objective = " + activeQuests[0].currentObjective);
        }
>>>>>>> parent of 907bae1... Added Sell chest, deleted resource script, made reference to resource script use item script instead, added option to accept/decline quests
        if (Input.GetKeyDown(KeyCode.J))
        {
            ChangeActiveQuest();
        }
    }

    public static void UpdateQuests()
    {
        List<Quest> newCompletedQuests = new List<Quest>();
        foreach (Quest quest in instance.activeQuests)
        {
            //If current objective is done
            if (quest.objectives[quest.currentObjective].objectiveDone)
            {
                quest.NextObjective();
            }

            //Skip completed quests
            if (quest.questComplete)
            {
                Debug.Log("Inside if (quest.questcomplete");
                instance.completedQuests.Add(quest);
                // instance.activeQuests.Remove(quest);
                //completedQuests.Add(quest);
                //instance.activeQuestIndex--;
                newCompletedQuests.Add(quest);
                continue;
            }
        }
        foreach (Quest quest in newCompletedQuests)
        {
            instance.activeQuests.Remove(quest);
            instance.ChangeActiveQuest();
        }


        instance.UpdateQuestText();
    }    

    public void UpdateQuestText()
    {
        if (questDescriptionText == null)
            return;        

        if (activeQuests.Count > 0)
        {
            questDescriptionText.text = "Quest: " + activeQuests[activeQuestIndex].questName + "\n"
                + "Objective: " + activeQuests[activeQuestIndex].objectives[activeQuests[activeQuestIndex].currentObjective].objectiveDescription;
        }
        else
            questDescriptionText.text = "No objective.";
    }

    public void ChangeActiveQuest()
    {
        if (activeQuests.Count == 0)
        {
            return;
        }

        if (activeQuestIndex >= activeQuests.Count - 1)
            activeQuestIndex = 0;
        else
            activeQuestIndex++;

        UpdateQuestText();
    }
}

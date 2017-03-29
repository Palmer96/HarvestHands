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
    void Start ()
    {
        SaveAndLoadManager.OnSave += Save;
    }

    public virtual void Save()
    {
        SaveAndLoadManager.instance.saveData.noticeBoardSaveList.Add(new NoticeBoardSave(this));
        //Debug.Log("Saved item = " + name);
    }

    void OnDestroy()
    {
        SaveAndLoadManager.OnSave -= Save;
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
        questOfferObject.transform.localPosition = new Vector3(0, 0, 0);
        questOffer.ShowOffer();       
        potentialQuests.Remove(potentialQuests[index]);
        
        PrototypeQuestManager.instance.UpdateQuestText();

        if (potentialQuests.Count == 0)
            transform.GetChild(0).gameObject.SetActive(false);
        else
            transform.GetChild(0).gameObject.SetActive(true);
    }
}

[System.Serializable]
public class NoticeBoardSave
{    
    List<string> potentialQuests;
    List<string> acceptedQuests;
    float posX;
    float posY;
    float posZ;
    float rotX;
    float rotY;
    float rotZ;
    float rotW;

    public NoticeBoardSave(PrototypeObjectiveBoard board)
    {
        potentialQuests = new List<string>();
        acceptedQuests = new List<string>();
        posX = board.transform.position.x;
        posY = board.transform.position.y;
        posZ = board.transform.position.z;
        rotX = board.transform.rotation.x;
        rotY = board.transform.rotation.y;
        rotZ = board.transform.rotation.z;
        rotW = board.transform.rotation.w;
        foreach (QuestPrototype quest in board.acceptedQuests)
        {
            acceptedQuests.Add(quest.questName);
        }
        foreach (QuestPrototype quest in board.potentialQuests)
        {
            potentialQuests.Add(quest.questName);
        }
    }

    public GameObject LoadObject()
    {
        GameObject newNoticeBoard = (GameObject)Object.Instantiate(SaveAndLoadManager.instance.instantiateableQuestBoard, new Vector3(posX, posY, posZ), new Quaternion(rotX, rotY, rotZ, rotW));
        PrototypeObjectiveBoard objectiveBoard = newNoticeBoard.GetComponent<PrototypeObjectiveBoard>();
        objectiveBoard.acceptedQuests = new List<QuestPrototype>();
        objectiveBoard.potentialQuests = new List<QuestPrototype>();
        foreach(string questName in acceptedQuests)
        {
            foreach(QuestPrototype quest in QuestGrabber.questList)
            {
                if (questName == quest.questName)
                {
                    objectiveBoard.acceptedQuests.Add(quest);
                }
            }
        }
        foreach (string questName in potentialQuests)
        {
            foreach (QuestPrototype quest in QuestGrabber.questList)
            {
                if (questName == quest.questName)
                {
                    objectiveBoard.potentialQuests.Add(quest);
                }
            }
        }

        return newNoticeBoard;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{    
    public static List<NPC> npcList = new List<NPC>();
    public string npcName = "xXxPussySlayer69xXx";
    public int arousalValue = 0;

    public List<QuestPrototype> questPool = new List<QuestPrototype>();
    public List<QuestPrototype> acceptableQuests = new List<QuestPrototype>();

	// Use this for initialization
	void Start () {
        npcList.Add(this);
        SaveAndLoadManager.OnSave += Save;
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

    public void CheckForNewPotentialQuests()
    {
        if (questPool.Count < 1)
            return;

        for (int i = questPool.Count; i > 0; --i)
        {
            if (questPool[i-1].CheckPrerequisitesMet())
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
        //PrototypeQuestManager.instance.activeQuests.Add(quest);
        acceptableQuests.RemoveAt(index);
        return quest;
    }

    public virtual void Save()
    {
        SaveAndLoadManager.instance.npcSaveList.Add(new NPCSave(this));
    }

    void OnDestroy()
    {
        SaveAndLoadManager.OnSave -= Save;
    }
}

[System.Serializable]
public class NPCSave
{
    string npcName;
    int arousalValue;
    int videOverrideNode;
    float posX;
    float posY;
    float posZ;
    float rotX;
    float rotY;
    float rotZ;
    //List of questpool quests
    //List of acceptable quests //or just search questppol for accepted ones instead of saving two lists

    public NPCSave(NPC npc)
    {
        npcName = npc.npcName;
        arousalValue = npc.arousalValue;
        videOverrideNode = npc.GetComponent<VIDE_Assign>().overrideStartNode;
        posX = npc.transform.position.x;
        posY = npc.transform.position.y;
        posZ = npc.transform.position.z;
        rotX = npc.transform.rotation.x;
        rotY = npc.transform.rotation.y;
        rotZ = npc.transform.rotation.z;

        //Quests and stuff
    }

    public GameObject LoadObject()
    {
        foreach (NPC npcPrefab in NPC.npcList)
        {           
            if (npcPrefab.npcName == npcName)
            {
                //Debug.Log("Loading Hammer");                
                npcPrefab.arousalValue = arousalValue;
                npcPrefab.GetComponent<VIDE_Assign>().overrideStartNode = videOverrideNode;
                npcPrefab.transform.position = new Vector3(posX, posY, posZ);
                npcPrefab.transform.rotation = new Quaternion(rotX, rotY, rotZ, 0);

                //Quest stuff
                return npcPrefab.gameObject;
            }
        }
        Debug.Log("Failed to load NPC, npcName = " + npcName.ToString());
        return null;
    }
}
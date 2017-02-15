using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public string npcName = "xXxPussySlayer69xXx";
    public int arousalValue = 0;

	// Use this for initialization
	void Start () {
		
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
}

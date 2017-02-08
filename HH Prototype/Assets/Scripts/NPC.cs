using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public string npcName = "xXxPussySlayer69xXx";

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
}

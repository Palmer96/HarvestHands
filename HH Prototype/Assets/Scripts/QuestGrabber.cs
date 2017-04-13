using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGrabber : MonoBehaviour
{

    public static List<QuestPrototype> questList = new List<QuestPrototype>();

	// Use this for initialization
	void Start ()
    {
		foreach (QuestPrototype quest in GetComponentsInChildren<QuestPrototype>())
        {
            questList.Add(quest);
        }
	}

    //public void UpdatePrerequisies()
    //{
    //    foreach (QuestPrototype quest in questList)
    //    {
    //        quest.CheckPrerequisitesMet();
    //    }
    //}
}

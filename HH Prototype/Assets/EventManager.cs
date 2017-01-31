using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{

    public delegate void HarvestAction(string plantType);
    public static event HarvestAction OnHarvest = delegate { };

    public delegate void TalkAction(string npcName);
    public static event TalkAction OnTalk = delegate { };

    public delegate void SellAction(string npcName);
    public static event SellAction OnSell = delegate { };
    
	// Use this for initialization
	void Start ()
    {
        
	}
	
    public static void HarvestPlant(string plantType)
    {
        Debug.Log("Harvesting " + plantType);
        OnHarvest(plantType);
    }

	public static void TalkTo(string NPCName)
    {
        Debug.Log("Talking to " + NPCName);
        OnTalk(NPCName);
    }

    public static void SellTo(string name = "")
    {
        Debug.Log("Selling to " + name);
        OnSell(name);
    }
}

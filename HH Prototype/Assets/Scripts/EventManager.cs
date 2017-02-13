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

    public delegate void PlantAction(string plantName);
    public static event PlantAction OnPlant = delegate { };

    public delegate void ConstructAction(string buildingName);
    public static event ConstructAction OnConstruct = delegate { };

    // Use this for initialization
    void Start ()
    {
        
	}
	
    public static void HarvestEvent(string plantType)
    {
        Debug.Log("Harvesting " + plantType);
        OnHarvest(plantType);
    }

	public static void TalkEvent(string NPCName)
    {
        Debug.Log("Talking to " + NPCName);
        OnTalk(NPCName);
    }

    public static void SellEvent(string name = "")
    {
      //  Debug.Log("Selling to " + name);
        OnSell(name);
    }

    public static void PlantEvent(string name = "")
    {
        Debug.Log("Planting to " + name);
        OnPlant(name);
    }

    public static void ConstructEvent(string name = "")
    {
        Debug.Log("Constructed " + name);
        OnConstruct(name);
    }
}

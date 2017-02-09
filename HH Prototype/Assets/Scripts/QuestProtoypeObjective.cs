using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QuestProtoypeObjective : MonoBehaviour
{
    [System.Serializable]
    public enum objectiveType
    {
        NONE = -1,
        Talk = 0,
        Plant = 1,
        Harvest = 2,
        Sell = 3,    
        Buy = 4,
        Construct = 5,
    }
    public objectiveType type = objectiveType.NONE;
    public bool objectiveDone = false;
    public string objectiveDescription = "";


    public virtual void ActivateObjective(){}

    public virtual void DectivateObjective(){}

    public void CompleteObjective()
    {
        VirtualompleteObjective();
    }
    public virtual void VirtualompleteObjective()
    {
        objectiveDone = true;
        PrototypeQuestManager.UpdateQuests();
    }
}

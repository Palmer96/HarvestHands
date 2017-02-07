using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestProtoypeObjective : MonoBehaviour
{
    public enum objectiveType
    {
        NONE = -1,
        Talk = 0,
        Plant = 1,
        Harvest = 2,
        Sell = 3,    
    }
    public objectiveType type = objectiveType.NONE;
    public bool objectiveDone = false;
    public string objectiveDescription = "";


    public virtual void ActivateObjective(){}

    public virtual void DectivateObjective(){}

}

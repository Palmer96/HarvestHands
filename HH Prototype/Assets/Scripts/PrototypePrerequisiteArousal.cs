using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypePrerequisiteArousal : PrototypeQuestPrerequisite
{
    public NPC npc;
    public int arousalRequirement = 0;

    public override bool CheckPrerequisiteMet()
    {
        //Debug.Log("Inside overrided check prerequesites met");
        //Debug.Log((arousalRequirement >= npc.arousalValue).ToString() + "arousal requirements >= npc.arousal value " + arousalRequirement.ToString() + " >= " + npc.arousalValue.ToString());
        return (npc.arousalValue >= arousalRequirement);
    }
}

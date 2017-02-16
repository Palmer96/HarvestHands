using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypePrerequisiteQuest : PrototypeQuestPrerequisite
{
    public Quest quest;

    public override bool CheckPrerequisiteMet()
    {
        return quest.questComplete;
    }
}

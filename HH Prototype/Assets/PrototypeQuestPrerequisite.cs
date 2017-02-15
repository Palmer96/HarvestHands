using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeQuestPrerequisite : MonoBehaviour
{  
    public virtual bool CheckPrerequisiteMet()
    {
        return true;
    }
}

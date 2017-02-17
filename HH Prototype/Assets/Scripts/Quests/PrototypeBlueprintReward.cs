using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeBlueprintReward : PrototypeQuestReward
{
    GameObject bluePrintReward = null;


    public virtual void GenerateReward()
    {
        if (rewardGiven)
            return;

        rewardGiven = true;
        if (bluePrintReward != null)
            PlayerInventory.instance.book.GetComponent<Blueprint>().Constructs.Add(bluePrintReward);
    }
}

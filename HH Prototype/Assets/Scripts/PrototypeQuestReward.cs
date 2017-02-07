using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeQuestReward : MonoBehaviour
{
    public int money = 0;
    public bool rewardGiven = false;

	public void GenerateReward()
    {
        if (rewardGiven)
            return;

        rewardGiven = true;
        PlayerInventory.instance.money += money;
    }
}

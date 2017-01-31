using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestReward : ScriptableObject
{
    public virtual void GiveReward()
    {

    }
}

[CreateAssetMenu(fileName = "Data", menuName = "Quest/Reward/Money", order = 1)]
public class MoneyReward : QuestReward
{
    public int amount = 0;

    public override void GiveReward()
    {
        
    }
}

[CreateAssetMenu(fileName = "Data", menuName = "Quest/Reward/Item", order = 2)]
public class ItemReward : QuestReward
{
    public GameObject reward;
    public float spawnX = 0, spawnY = 0, spawnZ = 0;

    public override void GiveReward()
    {
        Instantiate(reward, new Vector3(spawnX, spawnY, spawnZ), Quaternion.identity);
    }
}
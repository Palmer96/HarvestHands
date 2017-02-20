using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeQuestReward : MonoBehaviour
{
    [System.Serializable]
    public struct ItemRewardStruct
    {
        public int quantity;
        public GameObject item;
    }
    public bool rewardGiven = false;

    public int money = 0;
    public List<ItemRewardStruct> itemRewards = new List<ItemRewardStruct>();
    public List<GameObject> recipeRewards = new List<GameObject>();
    public List<GameObject> blueprintRewards = new List<GameObject>();

	public virtual void GenerateReward()
    {
        if (rewardGiven)
            return;

        rewardGiven = true;
        PlayerInventory.instance.money += money;
        Debug.Log("Inside generate rewards");
        foreach (ItemRewardStruct reward in itemRewards)
        {
            Debug.Log("insside itemreward structs");
            if (reward.item.GetComponent<Item>() == null)
                continue;
            GameObject newObject = Instantiate(reward.item);
            newObject.GetComponent<Item>().quantity = reward.quantity;
            PlayerInventory.instance.AddItem(newObject);
            Debug.Log("Inside itemereward structs finsih");
        }
        foreach (GameObject reward in recipeRewards)
        {
            if (reward.GetComponent<CraftingRecipe>() == null)
                continue;
            CraftingManager.instance.knownRecipes.Add(reward.GetComponent<CraftingRecipe>());
        }
        foreach (GameObject reward in blueprintRewards)
        {
            PlayerInventory.instance.book.GetComponent<Blueprint>().Constructs.Add(reward);
        }
    }
}

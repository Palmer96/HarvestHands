using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellChest : MonoBehaviour
{
    public static List<SellChest> sellChests = new List<SellChest>();
    public int ID;
    public int valueOfItems = 0;

    // Use this for initialization
    void Start()
    {
        SellChest.sellChests.Add(this);
        SaveAndLoadManager.OnSave += Save;
    }

 // void Update()
 // {
 //     transform.GetChild(0).GetComponent<TextMesh>().text = "Todays Profit: $" + valueOfItems.ToString();
 // }

    //  void OnCollisionEnter(Collision col)
    //  {
    //  if (col.transform.CompareTag("Item") || col.transform.CompareTag("Rabbit"))
    //  {
    //      AddToSell(col.gameObject);
    //  }
    //  }

    public void AddToSell(GameObject sold)
    {
        Debug.Log("0");
        if (sold.GetComponent<Item>() != null)
        {
            Debug.Log("1");
            Item item = sold.GetComponent<Item>();
            if (item.sellable == true)
            {
                Debug.Log("2");
                if (item.GetComponent<Item>())
                    EventManager.SellEvent(item.GetComponent<Item>().itemName);
                //Debug.Log(col.transform.name);
                valueOfItems += item.value * item.quantity;
                PlayerInventory.instance.money += item.value * item.quantity;
                Destroy(sold);
            }
        }
    }
    public void AddSingleToSell(GameObject sold)
    {
        Debug.Log("00");
        if (sold.GetComponent<Item>() != null)
        {
            Debug.Log("3");
            Item item = sold.GetComponent<Item>();
            if (item.sellable == true)
            {
                Debug.Log("4");
                if (item.GetComponent<Item>())
                    EventManager.SellEvent(item.GetComponent<Item>().itemName);
                //Debug.Log(col.transform.name);
                if (item.quantity == 1)
                    valueOfItems += item.value * item.quantity;
                PlayerInventory.instance.money += item.value * item.quantity;
                Destroy(sold);
            }
        }
    }

    public static void SellAllChests()
    {
        foreach (SellChest chest in sellChests)
        {
            //  PlayerInventory.instance.money += chest.valueOfItems;
            chest.valueOfItems = 0;
        }
    }

    public virtual void Save()
    {
        SaveAndLoadManager.instance.saveData.sellChestSaveList.Add(new SellChestSave(this));
        //Debug.Log("Saved item = " + name);
    }

    void OnDestroy()
    {
        SaveAndLoadManager.OnSave -= Save;
    }
}

[System.Serializable]
public class SellChestSave
{
    int ID;
    int sellValue;

    public SellChestSave(SellChest sellChest)
    {
        sellValue = sellChest.valueOfItems;
    }

    public GameObject LoadObject()
    {
        foreach (SellChest sellChest in SellChest.sellChests)
        {
            if (ID == sellChest.ID)
            {
                sellChest.valueOfItems = sellValue;
                return sellChest.gameObject;
            }
        }
        Debug.Log("Failed to load SellChest, ID = " + ID.ToString() + ", sellValue = " + sellValue.ToString());
        return null;
    }
}
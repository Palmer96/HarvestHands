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

    void Update()
    {
        transform.GetChild(0).GetComponent<TextMesh>().text = "$" + valueOfItems.ToString();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.transform.CompareTag("Item") || col.transform.CompareTag("Rabbit"))
        {
            AddToSell(col.gameObject);
        }

    }

    public void AddToSell(GameObject sold)
    {
        if (sold.GetComponent<Item>() != null)
        {
            Item item = sold.GetComponent<Item>();
            if (item.sellable == true)
            {
                if (item.GetComponent<Item>())
                    EventManager.SellEvent(item.GetComponent<Item>().itemName);
                //Debug.Log(col.transform.name);
                valueOfItems += item.value * item.quantity;
                Destroy(sold);
            }
        }
    }
    public void AddSingleToSell(GameObject sold)
    {
        if (sold.GetComponent<Item>() != null)
        {
            Item item = sold.GetComponent<Item>();
            if (item.sellable == true)
            {
                if (item.GetComponent<Item>())
                    EventManager.SellEvent(item.GetComponent<Item>().itemName);
                //Debug.Log(col.transform.name);
                if (item.quantity == 1)
                valueOfItems += item.value * item.quantity;
                Destroy(sold);
            }
        }
    }

    public static void SellAllChests()
    {
        foreach (SellChest chest in sellChests)
        {
            PlayerInventory.instance.money += chest.valueOfItems;
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
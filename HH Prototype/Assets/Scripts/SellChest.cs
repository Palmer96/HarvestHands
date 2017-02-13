﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellChest : MonoBehaviour
{
    public static List<SellChest> sellChests = new List<SellChest>();
    public int valueOfItems = 0;

    // Use this for initialization
    void Start()
    {
        SellChest.sellChests.Add(this);
    }

    void Update()
    {
        transform.GetChild(0).GetComponent<TextMesh>().text = "$" + valueOfItems.ToString();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Item") || col.CompareTag("Rabbit"))
        {
            Item item = col.GetComponent<Item>();
            if (item.sellable == true)
            {
                if (item.GetComponent<Item>())
                    EventManager.SellEvent(item.GetComponent<Item>().itemName);
                Debug.Log(col.name);
                valueOfItems += item.value * item.quantity;
                Destroy(col.gameObject);
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
}

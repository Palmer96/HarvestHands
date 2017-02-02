using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int itemID;
    public string itemName;
    public int quantity;
    public int value;
    public bool sellable;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void IncreaseQuantity()
    {
        quantity++;
    }

    public void IncreaseQuantity(int amount)
    {
        quantity += amount;
    }

    public void DecreaseQuantity()
    {
        quantity--;
    }

    public void DecreaseQuantity(int amount)
    {
        quantity -= amount;
    }
}

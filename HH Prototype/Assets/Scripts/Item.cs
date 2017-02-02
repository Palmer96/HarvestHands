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

    public virtual void IncreaseQuantity()
    {
        quantity++;
    }
    public virtual void IncreaseQuantity(int amount)
    {
        quantity += amount;
    }
    public virtual void DecreaseQuantity()
    {
        quantity--;
    }

    public virtual void DecreaseQuantity(int amount)
    {
        quantity -= amount;
    }
}

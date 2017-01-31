using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance = null;
    public int money = 0;

    public List<GameObject> heldTools = new List<GameObject>();

    public List<GameObject> heldObjects = new List<GameObject>();

    public List<Sprite> Images = new List<Sprite>();


	// Use this for initialization
	void Start ()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

                
    }
	
	// Update is called once per frame
	void Update ()
    {
        
	}


    public void ActivateItem(int number)
    {

    }


    public bool AddTool(GameObject item)
    {
        for(int i = 0; i < heldObjects.Count; i++)
        {
            if (heldObjects[i] == null)
            {
                heldObjects[i] = item;
                return true;
            }
        }
        return false;

    }

    public void RemoveTool(GameObject item)
    {
        for (int i = 0; i < heldObjects.Count; i++)
        {
            if (heldObjects[i] == item)
            {
                heldObjects[i] = null;
            }
        }
    }

    public void RemoveTool(int location)
    {
        heldObjects[location] = null;
    }

    public bool AddItem(GameObject item)
    {
        for (int i = 0; i < heldObjects.Count; i++)
        {
            if (heldObjects[i] == null)
            {
                heldObjects[i] = item;
                return true;
            }
        }
        return false;

    }

    public void RemoveItem(GameObject item)
    {
        for (int i = 0; i < heldObjects.Count; i++)
        {
            if (heldObjects[i] == item)
            {
                heldObjects[i] = null;
            }
        }
    }

    public void RemoveItem(int location)
    {
        heldObjects[location] = null;
    }



    void UpdateInventory()
    {
        for (int i = 0; i < heldObjects.Count; i++)
        {
            if (heldObjects[i] != null)
            {
                
            }
        }
    }
}

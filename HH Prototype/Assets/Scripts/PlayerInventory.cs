using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance = null;
    public int money = 0;

    public List<GameObject> heldObjects = new List<GameObject>();
        

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
}

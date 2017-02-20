using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivestockManager : MonoBehaviour
{
    public static LivestockManager instance = null;
    public List<Livestock> livestockList = new List<Livestock>();

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

    public void AddLivestock(Livestock livestock)
    {
        livestockList.Add(livestock);
    }
}

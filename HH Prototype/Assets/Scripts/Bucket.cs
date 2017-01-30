using UnityEngine;
using System.Collections;

public class Bucket : MonoBehaviour
{
    public int currentWaterLevel = 10;
    public int maxWaterLevel = 10;
    public int waterDrain = 1;



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    void OnTriggerEnter(Collider col)
    {        
        if (col.gameObject.CompareTag("Plant"))
        {
            if(currentWaterLevel > 0)
            {
                Plant plant = col.GetComponent<Plant>();
                if (!plant.isWatered)
                {
                    currentWaterLevel -= waterDrain;
                    plant.WaterPlant();
                }
            }
        }
        else if (col.gameObject.CompareTag("WaterSource"))
        {
            currentWaterLevel = maxWaterLevel;
        }
    }
}

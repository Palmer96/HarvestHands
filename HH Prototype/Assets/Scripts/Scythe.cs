using UnityEngine;
using System.Collections;

public class Scythe : Tool {

	// Use this for initialization
	void Start () {
        toolID = 4;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void UseTool()
    {
        base.UseTool();

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Plant"))
        {
            Plant plant = col.gameObject.GetComponent<Plant>();
            if (plant.readyToHarvest)
            {
                plant.HarvestPlant();
            }
        }
    }
}

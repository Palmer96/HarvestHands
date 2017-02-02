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
        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

        Debug.Log("Scythe");
        if (Physics.Raycast(ray, out hit, rayMaxDist))
        {
            if (hit.transform.CompareTag("Plant"))
            {
                hit.transform.GetComponent<Plant>().HarvestPlant();
            }
        }

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

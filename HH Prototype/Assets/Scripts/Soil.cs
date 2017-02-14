using UnityEngine;
using System.Collections;

public class Soil : MonoBehaviour
{
    public bool occupied = false;
	// Use this for initialization
	void Start ()
    {
        PlantManager.instance.AddSoil(this);
	}
	
	public void PlantSeed(GameObject plantPrefab)
    {
        if (occupied)
            return;

        GameObject newPlant = Instantiate(plantPrefab);
        newPlant.transform.position = transform.position;
        newPlant.transform.parent = gameObject.transform;

        Plant plant = newPlant.GetComponent<Plant>();
        plant.dayPlanted = DayNightController.instance.ingameDay;
        plant.soil = this;

        occupied = true;

        WaveManager.instance.plantsLeft++;
    }
}

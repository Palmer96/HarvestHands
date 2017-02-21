using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlantManager : MonoBehaviour
{
    public static PlantManager instance = null;
    public List<Plant> plantList = new List<Plant>();
    public List<Soil> soilList = new List<Soil>();

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

    public void AddPlant(Plant plant)
    {
        plantList.Add(plant);
    }

    public void AddSoil(Soil soil)
    {
        soilList.Add(soil);
    }

    public void UpdatePlants(float time)
    {
        foreach (Plant plant in plantList)
        {
            if (plant == null)
                continue;

            plant.TimeJump(time);
            
        }
    }

    public void WaterPlants()
    {
        foreach (Plant plant in plantList)
        {
            if (plant == null)
                continue;

          //  plant.WaterPlant();

        }
    }
}

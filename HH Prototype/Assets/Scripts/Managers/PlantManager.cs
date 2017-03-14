using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlantManager : MonoBehaviour
{
    public static PlantManager instance = null;
    public List<Plant> plantList = new List<Plant>();
    public List<Soil> soilList = new List<Soil>();
    public List<Plot> plotList = new List<Plot>();

    public float rainWater = 0.01f;

    public bool isRaining = false;
    public GameObject WeedPrefab;
    public float weedSpawnChance = 5f;


    // Use this for initialization
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (isRaining)
        {
            foreach (Plant plant in plantList)
            {
                if (plant == null)
                    continue;
                plant.RainPlant(rainWater);
            }
        }

        //if (Input.GetKeyDown(KeyCode.O))
        //{
        //    GlobalWeedSpread();
        //}
    }

    public void AddPlant(Plant plant)
    {
        plantList.Add(plant);
    }

    public void AddSoil(Soil soil)
    {
        soilList.Add(soil);
    }

    public void RemoveNulls()
    {
        List<Plant> tempPlantList = new List<Plant>();
        foreach (Plant plant in plantList)
        {
            if (plant != null)
                tempPlantList.Add(plant);
        }
        List<Soil> tempSoilList = new List<Soil>();
        foreach (Soil soil in soilList)
        {
            if (soil != null)
                tempSoilList.Add(soil);
        }
        List<Plot> tempPlotList = new List<Plot>();
        foreach (Plot plot in plotList)
        {
            if (plot != null)
                tempPlotList.Add(plot);
        }
        
        plantList = tempPlantList;
        soilList = tempSoilList;
        plotList = tempPlotList;
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
    public void Raining(bool rain)
    {
        isRaining = rain;
    }

    public void GlobalWeedSpread()
    {
        Debug.Log("inside weed spread");
        if (WeedPrefab == null)
            return;

        for (int i = 0; i < soilList.Count; ++i)
        {
            Debug.Log("inside soil list count thing");
            float spawnChance = Random.Range(0, 100);
            if (spawnChance < weedSpawnChance)
            {
                Debug.Log("inside weed instantiation");
                GameObject newWeed = Instantiate(WeedPrefab);
                newWeed.GetComponent<Weed>().InfestSoil(soilList[i]);                
            }
        }
    }
}

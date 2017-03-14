using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weed : MonoBehaviour
{
    public Soil soil;
    public Plant plant;

    [Tooltip("Infested plant will dry at this rate (1.0 is normal, 2.0 is twice as fast)")]
    public float dryMultiplier = 2;

    [Header("Weed Spread")]
    public float timeToSpread;
    public float currentSpreadTimer;

    // Use this for initialization
    void Start()
    {
        currentSpreadTimer = timeToSpread;
    }

    // Update is called once per frame
    void Update()
    {
        currentSpreadTimer -= DayNightController.instance.timePast;
        if (currentSpreadTimer <= 0)
        {
            Spread();
            currentSpreadTimer += timeToSpread;
        }
    }

    public void InfestSoil(Soil soilPlot)
    {
        soil = soilPlot;
        soil.weedInfestation = this;
        soil.occupied = true;

        //Check for plant
        plant = soil.GetComponentInChildren<Plant>();
        if (plant != null)
            plant.dryMultiplier *= dryMultiplier;

        transform.SetParent(soil.transform);
        transform.localPosition = new Vector3(0, 0, 0);
    }

    public void RemoveWeed()
    {
        if (plant != null)
            plant.dryMultiplier /= dryMultiplier;
        soil.weedInfestation = null;
        if (plant == null)
            soil.occupied = false;
        Destroy(gameObject);
    }

    public void Spread()
    {
        Plot plot = soil.GetComponentInParent<Plot>();
        if (plot == null)
            return;

        if (plot.soilList.Count <= 1)
            return;

        for (int i = 0; i < plot.soilList.Count; ++i)
        {
            if (plot.soilList[i] == soil)
            {
                GameObject newWeedObject = Instantiate(gameObject);
                newWeedObject.transform.localScale = new Vector3(1, 1, 1);
                Weed newWeed = newWeedObject.GetComponent<Weed>();
                newWeed.soil = null;
                newWeed.plant = null;
                newWeed.currentSpreadTimer = timeToSpread;

                //This only really works for the line soils

                //If first soil
                if (i == 0)
                {
                    newWeed.InfestSoil(plot.soilList[i + 1]);
                    break;
                }
                //If last soil
                else if (i == plot.soilList.Count - 1)
                {
                    newWeed.InfestSoil(plot.soilList[i - 1]);
                    break;
                }
                //If somewhere in the middle
                else
                {
                    bool testBeforeFirst = (Random.value > 0.5f);
                    Debug.Log("testBeforeFirst = " + testBeforeFirst);
                    if (testBeforeFirst)
                    {
                        if (plot.soilList[i - 1].GetComponent<Soil>().weedInfestation == null)
                        {
                            newWeed.InfestSoil(plot.soilList[i - 1]);
                            break;
                        }
                        else if (plot.soilList[i + 1].GetComponent<Soil>().weedInfestation == null)
                        {
                            newWeed.InfestSoil(plot.soilList[i + 1]);
                            break;
                        }
                    }
                    else
                    {
                        if (plot.soilList[i + 1].GetComponent<Soil>().weedInfestation == null)
                        {
                            newWeed.InfestSoil(plot.soilList[i + 1]);
                            break;
                        }
                        else if (plot.soilList[i - 1].GetComponent<Soil>().weedInfestation == null)
                        {
                            newWeed.InfestSoil(plot.soilList[i - 1]);
                            break;
                        }
                    }
                }
                //Assuming all the spreads fail
                Destroy(newWeedObject);
                break;
            }
        }
    }
}

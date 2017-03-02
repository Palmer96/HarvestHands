using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : BuildingIdentifier
{
    public List<Soil> soilList = new List<Soil>();

	// Use this for initialization
	void Start ()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            soilList.Add(transform.GetChild(i).GetComponent<Soil>());
        }



        PlantManager.instance.plotList.Add(this);
        SaveAndLoadManager.OnSave += Save;
    }

    public override void Save()
    {
        SaveAndLoadManager.instance.plotSaveList.Add(new PlotSave(this));
        //Debug.Log("Saved item = " + name);
    }

    void OnDestroy()
    {
        SaveAndLoadManager.OnSave -= Save;
    }
}

[System.Serializable]
public class PlotSave
{
    int ID;
    List<SoilSave> soilList;
    float posX;
    float posY;
    float posZ;
    float rotX;
    float rotY;
    float rotZ;

    public PlotSave(Plot plot)
    {
        ID = plot.ID;
        soilList = new List<SoilSave>();
        foreach (Soil soil in plot.soilList)
        {
            soilList.Add(new SoilSave(soil));
        }
        posX = plot.transform.position.x;
        posY = plot.transform.position.y;
        posZ = plot.transform.position.z;
        rotX = plot.transform.rotation.x;
        rotY = plot.transform.rotation.y;
        rotZ = plot.transform.rotation.z;
    }

    public GameObject LoadObject()
    {
        foreach (GameObject plotPrefabType in SaveAndLoadManager.instance.instantiateablePlots)
        {
            Plot plotPrefab = plotPrefabType.GetComponent<Plot>();
            if (plotPrefab == null)
                continue;

            if (plotPrefab.ID == ID)
            {
                //Debug.Log("Loading Axe");
                GameObject plot = (GameObject)Object.Instantiate(plotPrefabType, new Vector3(posX, posY, posZ), new Quaternion(rotX, rotY, rotZ, 0));
                Plot newPlot = plot.GetComponent<Plot>();
                foreach(Soil soil in newPlot.soilList)
                {
                    Object.Destroy(soil.gameObject);
                }
                foreach (SoilSave soil in soilList)
                {
                    newPlot.soilList.Add(soil.LoadObject().GetComponent<Soil>());                    
                }
                foreach (Soil soil in newPlot.soilList)
                {
                    soil.transform.SetParent(newPlot.transform);
                }
                          
                return plot;
            }
        }
        Debug.Log("Failed to load Plot, ID = " + ID.ToString());
        return null;
    }
}
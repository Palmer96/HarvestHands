using UnityEngine;
using System.Collections;

public class Soil : MonoBehaviour
{
    public bool occupied = false;
	// Use this for initialization
	void Start ()
    {
        PlantManager.instance.AddSoil(this);
        //SaveAndLoadManager.OnSave += Save;
    }
	
	public void PlantSeed(GameObject plantPrefab)
    {
        if (occupied)
            return;

        GameObject newPlant = Instantiate(plantPrefab);
        newPlant.transform.position = transform.position;
        newPlant.transform.parent = gameObject.transform;

        Plant plant = newPlant.GetComponent<Plant>();
     //   plant.dayPlanted = DayNightController.instance.ingameDay;
        plant.soil = this;

        occupied = true;

      //  WaveManager.instance.plantsLeft++;
    }

    /* Plot Cover saving and load soil + plants
    public virtual void Save()
    {
        SaveAndLoadManager.instance.soilSaveList.Add(new BuildingSave(this));
        //Debug.Log("Saved item = " + name);
    }

    void OnDestroy()
    {
        SaveAndLoadManager.OnSave -= Save;
    }
    */
}

[System.Serializable]
public class SoilSave
{
    bool occupied;
    float posX;
    float posY;
    float posZ;
    float rotX;
    float rotY;
    float rotZ;
    PlantSave plantSave;

    public SoilSave(Soil soil)
    {        
        occupied = soil.occupied;
        if (soil.occupied)
        {
            plantSave = new PlantSave(soil.GetComponentInChildren<Plant>());
        }
        else
        {
            plantSave = null;
        }
        posX = soil.transform.position.x;
        posY = soil.transform.position.y;
        posZ = soil.transform.position.z;
        rotX = soil.transform.rotation.x;
        rotY = soil.transform.rotation.y;
        rotZ = soil.transform.rotation.z;
    }

    public GameObject LoadObject(Transform parent = null)
    {
        if (SaveAndLoadManager.instance.instantiateableSoil != null)
        {               
            //Debug.Log("Loading Axe");
            GameObject soil = (GameObject)Object.Instantiate(SaveAndLoadManager.instance.instantiateableSoil, new Vector3(posX, posY, posZ), new Quaternion(rotX, rotY, rotZ, 0));
            Soil newSoil = soil.GetComponent<Soil>();
            if (parent != null)
            {
                newSoil.transform.SetParent(parent);
                newSoil.transform.position = new Vector3(posX, posY, posZ);
                newSoil.transform.rotation = new Quaternion(rotX, rotY, rotZ, 0);
            }
            newSoil.occupied = occupied;
            if (occupied)
            {
                GameObject plant = plantSave.LoadObject(soil.transform);
            }
            return soil;               
        }
        Debug.Log("Failed to load Soil, is SaveAndLoadManager.instantiateableSoil set?");
        return null;
    }
}

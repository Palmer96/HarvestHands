using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingIdentifier : MonoBehaviour
{
    public int ID;

	// Use this for initialization
	void Start ()
    {
        SaveAndLoadManager.OnSave += Save;
    }

    public virtual void Save()
    {
        SaveAndLoadManager.instance.buildingSaveList.Add(new BuildingSave(this));
        //Debug.Log("Saved item = " + name);
    }

    void OnDestroy()
    {
        SaveAndLoadManager.OnSave -= Save;
    }
}

[System.Serializable]
public class BuildingSave
{
    int ID;
    float posX;
    float posY;
    float posZ;
    float rotX;
    float rotY;
    float rotZ;

    public BuildingSave(BuildingIdentifier building)
    {
        ID = building.ID;
        posX = building.transform.position.x;
        posY = building.transform.position.y;
        posZ = building.transform.position.z;
        rotX = building.transform.rotation.x;
        rotY = building.transform.rotation.y;
        rotZ = building.transform.rotation.z;
    }

    public GameObject LoadObject()
    {
        foreach (GameObject buildingPrefabType in SaveAndLoadManager.instance.instantiateableBuildings)
        {
            BuildingIdentifier buildingPrefab = buildingPrefabType.GetComponent<BuildingIdentifier>();
            if (buildingPrefab == null)
                continue;

            if (buildingPrefab.ID == ID)
            {
                //Debug.Log("Loading Axe");
                GameObject building = (GameObject)Object.Instantiate(buildingPrefabType, new Vector3(posX, posY, posZ), new Quaternion(rotX, rotY, rotZ, 0));
                return building;
            }
        }
        Debug.Log("Failed to load BuildingIdentifier, ID = " + ID.ToString());
        return null;
    }
}

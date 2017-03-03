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
        SaveAndLoadManager.instance.saveData.buildingIdentifierSaveList.Add(new BuildingIdentifierSave(this));
        //Debug.Log("Saved item = " + name);
    }

    void OnDestroy()
    {
        SaveAndLoadManager.OnSave -= Save;
    }
}

[System.Serializable]
public class BuildingIdentifierSave
{
    int ID;
    float posX;
    float posY;
    float posZ;
    float rotX;
    float rotY;
    float rotZ;
    float rotW;

    public BuildingIdentifierSave(BuildingIdentifier building)
    {
        ID = building.ID;
        posX = building.transform.position.x;
        posY = building.transform.position.y;
        posZ = building.transform.position.z;
        rotX = building.transform.rotation.x;
        rotY = building.transform.rotation.y;
        rotZ = building.transform.rotation.z;
        rotW = building.transform.rotation.w;
    }

    public GameObject LoadObject()
    {
        foreach (GameObject buildingPrefabType in SaveAndLoadManager.instance.instantiateableBuildingIdentifiers)
        {
            BuildingIdentifier buildingPrefab = buildingPrefabType.GetComponent<BuildingIdentifier>();
            if (buildingPrefab == null)
                continue;

            if (buildingPrefab.ID == ID)
            {
                //Debug.Log("Loading Axe");
                GameObject building = (GameObject)Object.Instantiate(buildingPrefabType, new Vector3(posX, posY, posZ), new Quaternion(rotX, rotY, rotZ, rotW));
                return building;
            }
        }
        Debug.Log("Failed to load BuildingIdentifier, ID = " + ID.ToString());
        return null;
    }
}

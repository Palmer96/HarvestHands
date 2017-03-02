using UnityEngine;
using System.Collections;

public class Sickle : Item
{
    public int level = 1;
    // Use this for initialization
    void Start()
    {
        itemID = 6;
        itemCap = 1;
        SaveAndLoadManager.OnSave += Save;
    }

    // Update is called once per frame
    void Update()
    {
        if (used)
        {
            useTimer -= Time.deltaTime;
            if (useTimer < 0)
            {
                used = false;
            }
        }
    }

    public override void PrimaryUse(ClickType click)
    {
        switch (click)
        {
            //       case ClickType.Single:
            //           ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
            //
            //           Debug.Log("Sickle");
            //           if (Physics.Raycast(ray, out hit, rayMaxDist))
            //           {
            //               if (hit.transform.CompareTag("Plant"))
            //               {
            //                   hit.transform.GetComponent<Plant>().HarvestPlant(level);
            //                   used = true;
            //                   useTimer = useRate;
            //               }
            //           }
            //           break;

            case ClickType.Hold:
                if (!used)
                {
                    ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

                    Debug.Log("Sickle");
                    if (Physics.Raycast(ray, out hit, rayMaxDist))
                    {
                        if (hit.transform.CompareTag("Plant"))
                        {
                            hit.transform.GetComponent<Plant>().HarvestPlant(level);
                            used = true;
                            useTimer = useRate;
                        }
                    }
                }
                break;
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
        if (col.gameObject.CompareTag("Soil"))
        {
            Plant plant = col.transform.GetChild(0).GetComponent<Plant>();
            if (plant.readyToHarvest)
            {
                plant.HarvestPlant();
            }
        }
    }

    public override void Save()
    {
        SaveAndLoadManager.instance.sickleSaveList.Add(new SickleSave(this));
    }

    void OnDestroy()
    {
        SaveAndLoadManager.OnSave -= Save;
    }
}

[System.Serializable]
public class SickleSave
{
    int level;
    float posX;
    float posY;
    float posZ;
    float rotX;
    float rotY;
    float rotZ;

    public SickleSave(Sickle sickle)
    {
        level = sickle.level;
        posX = sickle.transform.position.x;
        posY = sickle.transform.position.y;
        posZ = sickle.transform.position.z;
        rotX = sickle.transform.rotation.x;
        rotY = sickle.transform.rotation.y;
        rotZ = sickle.transform.rotation.z;
    }

    public GameObject LoadObject()
    {
        foreach (GameObject toolPrefab in SaveAndLoadManager.instance.instantiateableTools)
        {
            Sickle sicklePrefab = toolPrefab.GetComponent<Sickle>();
            if (sicklePrefab == null)
                continue;

            if (sicklePrefab.level == level)
            {
                //Debug.Log("Loading Axe");
                GameObject sickle = (GameObject)Object.Instantiate(toolPrefab, new Vector3(posX, posY, posZ), new Quaternion(rotX, rotY, rotZ, 0));
                return sickle;
            }
        }
        Debug.Log("Failed to load sickle, level = " + level.ToString());
        return null;
    }
}
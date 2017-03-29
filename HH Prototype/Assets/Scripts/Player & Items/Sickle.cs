using UnityEngine;
using System.Collections;

public class Sickle : Item
{
    public int level = 1;
    // Use this for initialization
    void Start()
    {
        startScale = transform.lossyScale;
        itemID = 6;
        itemCap = 1;
        MinimapManager.instance.CreateImage(transform, new Color(0.1f, 1f, 1.1f));
        SaveAndLoadManager.OnSave += Save;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveing)
        {
            if (moveBack)
                transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(1.6f, -0.8f, 2), 0.1f);
            else
                transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(1.6f, -0.8f, 2.5f), 0.2f);

            if (transform.localPosition.z > 2.4f)
            {
                moveBack = true;
                PrimaryUse();
            }
            if (moveBack)
            {
                if (transform.localPosition.z < 2.01f)
                {
                    transform.localPosition = new Vector3(1.6f, -0.8f, 2);
                    moveing = false;
                    moveBack = false;
                }
            }
        }
    }

    public override void Move()
    {
        base.Move();
    }

    public override void PrimaryUse()
    {
        {
            ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

            Debug.Log("Sickle");
            if (Physics.Raycast(ray, out hit, rayMaxDist))
            {
                Soil soil = null;

                if (hit.transform.CompareTag("Plant"))
                {
                    hit.transform.GetComponent<Plant>().HarvestPlant(level);
                    used = true;
                    useTimer = useRate;
                    soil = hit.transform.parent.GetComponent<Soil>();
                }

                else if (hit.transform.CompareTag("Soil"))
                {
                    if (hit.transform.childCount > 0)
                        hit.transform.GetChild(0).GetComponent<Plant>().HarvestPlant(level);
                    used = true;
                    useTimer = useRate;
                    soil = hit.transform.GetComponent<Soil>();
                }
                else
                    ScreenMessage.instance.CreateMessage("You cannot use " + itemName + " here");

                if (soil != null)
                    if (soil.weedInfestation != null)
                        soil.weedInfestation.RemoveWeed();
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
        SaveAndLoadManager.instance.saveData.sickleSaveList.Add(new SickleSave(this));
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
    float rotW;
    int inventorySlot;

    public SickleSave(Sickle sickle)
    {
        level = sickle.level;
        posX = sickle.transform.position.x;
        posY = sickle.transform.position.y;
        posZ = sickle.transform.position.z;
        rotX = sickle.transform.rotation.x;
        rotY = sickle.transform.rotation.y;
        rotZ = sickle.transform.rotation.z;
        rotW = sickle.transform.rotation.w;
        if (sickle.beingHeld)
        {
            for (int i = 0; i < PlayerInventory.instance.heldObjects.Count; ++i)
            {
                if (sickle.gameObject == PlayerInventory.instance.heldObjects[i])
                {
                    inventorySlot = i;
                }
            }
        }
        else
        {
            inventorySlot = -1;
        }
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
                GameObject sickle = (GameObject)Object.Instantiate(toolPrefab, new Vector3(posX, posY, posZ), new Quaternion(rotX, rotY, rotZ, rotW));
                if (inventorySlot != -1)
                {
                    PlayerInventory.instance.AddItemInSlot(sickle, inventorySlot);
                }
                return sickle;
            }
        }
        Debug.Log("Failed to load sickle, level = " + level.ToString());
        return null;
    }
}
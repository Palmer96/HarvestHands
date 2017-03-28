using UnityEngine;
using System.Collections;

public class Bucket : Item
{
    public int level = 1;
    public float currentWaterLevel = 10;
    public float maxWaterLevel = 10;
    public float waterDrain = 3;

    public GameObject waterDrop;


    // Use this for initialization
    void Start()
    {
        startScale = transform.lossyScale;
        itemID = 5;
        itemCap = 1;
        MinimapManager.instance.CreateImage(transform, new Color(0.1f, 1f, 1.1f));
        SaveAndLoadManager.OnSave += Save;
    }

    // Update is called once per frame
    void Update()
    {
        transform.GetChild(1).GetComponent<TextMesh>().text = currentWaterLevel.ToString();
        if (moveing)
        {
            if (moveBack)
                transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(1.6f, -0.8f, 2), 0.1f);
            //transform.localRotation = Quaternion.Lerp(transform.localRotation, rot.rotation, 0.1f);
            else
                transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(1.6f, -0.8f, 2.5f), 0.2f);
            // transform.localRotation = Quaternion.Lerp(transform.localRotation, startrot.rotation, 0.1f);

            if (transform.localPosition.z > 2.4f)
            {
                // moveBack = true;
                used = false;
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

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("WaterSource"))
        {
            currentWaterLevel = maxWaterLevel;
        }
    }


    public override void PrimaryUse()
    {
        if (!used)
        {
            if (currentWaterLevel > 0)
                currentWaterLevel -= waterDrain;
            else
                ScreenMessage.instance.CreateMessage("Your " + itemName + " is empty");

            ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

            if (Physics.Raycast(ray, out hit, rayMaxDist))
            {
                if (hit.transform.CompareTag("WaterSource"))
                    currentWaterLevel = maxWaterLevel;
                else if (hit.transform.CompareTag("Plant"))
                {
                    if (currentWaterLevel > 0)
                    {
                        if (hit.transform.GetComponent<Plant>().WaterPlant(waterDrain))
                        {
                            EventManager.WaterEvent(hit.transform.GetComponent<Plant>().plantName.ToString());
                            used = true;
                            useTimer = useRate;
                        }
                    }
                }
                else if (hit.transform.CompareTag("Soil"))
                {
                    if (currentWaterLevel > 0)
                    {
                        if (hit.transform.childCount > 0)
                        {

                            if (hit.transform.GetChild(0).GetComponent<Plant>().WaterPlant(waterDrain))
                            {
                                EventManager.WaterEvent(hit.transform.GetChild(0).GetComponent<Plant>().plantName.ToString());
                                used = true;
                                useTimer = useRate;
                            }
                        }
                    }
                }
            }
        }
    }


    //  public override void PrimaryUse()
    //  {
    //      if (!used)
    //      {
    //          ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
    //
    //          if (Physics.Raycast(ray, out hit, rayMaxDist))
    //          {
    //              if (hit.transform.CompareTag("WaterSource"))
    //                  currentWaterLevel = maxWaterLevel;
    //              else if (hit.transform.CompareTag("Plant"))
    //              {
    //                  if (currentWaterLevel > 0)
    //                  {
    //                      if (hit.transform.GetComponent<Plant>().WaterPlant(waterDrain))
    //                      {
    //                          currentWaterLevel -= waterDrain;
    //                          EventManager.WaterEvent(hit.transform.GetComponent<Plant>().plantName.ToString());
    //                          used = true;
    //                          useTimer = useRate;
    //                      }
    //                  }
    //              }
    //          }
    //      }
    //  }

    public override void Save()
    {
        SaveAndLoadManager.instance.saveData.bucketSaveList.Add(new BucketSave(this));
    }

    void OnDestroy()
    {
        SaveAndLoadManager.OnSave -= Save;
    }
}

[System.Serializable]
public class BucketSave
{
    int level;
    float currentWaterLevel;
    float posX;
    float posY;
    float posZ;
    float rotX;
    float rotY;
    float rotZ;
    float rotW;
    int inventorySlot;

    public BucketSave(Bucket bucket)
    {
        level = bucket.level;
        currentWaterLevel = bucket.currentWaterLevel;
        posX = bucket.transform.position.x;
        posY = bucket.transform.position.y;
        posZ = bucket.transform.position.z;
        rotX = bucket.transform.rotation.x;
        rotY = bucket.transform.rotation.y;
        rotZ = bucket.transform.rotation.z;
        rotW = bucket.transform.rotation.w;
        if (bucket.beingHeld)
        {
            for (int i = 0; i < PlayerInventory.instance.heldObjects.Count; ++i)
            {
                if (bucket.gameObject == PlayerInventory.instance.heldObjects[i])
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
            Bucket bucketPrefab = toolPrefab.GetComponent<Bucket>();
            if (bucketPrefab == null)
                continue;

            if (bucketPrefab.level == level)
            {
                //Debug.Log("Loading Bucket");
                GameObject bucket = (GameObject)Object.Instantiate(toolPrefab, new Vector3(posX, posY, posZ), new Quaternion(rotX, rotY, rotZ, rotW));
                bucket.GetComponent<Bucket>().currentWaterLevel = currentWaterLevel;
                if (inventorySlot != -1)
                {
                    PlayerInventory.instance.AddItemInSlot(bucket, inventorySlot);
                }
                return bucket;
            }
        }
        Debug.Log("Failed to load bucket, level = " + level.ToString());
        return null;
    }
}
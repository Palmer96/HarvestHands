using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shovel : Item
{
    public int level = 1;
    public GameObject dirt;

    public float size;
    public float depth;

    // Use this for initialization
    void Start()
    {
        startScale = transform.lossyScale;
        itemID = 4;
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
                    //   base.UseTool();
                    ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

                    Debug.Log("Shovel");
                    if (Physics.Raycast(ray, out hit, rayMaxDist))
                    {
                        if (hit.transform.CompareTag("Ground"))
                        {
                            used = true;
                            useTimer = useRate;
                            Instantiate(dirt, hit.point, transform.rotation);
                            if (level > 1)
                                Instantiate(dirt, hit.point + (transform.up * 1.5f), transform.rotation);
                            if (level > 2)
                                Instantiate(dirt, hit.point + (transform.up * 3), transform.rotation);
                        }
                        else
                            ScreenMessage.instance.CreateMessage("You cannot use " + itemName + " here");
                    }
                }

    }

    public override void Save()
    {
        SaveAndLoadManager.instance.saveData.shovelSaveList.Add(new ShovelSave(this));
    }

    void OnDestroy()
    {
        SaveAndLoadManager.OnSave -= Save;
    }
}


[System.Serializable]
public class ShovelSave
{
    int level;
    float posX;
    float posY;
    float posZ;
    float rotX;
    float rotY;
    float rotZ;
    float rotW;

    public ShovelSave(Shovel shovel)
    {
        level = shovel.level;
        posX = shovel.transform.position.x;
        posY = shovel.transform.position.y;
        posZ = shovel.transform.position.z;
        rotX = shovel.transform.rotation.x;
        rotY = shovel.transform.rotation.y;
        rotZ = shovel.transform.rotation.z;
        rotW = shovel.transform.rotation.z;
    }

    public GameObject LoadObject()
    {
        foreach (GameObject toolPrefab in SaveAndLoadManager.instance.instantiateableTools)
        {
            Shovel shovelPrefab = toolPrefab.GetComponent<Shovel>();
            if (shovelPrefab == null)
                continue;

            if (shovelPrefab.level == level)
            {
                //Debug.Log("Loading Shovel");
                GameObject shovel = (GameObject)Object.Instantiate(toolPrefab, new Vector3(posX, posY, posZ), new Quaternion(rotX, rotY, rotZ, rotW));
                return shovel;
            }
        }
        Debug.Log("Failed to load shovel, level = " + level.ToString());
        return null;
    }
}
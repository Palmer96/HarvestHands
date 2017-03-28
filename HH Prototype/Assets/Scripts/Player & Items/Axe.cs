using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Item
{

    public int level = 1;
    public GameObject wood;

    // Use this for initialization
    void Start()
    {
        startScale = transform.lossyScale;
        itemID = 3;
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

                //   if (!used)
                {
                    ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

                    //Debug.Log("Axe");
                    if (Physics.Raycast(ray, out hit, rayMaxDist))
                    {
                        if (hit.transform.CompareTag("Tree"))
                        {
                            used = true;
                            useTimer = useRate;
                            hit.transform.GetComponent<Tree>().Harvest();
                            if (level > 1)
                                hit.transform.GetComponent<Tree>().Harvest();
                            if (level > 2)
                                hit.transform.GetComponent<Tree>().Harvest();
                            //    Instantiate(wood, hit.point, transform.rotation);
                        }
                        else if (hit.transform.CompareTag("Soil") || hit.transform.CompareTag("Plant"))
                        {
                            used = true;
                            useTimer = useRate;
                            //Get soil
                            Soil soil;
                            if (hit.transform.CompareTag("Soil"))
                                soil = hit.transform.GetComponent<Soil>();
                            else
                                soil = hit.transform.parent.GetComponent<Soil>();
                            //Remove Weed
                            if (soil != null)
                                if (soil.weedInfestation != null)
                                    soil.weedInfestation.RemoveWeed();                            
                        }
                        else
                            ScreenMessage.instance.CreateMessage("You cannot use " + itemName + " here");
                    }
                }
    }

//   public override void SecondaryUse()
//   {
//               //   if (!used)
//               {
//                   ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
//
//                   if (Physics.Raycast(ray, out hit, rayMaxDist))
//                   {
//                       if (hit.transform.CompareTag("Tree"))
//                       {
//                           used = true;
//                           useTimer = useRate;
//                           hit.transform.GetComponent<Tree>().Harvest();
//                           hit.transform.GetComponent<Tree>().Harvest();
//                           hit.transform.GetComponent<Tree>().Harvest();
//                           //    Instantiate(wood, hit.point, transform.rotation);
//                       }
//                       else
//                           ScreenMessage.instance.CreateMessage("You cannot use " + itemName + " here");
//                   }
//               }
//   }
        

    void OnDestroy()
    {
        SaveAndLoadManager.OnSave -= Save;
    }

    public override void Save()
    {
        SaveAndLoadManager.instance.saveData.axeSaveList.Add(new AxeSave(this));
        //Debug.Log("Saved item = " + name);
    }

}


[System.Serializable]
public class AxeSave
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

    public AxeSave(Axe axe)
    {
        level = axe.level;
        posX = axe.transform.position.x;
        posY = axe.transform.position.y;
        posZ = axe.transform.position.z;
        rotX = axe.transform.rotation.x;
        rotY = axe.transform.rotation.y;
        rotZ = axe.transform.rotation.z;
        rotW = axe.transform.rotation.w;
        if (axe.beingHeld)
        {
            for (int i = 0; i < PlayerInventory.instance.heldObjects.Count; ++i)
            {
                if (axe.gameObject == PlayerInventory.instance.heldObjects[i])
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
            Axe axePrefab = toolPrefab.GetComponent<Axe>();
            if (axePrefab == null)
                continue;

            if (axePrefab.level == level)
            {
                //Debug.Log("Loading Axe");
                GameObject axe = (GameObject)Object.Instantiate(toolPrefab, new Vector3(posX, posY, posZ), new Quaternion(rotX, rotY, rotZ, rotW));
                if (inventorySlot != -1)
                {
                    PlayerInventory.instance.AddItemInSlot(axe, inventorySlot);
                }
                return axe;
            }
        }
        Debug.Log("Failed to load Axe, level = " + level.ToString());
        return null;
    }
}

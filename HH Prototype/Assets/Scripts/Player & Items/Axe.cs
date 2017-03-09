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
        itemID = 3;
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
            //  case ClickType.Single:
            //      ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
            //
            //      //Debug.Log("Axe");
            //      if (Physics.Raycast(ray, out hit, rayMaxDist))
            //      {
            //          if (hit.transform.CompareTag("Tree"))
            //          {
            //              used = true;
            //              useTimer = useRate;
            //              hit.transform.GetComponent<Tree>().Harvest();
            //              if (level > 1)
            //                  hit.transform.GetComponent<Tree>().Harvest();
            //              if (level > 2)
            //                  hit.transform.GetComponent<Tree>().Harvest();
            //              //    Instantiate(wood, hit.point, transform.rotation);
            //          }
            //      }
            //      break;

            case ClickType.Hold:
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
                        //else if (hit.transform.CompareTag("Shelf"))
                        //{
                        //    hit.transform.GetComponent<Shelf>().StoreItem(gameObject);
                        //    PlayerInventory.instance.lClickTimer = 0;
                        //    PlayerInventory.instance.rClickTimer = 0;
                        //    //PlayerInventory.instance.qTimer = 0;
                        //    //PlayerInventory.instance.eTimer = 0;
                        //}
                    }
                }
                break;
        }

    }

    public override void SecondaryUse(ClickType click)
    {
        switch (click)
        {
            case ClickType.Hold:
                //   if (!used)
                {
                    ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

                    if (Physics.Raycast(ray, out hit, rayMaxDist))
                    {
                        if (hit.transform.CompareTag("Tree"))
                        {
                            used = true;
                            useTimer = useRate;
                            hit.transform.GetComponent<Tree>().Harvest();
                            hit.transform.GetComponent<Tree>().Harvest();
                            hit.transform.GetComponent<Tree>().Harvest();
                            //    Instantiate(wood, hit.point, transform.rotation);
                        }
                    }
                }
                break;
        }

    }
        

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

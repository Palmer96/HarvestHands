﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : Item
{

    public int level = 1;

    // Use this for initialization
    void Start()
    {
        startScale = transform.lossyScale;
        itemID = 3;
        itemCap = 1;

        if (singleMesh == null)
            singleMesh = GetComponent<MeshFilter>().mesh;
        if (multiMesh == null)
            multiMesh = GetComponent<MeshFilter>().mesh;
        if (singleMaterial == null)
            singleMaterial = GetComponent<MeshRenderer>().material;
        if (multiMaterial == null)
            multiMaterial = GetComponent<MeshRenderer>().material;


        SaveAndLoadManager.OnSave += Save;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void PrimaryUse(ClickType click)
    {
        if (!used)
        {
            switch (click)
            {
                case ClickType.Single:
                    ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

                    Debug.Log("Hammer2");
                    if (Physics.Raycast(ray, out hit, rayMaxDist))
                    {
                        if (hit.transform.CompareTag("Building"))
                        {
                            Debug.Log("Building2");
                            used = true;
                            useTimer = useRate;
                            hit.transform.GetComponent<Building>().Move();
                        }
                        else
                            ScreenMessage.instance.CreateMessage("You cannot use " + itemName + " here");
                    }
                    break;
                case ClickType.Hold:
                    ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

                    Debug.Log("Hammer");
                    if (Physics.Raycast(ray, out hit, rayMaxDist))
                    {
                        if (hit.transform.CompareTag("Building"))
                        {
                            Debug.Log("Building");
                            used = true;
                            useTimer = useRate;
                            hit.transform.GetComponent<Building>().Build();
                        }
                        else
                            ScreenMessage.instance.CreateMessage("You cannot use " + itemName + " here");
                    }
                    break;

                    //     case ClickType.Hold:
            }
        }
    }

    public override void SecondaryUse(ClickType click)
    {
        switch (click)
        {
            case ClickType.Hold:
                // if (!used)
                {
                    ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

                    Debug.Log("Hammer");
                    if (Physics.Raycast(ray, out hit, rayMaxDist))
                    {
                        switch (hit.transform.tag)
                        {
                            case "Building":
                                useTimer = useRate;
                                hit.transform.GetComponent<Building>().Deconstruct();
                                break;
                            case "Built":
                                Destroy(hit.transform.gameObject);
                                break;
                            case "Soil":
                                Destroy(hit.transform.parent.gameObject);
                                break;
                            default:
                                ScreenMessage.instance.CreateMessage("You cannot use " + itemName + " here");
                                break;
                        }
                    }
                }
                break;
        }

    }

    public void HammerUp()
    {
        used = false;
    }

    public override void Save()
    {
        SaveAndLoadManager.instance.saveData.hammerSaveList.Add(new HammerSave(this));
    }

    void OnDestroy()
    {
        SaveAndLoadManager.OnSave -= Save;
    }
}

[System.Serializable]
public class HammerSave
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

    public HammerSave(Hammer hammer)
    {
        level = hammer.level;
        posX = hammer.transform.position.x;
        posY = hammer.transform.position.y;
        posZ = hammer.transform.position.z;
        rotX = hammer.transform.rotation.x;
        rotY = hammer.transform.rotation.y;
        rotZ = hammer.transform.rotation.z;
        rotW = hammer.transform.rotation.w;
        if (hammer.beingHeld)
        {
            for (int i = 0; i < PlayerInventory.instance.heldObjects.Count; ++i)
            {
                if (hammer.gameObject == PlayerInventory.instance.heldObjects[i])
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
            Hammer hammerPrefab = toolPrefab.GetComponent<Hammer>();
            if (hammerPrefab == null)
                continue;

            if (hammerPrefab.level == level)
            {
                //Debug.Log("Loading Hammer");
                GameObject hammer = (GameObject)Object.Instantiate(toolPrefab, new Vector3(posX, posY, posZ), new Quaternion(rotX, rotY, rotZ, rotW));
                if (inventorySlot != -1)
                {
                    PlayerInventory.instance.AddItemInSlot(hammer, inventorySlot);
                }
                return hammer;
            }
        }
        Debug.Log("Failed to load hammer, level = " + level.ToString());
        return null;
    }
}
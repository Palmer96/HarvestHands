using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickaxe : Item
{

    public int level = 1;
    // Use this for initialization
    void Start()
    {
        startScale = transform.lossyScale;
        itemID = 7;
        itemCap = 1;
        MinimapManager.instance.CreateImage(transform, new Color(0.1f, 1f, 1.1f));
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
        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

        Debug.Log("Pickaxe");
        if (Physics.Raycast(ray, out hit, rayMaxDist))
        {
            if (hit.transform.CompareTag("Rock"))
            {
                used = true;
                useTimer = useRate;
                hit.transform.GetComponent<Rock>().Harvest();
                if (level > 1)
                    hit.transform.GetComponent<Rock>().Harvest();
                if (level > 2)
                    hit.transform.GetComponent<Rock>().Harvest();
                //    Instantiate(wood, hit.point, transform.rotation);
            }
            else
                ScreenMessage.instance.CreateMessage("You cannot use " + itemName + " here");
        }
    }

}

[System.Serializable]
public class PickaxeSave
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

    public PickaxeSave(Pickaxe pickaxe)
    {
        level = pickaxe.level;
        posX = pickaxe.transform.position.x;
        posY = pickaxe.transform.position.y;
        posZ = pickaxe.transform.position.z;
        rotX = pickaxe.transform.rotation.x;
        rotY = pickaxe.transform.rotation.y;
        rotZ = pickaxe.transform.rotation.z;
        rotW = pickaxe.transform.rotation.w;
        if (pickaxe.beingHeld)
        {
            for (int i = 0; i < PlayerInventory.instance.heldObjects.Count; ++i)
            {
                if (pickaxe.gameObject == PlayerInventory.instance.heldObjects[i])
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
            Pickaxe pickaxePrefab = toolPrefab.GetComponent<Pickaxe>();
            if (pickaxePrefab == null)
                continue;

            if (pickaxePrefab.level == level)
            {
                //Debug.Log("Loading Axe");
                GameObject pickaxe = (GameObject)Object.Instantiate(toolPrefab, new Vector3(posX, posY, posZ), new Quaternion(rotX, rotY, rotZ, rotW));
                if (inventorySlot != -1)
                {
                    PlayerInventory.instance.AddItemInSlot(pickaxe, inventorySlot);
                }
                return pickaxe;
            }
        }
        Debug.Log("Failed to load Axe, level = " + level.ToString());
        return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ClickType
    {
        Single,
        Hold
    };

    public int itemID;
    public string itemName;
    public int quantity;
    public int value;
    public bool sellable;
    public int itemCap;

    public float rayMaxDist = 5;

    public RaycastHit hit;
    public Ray ray;

    public bool used = false;
    public float useTimer = 0.25f;
    public float useRate = 0.25f;

    //  protected MeshFilter ownMesh;
    //  protected Material ownMaterial;
    //  protected MeshCollider ownMeshCollider;

    public Mesh singleMesh;
    public Mesh multiMesh;

    public Material singleMaterial;
    public Material multiMaterial;

    public bool dontUpdate;
    public bool beingHeld = false;

    public Vector3 startScale;

    // Use this for initialization
    void Start()
    {
        startScale = transform.lossyScale;
        if (!dontUpdate)
        {
            if (singleMesh == null)
                singleMesh = GetComponent<MeshFilter>().mesh;
            if (multiMesh == null)
                multiMesh = GetComponent<MeshFilter>().mesh;
            if (singleMaterial == null)
                singleMaterial = GetComponent<MeshRenderer>().material;
            if (multiMaterial == null)
                multiMaterial = GetComponent<MeshRenderer>().material;

            UpdateMesh();
        }
        if (itemCap == 0)
        {
            itemCap = 20;
        }

        MinimapManager.instance.CreateImage(transform, Color.blue);

        SaveAndLoadManager.OnSave += Save;

    }

    public virtual void PrimaryUse()
    {
        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

        if (Physics.Raycast(ray, out hit, rayMaxDist))
        {
            if (hit.transform.CompareTag("Building"))
            {
                hit.transform.GetComponent<Building>().AddResource(gameObject);
            }
           else if (hit.transform.CompareTag("SellZone"))
            {
                hit.transform.GetComponent<SellChest>().AddToSell(gameObject);
            }
            else
            ScreenMessage.instance.CreateMessage("You cannot use " + itemName + " here");
        }
    }

    public virtual void PrimaryUse(ClickType click)
    {
        if (AttemptInteract(click))
            return;
    }

    public virtual void PrimaryUse(GameObject gameObj)
    {
        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

        if (Physics.Raycast(ray, out hit, rayMaxDist))
        {
            if (hit.transform.CompareTag("Building"))
            {
                hit.transform.GetComponent<Building>().AddResource(gameObject);
            }
            else if (hit.transform.CompareTag("SellZone"))
            {
                hit.transform.GetComponent<SellChest>().AddToSell(gameObject);
            }
        }
    }


    public virtual void SecondaryUse()
    {

    }

    public virtual void SecondaryUse(ClickType click)
    {
        
    }

    public virtual void SecondaryUse(GameObject gameObj)
    {

    }




    public virtual void UpdateMesh()
    {
        bool colliderEnabled = false;

        if (GetComponent<MeshCollider>() != null)
        {
            colliderEnabled = GetComponent<MeshCollider>().enabled;
            GetComponent<MeshCollider>().enabled = true;
        }
        if (!dontUpdate)
        {
            if (quantity > 1)
            {
                GetComponent<MeshFilter>().mesh = multiMesh;
                GetComponent<MeshRenderer>().material = multiMaterial;
                if (GetComponent<MeshCollider>() != null)
                    GetComponent<MeshCollider>().sharedMesh = multiMesh;
            }
            else
            {
                GetComponent<MeshFilter>().mesh = singleMesh;
                GetComponent<MeshRenderer>().material = singleMaterial;
                if (GetComponent<MeshCollider>() != null)
                    GetComponent<MeshCollider>().sharedMesh = singleMesh;
            }
            if (GetComponent<MeshCollider>() != null)
                GetComponent<MeshCollider>().enabled = colliderEnabled;
        }
    }

    public virtual void IncreaseQuantity()
    {
        quantity++;
        UpdateMesh();
    }

    public virtual void IncreaseQuantity(int amount)
    {
        quantity += amount;
        UpdateMesh();
    }

    public virtual void DecreaseQuantity()
    {
        quantity--;
        if (quantity == 1)
        {
            UpdateMesh();
        }

        if (quantity < 0)
        {
            Destroy(gameObject);
        }
    }

    public virtual void DecreaseQuantity(int amount)
    {
        quantity -= amount;
        if (quantity == 1)
        {
            UpdateMesh();
        }

        if (quantity < 0)
        {
            Destroy(gameObject);
        }
    }

    public virtual bool AttemptInteract(ClickType click)
    {
        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

        if (Physics.Raycast(ray, out hit, rayMaxDist))
        {
            if (hit.transform.CompareTag("Building"))
            {
                hit.transform.GetComponent<Building>().AddResource(gameObject);
                return true;
            }
            else if (hit.transform.CompareTag("Shelf"))
            {
                hit.transform.GetComponent<Shelf>().StoreItem(gameObject);
                return true;
            }
            else if (hit.transform.CompareTag("SellZone"))
            {
                if (click == ClickType.Single)
                {
                    if (quantity > 1)
                    {
                        GameObject sellingObject = Instantiate(gameObject);
                        sellingObject.GetComponent<Item>().quantity = 1;
                        hit.transform.GetComponent<SellChest>().AddToSell(sellingObject);
                        DecreaseQuantity();
                    }
                    else
                    {
                        hit.transform.GetComponent<SellChest>().AddToSell(gameObject);
                    }
                    return true;
                }
                else if (click == ClickType.Hold)
                {
                    hit.transform.GetComponent<SellChest>().AddToSell(gameObject);
                    return true;
                }
            }
        }
        return false;
    }

    public virtual void Save()
    {
        SaveAndLoadManager.instance.saveData.itemSaveList.Add(new ItemSave(this));
        //Debug.Log("Saved item = " + name);
    }    

    void OnDestroy()
    {
        SaveAndLoadManager.OnSave -= Save;
    }
}

[System.Serializable]
public class ItemSave
{
    public int itemID;
    public int quantity;
    float posX;
    float posY;
    float posZ;
    float rotX;
    float rotY;
    float rotZ;
    float rotW;
    public int inventorySlot;

    public ItemSave(Item item)
    {
        itemID = item.itemID;
        quantity = item.quantity;
        posX = item.transform.position.x;
        posY = item.transform.position.y;
        posZ = item.transform.position.z;
        rotX = item.transform.rotation.x;
        rotY = item.transform.rotation.y;
        rotZ = item.transform.rotation.z;
        rotW = item.transform.rotation.w;
        if (item.beingHeld)
        {
            for (int i = 0; i < PlayerInventory.instance.heldObjects.Count; ++i)
            {
                if (item.gameObject == PlayerInventory.instance.heldObjects[i])
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
        foreach (GameObject toolPrefab in SaveAndLoadManager.instance.instantiateableItems)
        {
            Item itemPrefab = toolPrefab.GetComponent<Item>();
            if (itemPrefab == null)
                continue;

            if (itemPrefab.itemID == itemID)
            {
                //Debug.Log("Loading Item");
                GameObject item = (GameObject)Object.Instantiate(toolPrefab, new Vector3(posX, posY, posZ), new Quaternion(rotX, rotY, rotZ, rotW));
                item.GetComponent<Item>().quantity = quantity;
                if (inventorySlot != -1)
                {
                    PlayerInventory.instance.AddItemInSlot(item, inventorySlot);
                }
                return item;
            }
        }
        Debug.Log("Failed to load Item, ID = " + itemID.ToString());
        return null;
    }
}


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance = null;
    public int money = 0;


    //  public GameObject ItemHotbar;
    public GameObject ItemHotbar;
    public GameObject book;

    public bool bookOpen;

    //  public List<GameObject> heldObjects = new List<GameObject>();
    public List<GameObject> heldObjects = new List<GameObject>();
    //   public List<Sprite> itemSprites = new List<Sprite>();
    public List<Sprite> itemSprites = new List<Sprite>();


    //  public List<Image> itemImage = new List<Image>();
    public List<Text> itemText = new List<Text>();


    public bool usingTools;

    public float scroll;
    public int selectedItemNum;

    public bool inMenu;
    int oldnum;

    private float scrollTimer;
    // Use this for initialization
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        scrollTimer = 0.1f;

        inMenu = true;
        //  heldObjects[0].transform.SetParent(transform.GetChild(0));
        //  heldObjects[0].transform.localPosition = new Vector3(1, 0, 2);

        //   heldObjects[1].transform.SetParent(transform.GetChild(1));
        //   heldObjects[1].transform.localPosition = new Vector3(1, 0, 2);

        UpdateInventory();
    }

    // Update is called once per frame
    void Update()
    {
        scrollTimer -= Time.deltaTime;

        UpdateItemMesh();
        UpdateImages();

        Hotkeys();

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (bookOpen)
            {
                bookOpen = false;
                book.SetActive(false);
                selectedItemNum = oldnum;
                book.GetComponent<Blueprint>().ConstructionCancel();
            }
            else
            {
                bookOpen = true;
                book.SetActive(true);
                selectedItemNum = 10;
            }
        }

        if (!bookOpen || !inMenu)
        {
            UpdateInventory();

            if (Input.GetKeyDown(KeyCode.Q)) // Drop
            {
                // if (selectedItemNum != 0)
                RemoveItem();
            }

            if (Input.GetKeyDown(KeyCode.F)) // Drop Stack
            {
                // if (selectedItemNum != 0)
                DropAllofItem();
            }



            if (Input.GetKeyDown(KeyCode.E)) // Interact
            {
                RaycastHit hit;
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

                if (Physics.Raycast(ray, out hit, 5))
                {
                    switch (hit.transform.tag)
                    {
                        case "StoreItem":
                            hit.transform.GetComponent<StoreItem>().BuyObject();
                            break;
                        case "Bed":
                            DayNightController.instance.BedDayJump();
                            break;
                    }
                }
            }

            if (Input.GetMouseButtonDown(0)) // Primary Use
            {
                if (heldObjects[selectedItemNum] != null)
                {
                    heldObjects[selectedItemNum].GetComponent<Item>().PrimaryUse();
                }
            }
            if (Input.GetMouseButtonDown(1)) // Pickup
            {
                RaycastHit hit;
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

                if (Physics.Raycast(ray, out hit, 5))
                {
                    if (hit.transform.CompareTag("Item"))
                        AddItem(hit.transform.gameObject);
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0)) // Primary Use
            {
                book.GetComponent<Blueprint>().PrimaryUse();
            }
            if (Input.GetMouseButtonDown(1)) // Pickup
            {
                book.GetComponent<Blueprint>().SecondaryUse();
            }
        }
    }


    public bool AddItem(GameObject item)
    {
        for (int i = 0; i < heldObjects.Count; i++)
        {
            if (heldObjects[i] != null)
            {
                if (heldObjects[i].GetComponent<Item>().itemID == item.GetComponent<Item>().itemID)
                {
                    heldObjects[i].GetComponent<Item>().IncreaseQuantity(item.GetComponent<Item>().quantity);
                    if (heldObjects[i].GetComponent<UnityEngine.AI.NavMeshAgent>() != null)
                        WaveManager.instance.rabbitsLeft--;

                    Destroy(item);
                    return true;
                }
            }
        }
        for (int i = 0; i < heldObjects.Count; i++)
        {
            if (heldObjects[i] == null)
            {
                heldObjects[i] = item;
                heldObjects[i].transform.SetParent(transform.GetChild(0));
                heldObjects[i].transform.localPosition = new Vector3(1, 0, 2);
                heldObjects[i].GetComponent<Rigidbody>().isKinematic = true;
                heldObjects[i].layer = 2;
                heldObjects[i].GetComponent<Collider>().enabled = false;
                heldObjects[i].transform.rotation = transform.GetChild(0).rotation;
                if (heldObjects[i].GetComponent<UnityEngine.AI.NavMeshAgent>() != null)
                {
                    heldObjects[i].GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
                    WaveManager.instance.rabbitsLeft--;
                }

                return true;
            }
        }
        return false;

    }

    public void RemoveItem()
    {
        if (heldObjects[selectedItemNum] != null)
        {
            if (heldObjects[selectedItemNum].GetComponent<Item>().quantity > 1)
            {
                GameObject droppedItem = Instantiate(heldObjects[selectedItemNum], transform.GetChild(0).position + (transform.GetChild(0).forward * 2), transform.rotation);
                droppedItem.SetActive(true);
                droppedItem.GetComponent<Item>().quantity = 1;
                droppedItem.transform.parent = null;
                droppedItem.GetComponent<Rigidbody>().isKinematic = false;
                droppedItem.GetComponent<Rigidbody>().AddForce(transform.GetChild(0).forward * 500, ForceMode.Force);
                droppedItem.GetComponent<Collider>().enabled = true;
                droppedItem.layer = 0;

                heldObjects[selectedItemNum].GetComponent<Item>().DecreaseQuantity();
                if (heldObjects[selectedItemNum].GetComponent<Item>().itemID < 10)
                    heldObjects[selectedItemNum].GetComponent<Item>().UpdateMesh();
                if (droppedItem.GetComponent<Item>().itemID < 10)
                    droppedItem.GetComponent<Item>().UpdateMesh();
            }
            else
            {
                heldObjects[selectedItemNum].GetComponent<Rigidbody>().isKinematic = false;
                heldObjects[selectedItemNum].GetComponent<Rigidbody>().AddForce(transform.GetChild(0).forward * 500, ForceMode.Force);
                heldObjects[selectedItemNum].GetComponent<Collider>().enabled = true;
                heldObjects[selectedItemNum].transform.parent = null;
                heldObjects[selectedItemNum].layer = 0;
                if (heldObjects[selectedItemNum].GetComponent<Item>().itemID < 10)
                    heldObjects[selectedItemNum].GetComponent<Item>().UpdateMesh();
                heldObjects[selectedItemNum] = null;
            }
        }
    }

    public void DestroyItem()
    {
        Destroy(heldObjects[selectedItemNum]);
        heldObjects[selectedItemNum] = null;
    }

    public void DropAllofItem()
    {


        heldObjects[selectedItemNum].GetComponent<Rigidbody>().isKinematic = false;


        heldObjects[selectedItemNum].GetComponent<Rigidbody>().AddForce(transform.GetChild(0).forward * 500, ForceMode.Force);
        heldObjects[selectedItemNum].GetComponent<Collider>().enabled = true;
        heldObjects[selectedItemNum].transform.parent = null;
        heldObjects[selectedItemNum].layer = 0;
        heldObjects[selectedItemNum] = null;
    }

    void UpdateImages()
    {
        for (int i = 0; i < heldObjects.Count; i++)
        {

            if (i == selectedItemNum)
            {
                itemText[i].color = Color.yellow;
                itemText[i].transform.GetChild(1).GetComponent<Image>().color = Color.yellow;
            }
            else
            {
                itemText[i].color = Color.white;
                itemText[i].transform.GetChild(1).GetComponent<Image>().color = Color.grey;
            }


            if (heldObjects[i] != null)
            {
                // itemText[i].sprite = itemSprites[heldObjects[i].GetComponent<Item>().itemID];

                itemText[i].text = heldObjects[i].GetComponent<Item>().itemName;
                if (heldObjects[i].GetComponent<Item>().quantity > 1)
                    itemText[i].transform.GetChild(0).transform.GetComponentInChildren<Text>().text = heldObjects[i].GetComponent<Item>().quantity.ToString();
                else
                    itemText[i].transform.GetChild(0).transform.GetComponentInChildren<Text>().text = "";
            }
            else
            {
                //  itemText[i].sprite = itemSprites[0];
                itemText[i].transform.GetComponentInChildren<Text>().text = "-";
                itemText[i].transform.GetChild(0).transform.GetComponentInChildren<Text>().text = "";
            }
        }
    }

    void UpdateInventory()
    {
        if (scrollTimer < 0)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0.05f)
            {
               
                if (selectedItemNum > 0)
                {
                    selectedItemNum--;
                    scrollTimer = 0.1f;
                }
                else
                {
                    selectedItemNum = heldObjects.Capacity - 1;
                }
            }

            if (Input.GetAxis("Mouse ScrollWheel") < -0.05f)
            {
                if (selectedItemNum < itemText.Count - 1)
                {
                    selectedItemNum++;
                    scrollTimer = 0.1f;
                }
                else
                    selectedItemNum = 0;
            }
            oldnum = selectedItemNum;
        }
    }


    public void UseItem()
    {
        if (heldObjects[selectedItemNum] != null)
            heldObjects[selectedItemNum].GetComponent<Item>().PrimaryUse();
    }

    public void UseItem(GameObject gameObj)
    {
        if (heldObjects[selectedItemNum] != null)
            heldObjects[selectedItemNum].GetComponent<Item>().PrimaryUse(gameObj);
    }


    void UpdateItemMesh()
    {

        for (int i = 0; i < heldObjects.Count; i++)
            if (heldObjects[i] != null)
                HideObject(heldObjects[i]);


        for (int i = 0; i < heldObjects.Count; i++)
        {
            if (heldObjects[i] != null)
            {
                if (i == selectedItemNum)
                    ShowObject(heldObjects[i]);
                else
                    HideObject(heldObjects[i]);
            }
        }
    }

    void ShowObject(GameObject item)
    {
        if (item.GetComponent<MeshRenderer>() != null)
            item.GetComponent<MeshRenderer>().enabled = true;
        for (int i = 0; i < item.transform.childCount; i++)
        {
            if (item.transform.GetChild(i).GetComponent<MeshRenderer>() != null)
                item.transform.GetChild(i).GetComponent<MeshRenderer>().enabled = true;
            for (int j = 0; j < item.transform.GetChild(i).childCount; j++)
            {
                if (item.transform.GetChild(i).GetChild(j).GetComponent<MeshRenderer>() != null)
                    item.transform.GetChild(i).GetChild(j).GetComponent<MeshRenderer>().enabled = true;
            }
        }

    }

    void HideObject(GameObject item)
    {
        if (item.GetComponent<MeshRenderer>() != null)
            item.GetComponent<MeshRenderer>().enabled = false;
        for (int i = 0; i < item.transform.childCount; i++)
        {
            if (item.transform.GetChild(i).GetComponent<MeshRenderer>() != null)
                item.transform.GetChild(i).GetComponent<MeshRenderer>().enabled = false;
            for (int j = 0; j < item.transform.GetChild(i).childCount; j++)
            {
                if (item.transform.GetChild(i).GetChild(j).GetComponent<MeshRenderer>() != null)
                    item.transform.GetChild(i).GetChild(j).GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }

    public int BlueprintIndex()
    {
        for (int i = 0; i < heldObjects.Count; i++)
        {
            if (heldObjects[i] != null)
            {
                if (heldObjects[i].GetComponent<Item>().itemID == 5)
                {
                    return i;
                }
            }
        }
        return 0;
    }

    public bool HasBook()
    {
        for (int i = 0; i < heldObjects.Count; i++)
        {
            if (heldObjects[i] != null)
            {
                if (heldObjects[i].GetComponent<Item>().itemID == 5)
                {
                    Debug.Log("Has Book");
                    return true;
                }
            }
        }

        return false;
    }

    public void AddBlueprint(GameObject item)
    {
        GameObject bPrint = null;
        for (int i = 0; i < heldObjects.Count; i++)
        {
            if (heldObjects[i] != null)
            {
                if (heldObjects[i].GetComponent<Item>().itemID == 5)
                {
                    bPrint = heldObjects[i];
                    Debug.Log("Yep, has Book");
                    break;
                }
            }
        }

        if (bPrint != null)
        {
            Debug.Log("Add Blueprint");
            bPrint.GetComponent<Blueprint>().AddBuild(item);
        }
    }

    public void Hotkeys()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            selectedItemNum = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            selectedItemNum = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3))
            selectedItemNum = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4))
            selectedItemNum = 3;
        if (Input.GetKeyDown(KeyCode.Alpha5))
            selectedItemNum = 4;
        if (Input.GetKeyDown(KeyCode.Alpha6))
            selectedItemNum = 5;
        if (Input.GetKeyDown(KeyCode.Alpha7))
            selectedItemNum = 6;


    }

}

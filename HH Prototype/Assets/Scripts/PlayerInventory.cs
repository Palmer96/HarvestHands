using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance = null;
    public int money = 0;


    public GameObject ToolHotbar;
    public GameObject ItemHotbar;


    public List<GameObject> heldTools = new List<GameObject>();
    public List<GameObject> heldObjects = new List<GameObject>();
    public List<Sprite> toolSprites = new List<Sprite>();
    public List<Sprite> itemSprites = new List<Sprite>();

    public List<Image> toolImage = new List<Image>();

    public List<Image> itemImage = new List<Image>();

    public bool usingTools;

    public float scroll;
    public int selectedToolNum;
    public int selectedItemNum;

    private float scrollTimer;
    // Use this for initialization
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        scrollTimer = 0.1f;

        heldTools[0].transform.SetParent(transform.GetChild(0));
        heldTools[0].transform.localPosition = new Vector3(1, 0, 2);

        UpdateTools();
        UpdateInventory();
        if (usingTools)
        {
            ToolHotbar.SetActive(true);
            ItemHotbar.SetActive(false);
        }
        else
        {
            ToolHotbar.SetActive(false);
            ItemHotbar.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        scrollTimer -= Time.deltaTime;
        toolImage[0].sprite = toolSprites[5];
        if (usingTools)
        {
            UpdateTools();
            UpdateToolMesh();
        }
        else
        {
            UpdateInventory();
            UpdateItemMesh();
        }

        UpdateImages();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (usingTools)
            {
                usingTools = false;
                ToolHotbar.SetActive(false);
                ItemHotbar.SetActive(true);
            }
            else
            {
                usingTools = true;
                ToolHotbar.SetActive(true);
                ItemHotbar.SetActive(false);
            }
        }

        if (!usingTools || selectedToolNum != 0)
        {
            heldTools[0].GetComponent<Hand>().Drop();
        }



        if (Input.GetKeyDown(KeyCode.F))
        {
            DropAllofItem();
        }

        // added by nick
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
            if (Physics.Raycast(ray, out hit, 5))
            {
                if (hit.transform.tag == "NPC")
                {
                    NPC npc = hit.transform.GetComponent<NPC>();
                    if (npc == null)
                        Debug.Log("npc = null");
                    Debug.Log("Talking to " + npc.npcName);

                    EventManager.TalkEvent(npc.npcName);
                }
                else if (hit.transform.tag == "NoticeBoard")
                {
                    hit.transform.GetComponent<PrototypeObjectiveBoard>().GetRandomQuest();
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

            if (Physics.Raycast(ray, out hit, 5))
            {
                switch (hit.transform.tag)
                {
                    case "Tool":
                        if (usingTools)
                        {
                            if (selectedToolNum != 0)
                                AddTool(hit.transform.gameObject);
                            else
                                heldTools[0].GetComponent<Hand>().UseTool(hit.transform.gameObject);
                        }
                        break;
                    case "Item":
                        if (!usingTools)
                            AddItem(hit.transform.gameObject);
                        else if (selectedToolNum == 0)
                            heldTools[0].GetComponent<Hand>().PickUp(hit.transform.gameObject);
                        break;
                    case "StoreItem":
                        hit.transform.GetComponent<StoreItem>().BuyObject();
                        break;
                    case "Bed":
                        DayNightController.instance.DayJump();
                        break;

                    default:
                        if (!usingTools)
                            UseItem();
                        else
                            UseTool();
                        break;
                }
            }
        }
        if (Input.GetMouseButtonDown(1))// && heldItem)
        {
            if (!usingTools)
                RemoveItem();
            else
            {
                if (selectedToolNum != 0)
                    RemoveTool();
                else
                    heldTools[0].GetComponent<Hand>().Throw();
            }
        }
    }

    public bool AddTool(GameObject item)
    {
        for (int i = 0; i < heldTools.Count; i++)
        {
            if (heldTools[i] == null)
            {
                heldTools[i] = item;
                heldTools[i].transform.SetParent(transform.GetChild(0));
                heldTools[i].transform.localPosition = new Vector3(1, 0, 2);
                heldTools[i].GetComponent<Rigidbody>().isKinematic = true;
                heldTools[i].GetComponent<Collider>().enabled = false;
                heldTools[i].layer = 2;
                heldTools[i].transform.rotation = transform.GetChild(0).rotation;
                return true;
            }
        }
        return false;

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

                return true;
            }
        }
        return false;

    }

    public void RemoveTool()
    {
        if (selectedToolNum != 0)
        {
            if (heldTools[selectedToolNum] != null)
            {
                heldTools[selectedToolNum].GetComponent<Rigidbody>().isKinematic = false;
                heldTools[selectedToolNum].GetComponent<Rigidbody>().AddForce(transform.GetChild(0).forward * 500, ForceMode.Force);
                heldTools[selectedToolNum].transform.parent = null;
                heldTools[selectedToolNum].GetComponent<Collider>().enabled = true;
                heldTools[selectedToolNum].layer = 0;
                heldTools[selectedToolNum] = null;
            }
        }
    }


    public void RemoveItem()
    {
        if (heldObjects[selectedItemNum] != null)
        {
            if (heldObjects[selectedItemNum].GetComponent<Item>().quantity > 1)
            {
                GameObject droppedItem = Instantiate(heldObjects[selectedItemNum], transform.GetChild(0).position, transform.rotation);
                droppedItem.SetActive(true);
                droppedItem.GetComponent<Item>().quantity = 1;
                droppedItem.transform.parent = null;
                droppedItem.GetComponent<Rigidbody>().isKinematic = false;
                droppedItem.GetComponent<Rigidbody>().AddForce(transform.GetChild(0).forward * 500, ForceMode.Force);
                droppedItem.GetComponent<Collider>().enabled = true;
                droppedItem.layer = 0;

                heldObjects[selectedItemNum].GetComponent<Item>().DecreaseQuantity();
                heldObjects[selectedItemNum].GetComponent<Item>().UpdateMesh();
                droppedItem.GetComponent<Item>().UpdateMesh();
            }
            else
            {
                heldObjects[selectedItemNum].GetComponent<Rigidbody>().isKinematic = false;
                heldObjects[selectedItemNum].GetComponent<Rigidbody>().AddForce(transform.GetChild(0).forward * 500, ForceMode.Force);
                heldObjects[selectedItemNum].GetComponent<Collider>().enabled = true;
                heldObjects[selectedItemNum].transform.parent = null;
                heldObjects[selectedItemNum].layer = 0;
                heldObjects[selectedItemNum].GetComponent<Item>().UpdateMesh();
                heldObjects[selectedItemNum] = null;
            }
        }
    }

    public void DestroyItem()
    {
        Destroy(heldObjects[selectedItemNum]);
        heldObjects[selectedToolNum] = null;
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
        for (int i = 0; i < heldTools.Count; i++)
        {
            if (i != 0)
            {
                if (heldTools[i] != null)
                {
                    toolImage[i].sprite = toolSprites[heldTools[i].GetComponent<Tool>().toolID];
                    if (heldTools[i].GetComponent<Bucket>() != null)
                        toolImage[i].transform.GetComponentInChildren<Text>().text = heldTools[i].GetComponent<Bucket>().currentWaterLevel.ToString();
                    else
                        toolImage[i].transform.GetComponentInChildren<Text>().text = "";
                }
                else
                {
                    toolImage[i].sprite = toolSprites[0];
                    toolImage[i].transform.GetComponentInChildren<Text>().text = "";
                }
            }

            if (i == selectedToolNum)
            {
                toolImage[i].color = Color.blue;
            }
            else
                toolImage[i].color = Color.white;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////
        for (int i = 0; i < heldObjects.Count; i++)
        {
            if (heldObjects[i] != null)
            {

                itemImage[i].sprite = itemSprites[heldObjects[i].GetComponent<Item>().itemID];
                itemImage[i].transform.GetComponentInChildren<Text>().text = heldObjects[i].GetComponent<Item>().quantity.ToString();

            }
            else
            {
                itemImage[i].sprite = itemSprites[0];
                itemImage[i].transform.GetComponentInChildren<Text>().text = "";
            }

            if (i == selectedItemNum)
            {
                itemImage[i].color = Color.blue;
            }
            else
                itemImage[i].color = Color.white;
        }
    }

    void UpdateInventory()
    {
        if (scrollTimer < 0)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0.05f)
            {
                if (selectedItemNum < itemImage.Count - 1)
                {
                    selectedItemNum++;
                    scrollTimer = 0.1f;
                }
            }

            if (Input.GetAxis("Mouse ScrollWheel") < -0.05f)
            {
                if (selectedItemNum > 0)
                {
                    selectedItemNum--;
                    scrollTimer = 0.1f;
                }
            }
        }
    }

    void UpdateTools()
    {
        if (scrollTimer < 0)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0.05f)
            {
                if (selectedToolNum < toolImage.Count - 1)
                {
                    selectedToolNum++;
                    scrollTimer = 0.1f;
                }
            }

            if (Input.GetAxis("Mouse ScrollWheel") < -0.05f)
            {
                if (selectedToolNum > 0)
                {
                    selectedToolNum--;
                    scrollTimer = 0.1f;
                }
            }
        }
    }

    public void UseTool()
    {
        if (heldTools[selectedToolNum] != null)
            heldTools[selectedToolNum].GetComponent<Tool>().UseTool();
    }

    public void UseTool(GameObject gameObj)
    {
        if (heldTools[selectedToolNum] != null)
            heldTools[selectedToolNum].GetComponent<Tool>().UseTool(gameObj);
    }

    public void UseItem()
    {
        if (heldObjects[selectedItemNum] != null)
            heldObjects[selectedItemNum].GetComponent<Item>().UseItem();
    }

    public void UseItem(GameObject gameObj)
    {
        if (heldObjects[selectedItemNum] != null)
            heldObjects[selectedItemNum].GetComponent<Item>().UseItem(gameObj);
    }

    void UpdateToolMesh()
    {

        for (int i = 0; i < heldObjects.Count; i++)
            if (heldObjects[i] != null)
                HideObject(heldObjects[i]);

        for (int i = 0; i < heldTools.Count; i++)
        {
            if (heldTools[i] != null)
            {
                if (i == selectedToolNum)
                    ShowObject(heldTools[i]);
                else
                    HideObject(heldTools[i]);
            }
        }
    }

    void UpdateItemMesh()
    {

        for (int i = 0; i < heldTools.Count; i++)
            if (heldTools[i] != null)
                HideObject(heldTools[i]);


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

    public bool HasBook()
    {
        for (int i = 0; i < heldTools.Count; i++)
        {
            if (heldTools[i] != null)
            {
                if (heldTools[i].GetComponent<Tool>().toolID == 5)
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
        for (int i = 0; i < heldTools.Count; i++)
        {
            if (heldTools[i] != null)
            {
                if (heldTools[i].GetComponent<Tool>().toolID == 5)
                {
                    bPrint = heldTools[i];
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

}

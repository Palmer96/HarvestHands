using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance = null;
    public int money = 0;


    public Image ToolsBack;
    public Image ItemsBack;
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


        UpdateTools();
        UpdateInventory();
        if (usingTools)
        {
            ToolsBack.color = Color.red;
            ItemsBack.color = Color.grey;
        }
        else
        {
            ToolsBack.color = Color.grey;
            ItemsBack.color = Color.green;
        }
    }

    // Update is called once per frame
    void Update()
    {
        scrollTimer -= Time.deltaTime;

        if (usingTools)
            UpdateTools();
        else
            UpdateInventory();

        UpdateImages();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (usingTools)
            {
                usingTools = false;
                ToolsBack.color = Color.grey;
                ItemsBack.color = Color.green;
            }
            else
            {
                usingTools = true;
                ToolsBack.color = Color.red;
                ItemsBack.color = Color.grey;
            }
        }
    }


    public void ActivateItem(int number)
    {

    }


    public bool AddTool(GameObject item)
    {
        for (int i = 0; i < heldTools.Count; i++)
        {
            if (heldTools[i] == null)
            {
                heldTools[i] = item;
                item.SetActive(false);
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
                    heldObjects[i].GetComponent<Item>().IncreaseQuantity();


                    // item.SetActive(false);
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

                heldObjects[i] = item;
                heldObjects[i].transform.SetParent(transform.GetChild(0));
                heldObjects[i].transform.localPosition = new Vector3(1, 0, 2);
                heldObjects[i].GetComponent<Rigidbody>().isKinematic = true;

                heldObjects[i].layer = 2;
                

                //item.SetActive(false);
                return true;
            }
        }
        return false;

    }

    public void RemoveTool()
    {
        if (heldTools[selectedToolNum] != null)
        {
            GameObject droppedTool = Instantiate(heldTools[selectedToolNum], (transform.position + transform.forward * 2), transform.rotation);
            droppedTool.SetActive(true);
            heldTools[selectedToolNum] = null;
        }
    }


    public void RemoveItem()
    {
        if (heldObjects[selectedItemNum] != null)
        {
            if (heldObjects[selectedItemNum].GetComponent<Item>().quantity > 1)
            {
                GameObject droppedItem = Instantiate(heldObjects[selectedItemNum], (transform.position + transform.forward * 2), transform.rotation);
                droppedItem.SetActive(true);
                droppedItem.GetComponent<Item>().quantity = 1;
                heldObjects[selectedItemNum].GetComponent<Item>().DecreaseQuantity();
                droppedItem.transform.parent = null;
                droppedItem.GetComponent<Rigidbody>().isKinematic = false;
                droppedItem.GetComponent<Rigidbody>().AddForce(transform.GetChild(0).forward * 500, ForceMode.Force);
                // transform.GetChild(0).DetachChildren();
                droppedItem.layer = 0;

            }
            else
            {
                heldObjects[selectedItemNum].GetComponent<Rigidbody>().isKinematic = false;
                heldObjects[selectedItemNum].GetComponent<Rigidbody>().AddForce(transform.GetChild(0).forward * 500, ForceMode.Force);

                heldObjects[selectedItemNum].transform.parent = null;

                heldObjects[selectedItemNum].layer = 0;

                heldObjects[selectedItemNum] = null;

                // GameObject droppedItem = Instantiate(heldObjects[selectedItemNum], (transform.position + transform.forward * 2), transform.rotation);
                // Destroy(heldObjects[selectedItemNum]);
                // droppedItem.SetActive(true);
                // heldObjects[selectedItemNum] = null;
            }
        }
    }

    void UpdateImages()
    {
        for (int i = 0; i < heldTools.Count; i++)
        {
            if (heldTools[i] != null)
            {
                toolImage[i].sprite = toolSprites[heldTools[i].GetComponent<Tool>().toolID];
            }
            else
            {
                toolImage[i].sprite = toolSprites[0];
            }

            if (i == selectedToolNum)
            {
                toolImage[i].color = Color.blue;
            }
            else
                toolImage[i].color = Color.white;
        }

        for (int i = 0; i < heldObjects.Count; i++)
        {
            if (heldObjects[i] != null)
            {
                itemImage[i].sprite = itemSprites[heldObjects[i].GetComponent<Item>().itemID];
            }
            else
            {
                itemImage[i].sprite = itemSprites[0];
            }

            if (i == selectedItemNum)
            {
                itemImage[i].color = Color.blue;
            }
            else
                itemImage[i].color = Color.white;

            // ADD NUMBER TO CHILD TEXT
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
        {
            heldTools[selectedToolNum].GetComponent<Tool>().UseTool();
        }

    }
}

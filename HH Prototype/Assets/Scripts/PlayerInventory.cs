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

        heldTools[0].transform.SetParent(transform.GetChild(0));
        heldTools[0].transform.localPosition = new Vector3(1, 0, 2);

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
        toolImage[0].sprite = toolSprites[5];
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

                heldTools[i] = item;
                heldTools[i].transform.SetParent(transform.GetChild(0));
                heldTools[i].transform.localPosition = new Vector3(1, 0, 2);
                heldTools[i].GetComponent<Rigidbody>().isKinematic = true;

                heldTools[selectedToolNum].GetComponent<Collider>().enabled = false;
                heldTools[i].layer = 2;
                //   Collider col = heldObjects[i].GetComponent<Collider>().GetType();


                //item.SetActive(false);
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
                    heldObjects[i].GetComponent<Resource>().IncreaseQuantity(item.GetComponent<Resource>().quantity);

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
                heldObjects[i].GetComponent<Collider>().enabled = false;
                //item.SetActive(false);
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
                //  GameObject droppedTool = Instantiate(heldTools[selectedToolNum], (transform.position + transform.forward * 2), transform.rotation);
                //  droppedTool.SetActive(true);
                //  heldTools[selectedToolNum] = null;
                //

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
                GameObject droppedItem = Instantiate(heldObjects[selectedItemNum], (transform.position + transform.forward * 2), transform.rotation);
                droppedItem.SetActive(true);
                droppedItem.GetComponent<Item>().quantity = 1;
                heldObjects[selectedItemNum].GetComponent<Item>().DecreaseQuantity();
                droppedItem.transform.parent = null;
                droppedItem.GetComponent<Rigidbody>().isKinematic = false;
                droppedItem.GetComponent<Rigidbody>().AddForce(transform.GetChild(0).forward * 500, ForceMode.Force);
                droppedItem.GetComponent<Collider>().enabled = true;
                // transform.GetChild(0).DetachChildren();
                droppedItem.layer = 0;

            }
            else
            {
                heldObjects[selectedItemNum].GetComponent<Rigidbody>().isKinematic = false;
                heldObjects[selectedItemNum].GetComponent<Rigidbody>().AddForce(transform.GetChild(0).forward * 500, ForceMode.Force);
                heldObjects[selectedItemNum].GetComponent<Collider>().enabled = true;

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
            if (i != 0)
            {
            if (heldTools[i] != null)
            {
                toolImage[i].sprite = toolSprites[heldTools[i].GetComponent<Tool>().toolID];
            }
            else
            {
                toolImage[i].sprite = toolSprites[0];
            }

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

                itemImage[i].transform.GetComponentInChildren<Text>().text = heldObjects[i].GetComponent<Resource>().quantity.ToString();

                itemImage[i].sprite = itemSprites[heldObjects[i].GetComponent<Item>().itemID];

            }
            else
            {
                itemImage[i].sprite = itemSprites[0];
                itemImage[i].transform.GetComponentInChildren<Text>().text = 0.ToString();
            }

            if (i == selectedItemNum)
            {
                itemImage[i].color = Color.blue;
            }
            else
                itemImage[i].color = Color.white;

        //    if (heldObjects[i] != null)
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

    public void UseTool(GameObject gameObj)
    {
        if (heldTools[selectedToolNum] != null)
        {
            heldTools[selectedToolNum].GetComponent<Tool>().UseTool(gameObj);
        }

    }
}

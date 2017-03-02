using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public enum ControllerInput
    {
        A,
        B,
        X,
        Y,
        LBumper,
        RBumper,
        LTrigger,
        RTrigger,
        Start,
        Back,
        LStickClick,
        RStickClick

    }
    public static PlayerInventory instance = null;
    public int money = 0;


    //  public GameObject ItemHotbar;
    public GameObject ItemHotbar;
    public GameObject book;
    public GameObject hand;

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

    public bool disableLeft;

    public float lClickTimer = 0;
    public float lClickRate = 0.5f;
    public float rClickTimer = 0;
    public float rClickRate = 1;
    private float qTimer = 0;
    public float qRate = 0.5f;
    private float eTimer = 0;
    public float eRate = 0.5f;
    public float pickupRate = 0.1f;

    public Image holdSlider;

    public Slider waterLevel;

    RaycastHit hit;
    Ray ray;

    public ControllerInput iInteract;
    public ControllerInput iPrimaryUse;
    public ControllerInput iSecondaryUse;
    public ControllerInput iPickup;
    public ControllerInput iDrop;
    public ControllerInput iSelectUp;
    public ControllerInput iSelectDown;

    // Use this for initialization
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        scrollTimer = 0.1f;

        inMenu = false;

        UpdateInventory();
        for (int i = 0; i < ItemHotbar.transform.childCount; i++)
        {
            itemText[i] = ItemHotbar.transform.GetChild(i).GetComponent<Text>();
        }

        disableLeft = true;

        lClickTimer = 0;
        rClickTimer = 0;
        qTimer = 0;
        eTimer = 0;


    }



    // Update is called once per frame
    void Update()
    {
        scrollTimer -= Time.deltaTime;
        UpdateItemMesh();
        UpdateImages();

        if (!inMenu)
        {
            if (!bookOpen)
            {
                if (heldObjects[selectedItemNum] != null && heldObjects[selectedItemNum].GetComponent<Bucket>() != null)
                {
                    waterLevel.gameObject.SetActive(true);
                    waterLevel.maxValue = heldObjects[selectedItemNum].GetComponent<Bucket>().maxWaterLevel;
                    waterLevel.value = heldObjects[selectedItemNum].GetComponent<Bucket>().currentWaterLevel;
                }
                else
                    waterLevel.gameObject.SetActive(false);

                Hotkeys();

                UpdateInventory();
                /////////////--- Q Press --- DROP
                //////////////////////////////////////////////////////

                if (Input.GetKey(KeyCode.E) || Input.GetButton("Controller_" + iInteract)) // Interact
                {
                    ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

                    if (Physics.Raycast(ray, out hit, 5))
                    {
                        switch (hit.transform.tag)
                        {
                            case "StoreItem":
                                if (hit.transform.GetComponent<StoreItem>().price <= money)
                                {
                                    eTimer += Time.deltaTime;
                                    holdSlider.fillAmount = eTimer / eRate;

                                    if (eTimer > eRate)
                                    {
                                        eTimer = 0;
                                        holdSlider.fillAmount = 0;
                                        hit.transform.GetComponent<StoreItem>().BuyObject();/////

                                    }
                                }
                                break;
                            case "Bed":
                                eTimer += Time.deltaTime;
                                holdSlider.fillAmount = eTimer / eRate;

                                if (eTimer > eRate)
                                {
                                    eTimer = 0;
                                    holdSlider.fillAmount = 0;
                                    DayNightController.instance.BedDayJump();
                                }
                                break;
                            case "Item":
                                eTimer += Time.deltaTime;
                                holdSlider.fillAmount = eTimer / pickupRate;

                                if (eTimer > pickupRate)
                                {
                                    eTimer = 0;
                                    holdSlider.fillAmount = 0;
                                    AddItem(hit.transform.gameObject);
                                }
                                break;
                            default:
                                eTimer = 0;
                                holdSlider.fillAmount = eTimer * 2;
                                break;
                        }
                    }
                    else
                    {
                        eTimer = 0;
                        holdSlider.fillAmount = eTimer * 2;
                    }
                }

                if (heldObjects[selectedItemNum] != null)
                {
                    if (Input.GetKey(KeyCode.Q) || Input.GetButton("Controller_" + iDrop)) // Drop
                    {
                        qTimer += Time.deltaTime;
                        holdSlider.fillAmount = qTimer / qRate;
                        if (qTimer > qRate)
                        {
                            qTimer = 0;
                            holdSlider.fillAmount = 0;
                            DropAllofItem();
                        }
                    }
                    if (Input.GetKeyUp(KeyCode.Q) || Input.GetButtonUp("Controller_" + iDrop))
                    {
                        qTimer = 0;
                        holdSlider.fillAmount = 0;
                        RemoveItem();
                    }


                    Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
                    if (Physics.Raycast(ray, out hit, 5))
                    {
                        switch (hit.transform.tag)
                        {
                            case "Plant":
                                if (heldObjects[selectedItemNum].GetComponent<Bucket>() != null)
                                    hit.transform.GetComponent<Plant>().highlighted = true;
                                break;
                            case "Soil":
                                if (heldObjects[selectedItemNum].GetComponent<Bucket>() != null)
                                {
                                    if (hit.transform.childCount > 0)
                                        hit.transform.GetChild(0).GetComponent<Plant>().highlighted = true;
                                }
                                break;
                        }

                    }
                    /////////////--- E Press --- INTERACT ---
                    //////////////////////////////////////////////////////

                    if (Input.GetKeyUp(KeyCode.E) || Input.GetButtonUp("Controller_" + iInteract))
                    {
                        eTimer = 0;
                        holdSlider.fillAmount = 0;
                    }

                    /////////////--- Left Click --- USE ---
                    //////////////////////////////////////////////////////

                    if ((Input.GetMouseButton(0) || Input.GetButton("Controller_" + iPrimaryUse)) && disableLeft) // Drop
                    {
                        lClickTimer += Time.deltaTime;
                        rClickTimer = 0;
                        holdSlider.fillAmount = lClickTimer / lClickRate;
                        if (lClickTimer > lClickRate)
                        {
                            heldObjects[selectedItemNum].GetComponent<Item>().PrimaryUse(Item.ClickType.Hold);
                            if (heldObjects[selectedItemNum].GetComponent<Bucket>() == null)
                            {
                                lClickTimer = 0;
                                holdSlider.fillAmount = 0;
                                disableLeft = false;
                            }
                        }
                    }

                    if (Input.GetMouseButtonUp(0) || Input.GetButtonUp("Controller_" + iPrimaryUse))
                    {
                        if (lClickTimer < lClickRate)
                        {
                            if (heldObjects[selectedItemNum] != null)
                            {
                                //  if(heldObjects[selectedItemNum].GetComponent<Hammer>() != null)
                                heldObjects[selectedItemNum].GetComponent<Item>().PrimaryUse(Item.ClickType.Single);
                            }
                        }
                        disableLeft = true;
                        holdSlider.fillAmount = 0;
                        lClickTimer = 0;
                    }

                    if (Input.GetMouseButton(1) || Input.GetButton("Controller_" + iSecondaryUse)) // Drop
                    {
                        rClickTimer += Time.deltaTime;
                        //  lClickTimer = 0;
                        holdSlider.fillAmount = rClickTimer / rClickRate;
                        if (rClickTimer > rClickRate)
                        {
                            heldObjects[selectedItemNum].GetComponent<Item>().SecondaryUse(Item.ClickType.Hold);
                        }
                    }

                    if (Input.GetMouseButtonUp(1) || Input.GetButtonUp("Controller_" + iSecondaryUse))
                    {
                        if (rClickTimer < rClickRate)
                        {
                            heldObjects[selectedItemNum].GetComponent<Item>().SecondaryUse(Item.ClickType.Single);
                        }
                        holdSlider.fillAmount = 0;
                        rClickTimer = 0;
                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Controller_" + iPrimaryUse)) // Primary Use
                {
                    Blueprint.instance.PrimaryUse();
                    bookOpen = false;
                }
                if (Input.GetMouseButtonDown(1) || Input.GetButtonDown("Controller_" + iPrimaryUse)) // Pickup
                {
                    Blueprint.instance.SecondaryUse();
                    bookOpen = false;
                }
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
                    if (heldObjects[i].GetComponent<Item>().quantity >= heldObjects[i].GetComponent<Item>().itemCap)
                        continue;

                    if (((heldObjects[i].GetComponent<Item>().quantity + item.GetComponent<Item>().quantity) - heldObjects[i].GetComponent<Item>().itemCap) > 0)
                    {
                        int difference = heldObjects[i].GetComponent<Item>().itemCap - heldObjects[i].GetComponent<Item>().quantity;
                        item.GetComponent<Item>().DecreaseQuantity(difference);
                        heldObjects[i].GetComponent<Item>().IncreaseQuantity(difference);
                        AddItem(item);
                        return true;
                    }
                    else
                        heldObjects[i].GetComponent<Item>().IncreaseQuantity(item.GetComponent<Item>().quantity);

                    if (WaveManager.instance != null)
                    {
                        if (heldObjects[i].GetComponent<UnityEngine.AI.NavMeshAgent>() != null)
                            WaveManager.instance.rabbitsLeft--;
                    }

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
                heldObjects[i].transform.localPosition = new Vector3(1.6f, -0.8f, 2);
                heldObjects[i].GetComponent<Rigidbody>().isKinematic = true;
                heldObjects[i].layer = 2;
                heldObjects[i].GetComponent<Collider>().enabled = false;

                heldObjects[i].transform.rotation = transform.GetChild(0).rotation;

                if (heldObjects[i].GetComponent<Item>().itemID == 21)
                    heldObjects[i].transform.Rotate(0, 0, -60);
                else if (heldObjects[i].GetComponent<Item>().itemID == 6)
                    heldObjects[i].transform.Rotate(-90, 80, 0);
                else
                    heldObjects[i].transform.Rotate(0, 0, 30);

                if (heldObjects[i].GetComponent<UnityEngine.AI.NavMeshAgent>() != null)
                {
                    heldObjects[i].GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
                    if (WaveManager.instance != null)
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
                GameObject droppedItem = Instantiate(heldObjects[selectedItemNum], transform.GetChild(0).position + (transform.GetChild(0).forward * 2), heldObjects[selectedItemNum].transform.rotation);
                droppedItem.SetActive(true);
                droppedItem.GetComponent<Item>().quantity = 1;
                droppedItem.transform.parent = null;
                droppedItem.GetComponent<Rigidbody>().isKinematic = false;
                droppedItem.GetComponent<Rigidbody>().AddForce(transform.GetChild(0).forward * 500, ForceMode.Force);
                droppedItem.GetComponent<Collider>().enabled = true;
                if (droppedItem.GetComponent<MeshRenderer>() != null)
                    droppedItem.GetComponent<MeshRenderer>().enabled = true;
                droppedItem.layer = 0;

                heldObjects[selectedItemNum].GetComponent<Item>().DecreaseQuantity();
                if (heldObjects[selectedItemNum].GetComponent<Item>().itemID > 10)
                    heldObjects[selectedItemNum].GetComponent<Item>().UpdateMesh();
                if (droppedItem.GetComponent<Item>().itemID > 10)
                    droppedItem.GetComponent<Item>().UpdateMesh();
            }
            else
            {
                heldObjects[selectedItemNum].transform.position = transform.GetChild(0).position + (transform.GetChild(0).forward * 2);
                //  heldObjects[selectedItemNum].transform.rotation = transform.rotation;
                heldObjects[selectedItemNum].GetComponent<Rigidbody>().isKinematic = false;
                heldObjects[selectedItemNum].GetComponent<Rigidbody>().AddForce(transform.GetChild(0).forward * 500, ForceMode.Force);
                heldObjects[selectedItemNum].GetComponent<Collider>().enabled = true;
                heldObjects[selectedItemNum].transform.parent = null;
                heldObjects[selectedItemNum].layer = 0;
                if (heldObjects[selectedItemNum].GetComponent<MeshRenderer>() != null)
                    heldObjects[selectedItemNum].GetComponent<MeshRenderer>().enabled = true;
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


    public void DestroyItem(GameObject item)
    {
        int itemDroppedNum = 0;
        for (int i = 0; i < heldObjects.Count; i++)
        {
            if (heldObjects[i] == item)
            {
                itemDroppedNum = i;
                break;
            }
        }
        Destroy(heldObjects[itemDroppedNum]);
        heldObjects[itemDroppedNum] = null;
    }


    public void DropAllofItem()
    {
        GameObject droppedItem = Instantiate(heldObjects[selectedItemNum], transform.GetChild(0).position + (transform.GetChild(0).forward * 2), heldObjects[selectedItemNum].transform.rotation);
        droppedItem.SetActive(true);
        droppedItem.GetComponent<Item>().quantity = 1;
        droppedItem.transform.parent = null;
        droppedItem.GetComponent<Rigidbody>().isKinematic = false;
        droppedItem.GetComponent<Rigidbody>().AddForce(transform.GetChild(0).forward * 500, ForceMode.Force);
        droppedItem.GetComponent<Collider>().enabled = true;
        if (droppedItem.GetComponent<MeshRenderer>() != null)
            droppedItem.GetComponent<MeshRenderer>().enabled = true;
        droppedItem.layer = 0;
        droppedItem.GetComponent<Item>().quantity = heldObjects[selectedItemNum].GetComponent<Item>().quantity;
        heldObjects[selectedItemNum].GetComponent<Item>().DecreaseQuantity();
        if (heldObjects[selectedItemNum].GetComponent<Item>().itemID > 10)
            heldObjects[selectedItemNum].GetComponent<Item>().UpdateMesh();
        if (droppedItem.GetComponent<Item>().itemID > 10)
            droppedItem.GetComponent<Item>().UpdateMesh();
        Destroy(heldObjects[selectedItemNum]);
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
                itemText[i].text = heldObjects[i].GetComponent<Item>().itemName;
                if (heldObjects[i].GetComponent<Item>().quantity > 1)
                    itemText[i].transform.GetChild(0).transform.GetComponentInChildren<Text>().text = heldObjects[i].GetComponent<Item>().quantity.ToString();
                else
                    itemText[i].transform.GetChild(0).transform.GetComponentInChildren<Text>().text = "";
            }
            else
            {
                itemText[i].transform.GetComponentInChildren<Text>().text = "-";
                itemText[i].transform.GetChild(0).transform.GetComponentInChildren<Text>().text = "";
            }
        }
    }

    void UpdateInventory()
    {
        if (scrollTimer < 0)
        {
            if ((Input.GetAxis("Mouse ScrollWheel") > 0.05f) || Input.GetButton("Controller_" + iSelectDown))
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

            if ((Input.GetAxis("Mouse ScrollWheel") < -0.05f) || Input.GetButton("Controller_" + iSelectUp))
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

    void UpdateItemMesh()
    {
        for (int i = 0; i < heldObjects.Count; i++)
        {
            if (heldObjects[i] != null)
            {
                if (i == selectedItemNum)
                {
                    ShowObject(heldObjects[i]);
                    HideObject(hand);
                }
                else
                {
                    HideObject(heldObjects[i]);
                }
            }
            else
                 if (i == selectedItemNum)
                ShowObject(hand);
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
























/////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////
////////////////////////////_████████__██████  //////////////////////////
//////////////////////////_█░░░░░░░░██_██░░░░░░█/////////////////////////
//////////////////////////█░░░░░░░░░░░█░░░░░░░░░█////////////////////////
/////////////////////////█░░░░░░░███░░░█░░░░░░░░░█///////////////////////
/////////////////////////█░░░░███░░░███░█░░░████░█///////////////////////
////////////////////////█░░░██░░░░░░░░███░██░░░░██///////////////////////
///////////////////////█░░░░░░░░░░░░░░░░░█░░░░░░░░███////////////////////
//////////////////////█░░░░░░░░░░░░░██████░░░░░████░░█///////////////////
//////////////////////█░░░░░░░░░█████░░░████░░██░░██░░█//////////////////
/////////////////////██░░░░░░░███░░░░░░░░░░█░░░░░░░░███//////////////////
///////////////////_█░░░░░░░░░░░░░░█████████░░█████████//////////////////
//////////////////█░░░░░░░░░░█████_████████_█████_█//////////////////////
//////////////////█░░░░░░░░░░█___█_████___███_█_█////////////////////////
//////////////////█░░░░░░░░░░░░█_████_████__██_██████ ///////////////////
//////////////////░░░░░░░░░░░░░█████████░░░████████░░░█//////////////////
//////////////////░░░░░░░░░░░░░░░░█░░░░░█░░░░░░░░░░░░█///////////////////
//////////////////░░░░░░░░░░░░░░░░░░░░██░░░░█░░░░░░██////////////////////
//////////////////░░░░░░░░░░░░░░░░░░██░░░░░░░███████/////////////////////
//////////////////░░░░░░░░░░░░░░░░██░░░░░░░░░░█░░░░░█////////////////////
//////////////////░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░█///////////////////
//////////////////░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░█///////////////////
//////////////////░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░█///////////////////
//////////////////░░░░░░░░░░░█████████░░░░░░░░░░░░░░██///////////////////
//////////////////░░░░░░░░░░█▒▒▒▒▒▒▒▒███████████████▒▒█//////////////////
//////////////////░░░░░░░░░█▒▒███████▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒█//////////////////
//////////////////░░░░░░░░░█▒▒▒▒▒▒▒▒▒█████████████████ //////////////////
//////////////////░░░░░░░░░░████████▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒█//////////////////
//////////////////░░░░░░░░░░░░░░░░░░██████████████████///////////////////
//////////////////░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░█///////////////////////
//////////////////██░░░░░░░░░░░░░░░░░░░░░░░░░░░██ ///////////////////////
//////////////////▓██░░░░░░░░░░░░░░░░░░░░░░░░██//////////////////////////
//////////////////▓▓▓███░░░░░░░░░░░░░░░░░░░░█////////////////////////////
//////////////////▓▓▓▓▓▓███░░░░░░░░░░░░░░░██/////////////////////////////
//////////////////▓▓▓▓▓▓▓▓▓███████████████▓▓█////////////////////////////
//////////////////▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓██//////////////////////////
//////////////////▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓█//////////////////////////
//////////////////▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓█/////////////////////////
/////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////




//-----------------------------------------MMMMMMMMM
//---------_______________________---------MMMMMMMMM
//---------| Goblin Hammer Games |---------MMMMMMMMM
//---------|    Harvest Hands    |---------MMMMMMMMM
//---------_______________________---------MMMMMMMMM
//---------_______________________---------MMMMMMMMM
//---------|    Last Modified    |---------MMMMMMMMM
//---------|      27/2/2017      |---------MMMMMMMMM
//---------|         By          |---------MMMMMMMMM
//---------|    Kayne Palmer     |---------MMMMMMMMM
//---------_______________________---------MMMMMMMMM
//-----------------------------------------MMMMMMMMM



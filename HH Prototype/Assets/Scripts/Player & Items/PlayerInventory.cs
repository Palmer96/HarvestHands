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
    public bool disableRight;

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
    //  public ControllerInput iPickup;
    public ControllerInput iDrop;
    public ControllerInput iSelectUp;
    public ControllerInput iSelectDown;

    public bool eUsed;
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
        itemText[0].color = Color.yellow;

        disableLeft = true;
        disableRight = true;

        eUsed = false;

        lClickTimer = 0;
        rClickTimer = 0;
        qTimer = 0;
        eTimer = 0;


        SaveAndLoadManager.OnSave += Save;
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

                if (Input.GetKey(KeyCode.E) || Input.GetButton("Controller_" + iInteract) || Input.GetAxis("Controller_" + iInteract) != 0) // Interact
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

                            case "Shelf":
                                if (hit.transform.GetComponent<Shelf>().storedObject != null)
                                {
                                    eTimer += Time.deltaTime;
                                    holdSlider.fillAmount = eTimer / pickupRate;

                                    if (eTimer > pickupRate)
                                    {
                                        eTimer = 0;
                                        holdSlider.fillAmount = 0;
                                        AddItem(hit.transform.GetComponent<Shelf>().TakeOutItem());
                                    }
                                }
                                break;

                            case "NoticeBoard":
                                // eTimer += Time.deltaTime;
                                // holdSlider.fillAmount = eTimer / eRate;
                                //
                                // if (eTimer > eRate)
                                // {
                                //     eTimer = 0;
                                //     holdSlider.fillAmount = 0;
                                if (!eUsed)
                                {
                                    hit.transform.GetComponent<PrototypeObjectiveBoard>().GetRandomQuest();
                                    eUsed = true;
                                }
                                // }
                                break;

                            case "CraftingBench":
                              //  eTimer += Time.deltaTime;
                              //  holdSlider.fillAmount = eTimer / eRate;
                              //
                              //  if (eTimer > eRate)
                              //  {
                              //      eTimer = 0;
                              //      holdSlider.fillAmount = 0;
                            //        if (!eUsed)
                            //        {
                                        CraftingMenu.instance.ActivateMenu();
                             //           eUsed = true;
                            //        }
                           //     }
                                break;

                            case "Livestock":
                                eTimer += Time.deltaTime;
                                holdSlider.fillAmount = eTimer / eRate;

                                if (eTimer > eRate)
                                {
                                    eTimer = 0;
                                    holdSlider.fillAmount = 0;
                                    hit.transform.GetComponent<Livestock>().Interact();
                                }
                                break;

                            case "SellZone":
                                eTimer += Time.deltaTime;
                                holdSlider.fillAmount = eTimer / eRate;

                                if (eTimer > eRate)
                                {
                                    eTimer = 0;
                                    holdSlider.fillAmount = 0;
                                    hit.transform.GetComponent<Livestock>().Interact();
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
                /////////////--- E Press --- INTERACT ---
                //////////////////////////////////////////////////////

                if (Input.GetKeyUp(KeyCode.E) || Input.GetButtonUp("Controller_" + iInteract))
                {
                    eTimer = 0;
                    holdSlider.fillAmount = 0;
                    eUsed = false;
                }

                if (heldObjects[selectedItemNum] != null)
                {
                    if (Input.GetKey(KeyCode.Q) || Input.GetButton("Controller_" + iDrop) || Input.GetAxis("Controller_" + iDrop) != 0) // Drop
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
                    if (Input.GetKeyUp(KeyCode.Q) || Input.GetButtonUp("Controller_" + iDrop))// || Input.GetAxis("Controller_" + iDrop) == 0)
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
                                    //   if (hit.transform.childCount > 0)
                                    //       hit.transform.GetChild(0).GetComponent<Plant>().highlighted = true;
                                    for (int i = 0; i < hit.transform.childCount; ++i)
                                    {
                                        if (hit.transform.GetChild(i).GetComponent<Plant>() != null)
                                        {
                                            hit.transform.GetChild(i).GetComponent<Plant>().highlighted = true;
                                        }
                                    }
                                }
                                break;
                            case "Shelf":
                                if (hit.transform.GetComponent<Shelf>() != null)
                                {
                                    hit.transform.GetComponent<Shelf>().highlighted = true;
                                }
                                break;
                        }

                    }


                    /////////////--- Left Click --- USE ---
                    //////////////////////////////////////////////////////

                    if ((Input.GetMouseButton(0) || Input.GetButton("Controller_" + iPrimaryUse) || Input.GetAxis("Controller_" + iPrimaryUse) != 0) && disableLeft) // Drop
                    {
                        lClickTimer += Time.deltaTime;
                        rClickTimer = 0;
                        holdSlider.fillAmount = lClickTimer / lClickRate;
                        if (lClickTimer > lClickRate)
                        {
                            if (Physics.Raycast(ray, out hit, 5))
                            {

                                if (hit.transform.CompareTag("Shelf"))
                                {
                                    hit.transform.GetComponent<Shelf>().StoreItem(heldObjects[selectedItemNum]);
                                    lClickTimer = 0;
                                    holdSlider.fillAmount = 0;
                                }
                                else
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
                        }
                    }

                    if (Input.GetMouseButtonUp(0) || Input.GetButtonUp("Controller_" + iPrimaryUse))
                    {
                        if (disableLeft)
                        {
                            if (lClickTimer < lClickRate)
                            {
                                if (heldObjects[selectedItemNum] != null)
                                {
                                    if (heldObjects[selectedItemNum].GetComponent<Hammer>() != null)
                                    {

                                        if (heldObjects[selectedItemNum].GetComponent<Hammer>() != null)
                                            heldObjects[selectedItemNum].GetComponent<Hammer>().HammerUp();
                                    }
                                    else
                                        heldObjects[selectedItemNum].GetComponent<Item>().PrimaryUse(Item.ClickType.Single);

                                }
                            }
                        }
                        disableLeft = true;
                        holdSlider.fillAmount = 0;
                        lClickTimer = 0;
                    }

                    if ((Input.GetMouseButton(1) || Input.GetButton("Controller_" + iSecondaryUse) || Input.GetAxis("Controller_" + iSecondaryUse) != 0) && disableRight) // Drop
                    {
                        rClickTimer += Time.deltaTime;
                        //  lClickTimer = 0;
                        holdSlider.fillAmount = rClickTimer / rClickRate;
                        if (rClickTimer > rClickRate)
                        {
                            heldObjects[selectedItemNum].GetComponent<Item>().SecondaryUse(Item.ClickType.Hold);
                            if (heldObjects[selectedItemNum].GetComponent<Bucket>() == null)
                            {
                                lClickTimer = 0;
                                holdSlider.fillAmount = 0;
                                disableRight = false;
                            }
                        }
                    }

                    if (Input.GetMouseButtonUp(1) || Input.GetButtonUp("Controller_" + iSecondaryUse))// || (Input.GetAxis("Controller_" + iSecondaryUse) <= 0.1f && Input.GetAxis("Controller_" + iSecondaryUse) != 0)) 
                    {
                        if (rClickTimer < rClickRate)
                        {
                            heldObjects[selectedItemNum].GetComponent<Item>().SecondaryUse(Item.ClickType.Single);
                        }
                        disableRight = true;
                        holdSlider.fillAmount = 0;
                        rClickTimer = 0;
                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Controller_" + iPrimaryUse) || Input.GetAxis("Controller_" + iPrimaryUse) > 0) // Primary Use
                {
                    Blueprint.instance.PrimaryUse();
                    bookOpen = false;
                }
                if (Input.GetMouseButtonDown(1) || Input.GetButtonDown("Controller_" + iPrimaryUse) || Input.GetAxis("Controller_" + iPrimaryUse) > 0) // Pickup
                {
                    Blueprint.instance.SecondaryUse();
                    bookOpen = false;
                }
            }
        }
    }


    public bool AddItem(GameObject item)
    {
        if (item == null)
            return false;
        if (item.GetComponent<Item>() == null)
            return false;


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
                        ScreenMessage.instance.CreateMessage("Added " + difference + " " + item.GetComponent<Item>().itemName);
                        AddItem(item);
                        return true;
                    }
                    else
                    {
                        heldObjects[i].GetComponent<Item>().IncreaseQuantity(item.GetComponent<Item>().quantity);
                        ScreenMessage.instance.CreateMessage("Added " + item.GetComponent<Item>().quantity + " " + item.GetComponent<Item>().itemName);
                    }

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

        if (heldObjects[selectedItemNum] == null)
        {
            heldObjects[selectedItemNum] = item;
            heldObjects[selectedItemNum].transform.SetParent(transform.GetChild(0));
            heldObjects[selectedItemNum].transform.localPosition = new Vector3(1.6f, -0.8f, 2);
            heldObjects[selectedItemNum].GetComponent<Rigidbody>().isKinematic = true;
            heldObjects[selectedItemNum].layer = 2;
            heldObjects[selectedItemNum].GetComponent<Collider>().enabled = false;

            heldObjects[selectedItemNum].transform.rotation = transform.GetChild(0).rotation;

            if (heldObjects[selectedItemNum].GetComponent<Item>().itemID == 21)
                heldObjects[selectedItemNum].transform.Rotate(0, 0, -60);
            else if (heldObjects[selectedItemNum].GetComponent<Item>().itemID == 6)
                heldObjects[selectedItemNum].transform.Rotate(-160, -10, 160);
            else
                heldObjects[selectedItemNum].transform.Rotate(0, 0, 30);

            if (heldObjects[selectedItemNum].GetComponent<UnityEngine.AI.NavMeshAgent>() != null)
            {
                heldObjects[selectedItemNum].GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
                if (WaveManager.instance != null)
                    WaveManager.instance.rabbitsLeft--;
            }
            heldObjects[selectedItemNum].GetComponent<Item>().beingHeld = true;
            ScreenMessage.instance.CreateMessage("Added " + item.GetComponent<Item>().quantity + " " + item.GetComponent<Item>().itemName);
            return true;
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
                    heldObjects[i].transform.Rotate(-160, -10, 160);
                else
                    heldObjects[i].transform.Rotate(0, 0, 30);

                if (heldObjects[i].GetComponent<UnityEngine.AI.NavMeshAgent>() != null)
                {
                    heldObjects[i].GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
                    if (WaveManager.instance != null)
                        WaveManager.instance.rabbitsLeft--;
                }
                heldObjects[i].GetComponent<Item>().beingHeld = true;
                ScreenMessage.instance.CreateMessage("Added " + item.GetComponent<Item>().quantity + " " + item.GetComponent<Item>().itemName);
                return true;
            }
        }
        ScreenMessage.instance.CreateMessage("Inventory is full");
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

    public void RemoveItem(int inventorySlot)
    {
        if (heldObjects[inventorySlot] != null)
        {
            if (heldObjects[inventorySlot].GetComponent<Item>().quantity > 1)
            {
                GameObject droppedItem = Instantiate(heldObjects[inventorySlot], transform.GetChild(0).position + (transform.GetChild(0).forward * 2), heldObjects[inventorySlot].transform.rotation);
                droppedItem.SetActive(true);
                droppedItem.GetComponent<Item>().quantity = 1;
                droppedItem.transform.parent = null;
                droppedItem.GetComponent<Rigidbody>().isKinematic = false;
                droppedItem.GetComponent<Rigidbody>().AddForce(transform.GetChild(0).forward * 500, ForceMode.Force);
                droppedItem.GetComponent<Collider>().enabled = true;
                if (droppedItem.GetComponent<MeshRenderer>() != null)
                    droppedItem.GetComponent<MeshRenderer>().enabled = true;
                droppedItem.layer = 0;

                heldObjects[inventorySlot].GetComponent<Item>().DecreaseQuantity();
                if (heldObjects[inventorySlot].GetComponent<Item>().itemID > 10)
                    heldObjects[inventorySlot].GetComponent<Item>().UpdateMesh();
                if (droppedItem.GetComponent<Item>().itemID > 10)
                    droppedItem.GetComponent<Item>().UpdateMesh();
            }
            else
            {
                heldObjects[inventorySlot].transform.position = transform.GetChild(0).position + (transform.GetChild(0).forward * 2);
                //  heldObjects[selectedItemNum].transform.rotation = transform.rotation;
                heldObjects[inventorySlot].GetComponent<Rigidbody>().isKinematic = false;
                heldObjects[inventorySlot].GetComponent<Rigidbody>().AddForce(transform.GetChild(0).forward * 500, ForceMode.Force);
                heldObjects[inventorySlot].GetComponent<Collider>().enabled = true;
                heldObjects[inventorySlot].transform.parent = null;
                heldObjects[inventorySlot].layer = 0;
                if (heldObjects[inventorySlot].GetComponent<MeshRenderer>() != null)
                    heldObjects[inventorySlot].GetComponent<MeshRenderer>().enabled = true;
                if (heldObjects[inventorySlot].GetComponent<Item>().itemID < 10)
                    heldObjects[inventorySlot].GetComponent<Item>().UpdateMesh();
                heldObjects[inventorySlot] = null;
            }
        }
    }

    public void RemoveItem(GameObject objectToRemove)
    {
        for (int i = 0; i < heldObjects.Count; ++i)
        {
            if (heldObjects[i] == objectToRemove)
            {
                RemoveItem(i);
            }
        }
    }

    //Helper function for readding items after loading
    public bool AddItemInSlot(GameObject item, int slot)
    {
        //if (heldObjects[slot] == null)
        //{
        heldObjects[slot] = item;
        heldObjects[slot].transform.SetParent(transform.GetChild(0));
        heldObjects[slot].transform.localPosition = new Vector3(1.6f, -0.8f, 2);
        heldObjects[slot].GetComponent<Rigidbody>().isKinematic = true;
        heldObjects[slot].layer = 2;
        heldObjects[slot].GetComponent<Collider>().enabled = false;

        heldObjects[slot].transform.rotation = transform.GetChild(0).rotation;

        if (heldObjects[slot].GetComponent<Item>().itemID == 21)
            heldObjects[slot].transform.Rotate(0, 0, -60);
        else if (heldObjects[slot].GetComponent<Item>().itemID == 6)
            heldObjects[slot].transform.Rotate(-160, -10, 160);
        else
            heldObjects[slot].transform.Rotate(0, 0, 30);

        item.GetComponent<Item>().beingHeld = true;

        if (heldObjects[slot].GetComponent<UnityEngine.AI.NavMeshAgent>() != null)
        {
            heldObjects[slot].GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
            if (WaveManager.instance != null)
                WaveManager.instance.rabbitsLeft--;
        }

        return true;
        //}
        //else
        //{
        //    Debug.Log("PlayerInventory.AddItemInSlot failed, slot " + slot.ToString() + " is full");
        //    return false;
        //}
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
        int num = selectedItemNum;

        if (heldObjects[num] != null)
        {
            itemText[0].text = heldObjects[num].GetComponent<Item>().itemName;
            if (heldObjects[num].GetComponent<Item>().quantity > 1)
                itemText[0].transform.GetChild(0).transform.GetComponentInChildren<Text>().text = heldObjects[num].GetComponent<Item>().quantity.ToString();
            else
                itemText[0].transform.GetChild(0).transform.GetComponentInChildren<Text>().text = "";
        }
        else
        {
            itemText[0].text = "-";
            itemText[0].transform.GetChild(0).transform.GetComponentInChildren<Text>().text = "";
        }
        ////////////////////////////////////////////
        for (int i = 1; i < itemText.Capacity; i++)
        {
            num = selectedItemNum + i;
            if (num >= heldObjects.Capacity)
                num -= heldObjects.Capacity;
            else if (num <= 0)
                num += heldObjects.Capacity;

            if (heldObjects[num] != null)
            {
                itemText[i].text = heldObjects[num].GetComponent<Item>().itemName;
                if (heldObjects[num].GetComponent<Item>().quantity > 1)
                    itemText[i].transform.GetChild(0).transform.GetComponentInChildren<Text>().text = heldObjects[num].GetComponent<Item>().quantity.ToString();
                else
                    itemText[i].transform.GetChild(0).transform.GetComponentInChildren<Text>().text = "";
            }
            else
            {
                itemText[i].text = "-";
                itemText[i].transform.GetChild(0).transform.GetComponentInChildren<Text>().text = "";
            }
        }

        //    for (int i = 0 ; i < itemText.Capacity; i++)
        //    {
        //        int num = selectedItemNum;
        //        if (num <= 0)
        //            num += heldObjects.Capacity;
        //        else if (num >= heldObjects.Capacity)
        //            num -= heldObjects.Capacity;
        //
        //        if (heldObjects[num] != null)
        //            itemText[i].text = heldObjects[num].GetComponent<Item>().itemName;
        //        else
        //            itemText[i].text = "";
        //    }


        //for (int i = 0; i < itemText.Count; i++)
        //{
        //    if (heldObjects[i] != null)
        //    {
        //        itemText[i].text = heldObjects[i].GetComponent<Item>().itemName;
        //        if (heldObjects[i].GetComponent<Item>().quantity > 1)
        //            itemText[i].transform.GetChild(0).transform.GetComponentInChildren<Text>().text = heldObjects[i].GetComponent<Item>().quantity.ToString();
        //        else
        //            itemText[i].transform.GetChild(0).transform.GetComponentInChildren<Text>().text = "";
        //    }
        //    else
        //    {
        //        itemText[i].transform.GetComponentInChildren<Text>().text = "-";
        //        itemText[i].transform.GetChild(0).transform.GetComponentInChildren<Text>().text = "";
        //    }
        //}



        //  for (int i = 0; i < heldObjects.Count; i++)
        //  {
        //  
        //      if (i == selectedItemNum)
        //      {
        //          itemText[i].color = Color.yellow;
        //          itemText[i].transform.GetChild(1).GetComponent<Image>().color = Color.yellow;
        //      }
        //      else
        //      {
        //          itemText[i].color = Color.white;
        //          itemText[i].transform.GetChild(1).GetComponent<Image>().color = Color.grey;
        //      }
        //  
        //  
        //      if (heldObjects[i] != null)
        //      {
        //          itemText[i].text = heldObjects[i].GetComponent<Item>().itemName;
        //          if (heldObjects[i].GetComponent<Item>().quantity > 1)
        //              itemText[i].transform.GetChild(0).transform.GetComponentInChildren<Text>().text = heldObjects[i].GetComponent<Item>().quantity.ToString();
        //          else
        //              itemText[i].transform.GetChild(0).transform.GetComponentInChildren<Text>().text = "";
        //      }
        //      else
        //      {
        //          itemText[i].transform.GetComponentInChildren<Text>().text = "-";
        //          itemText[i].transform.GetChild(0).transform.GetComponentInChildren<Text>().text = "";
        //      }
        //  }
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
                if (selectedItemNum < heldObjects.Capacity - 1)
                {
                    selectedItemNum++;
                    scrollTimer = 0.1f;
                }
                else
                    selectedItemNum = 0;
            }
            oldnum = selectedItemNum;
        }

        // if (scrollTimer < 0)
        // {
        //     if ((Input.GetAxis("Mouse ScrollWheel") > 0.05f) || Input.GetButton("Controller_" + iSelectDown))
        //     {
        //
        //         if (selectedItemNum > 0)
        //         {
        //             selectedItemNum--;
        //             scrollTimer = 0.1f;
        //         }
        //         else
        //         {
        //             selectedItemNum = heldObjects.Capacity - 1;
        //         }
        //     }
        //
        //     if ((Input.GetAxis("Mouse ScrollWheel") < -0.05f) || Input.GetButton("Controller_" + iSelectUp))
        //     {
        //         if (selectedItemNum < heldObjects.Capacity - 1)
        //         {
        //             selectedItemNum++;
        //             scrollTimer = 0.1f;
        //         }
        //         else
        //             selectedItemNum = 0;
        //     }
        //     oldnum = selectedItemNum;
        // }
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

    public virtual void Save()
    {
        SaveAndLoadManager.instance.saveData.playerSaveData = new PlayerSave(this);
    }

    void OnDestroy()
    {
        SaveAndLoadManager.OnSave -= Save;
    }
}


[System.Serializable]
public class PlayerSave
{
    int money;
    float posX;
    float posY;
    float posZ;
    float rotX;
    float rotY;
    float rotZ;
    float rotW;

    float camrotX;
    float camrotY;
    float camrotZ;
    float camrotW;

    public PlayerSave(PlayerInventory playerInventory)
    {
        money = playerInventory.money;
        posX = playerInventory.transform.position.x;
        posY = playerInventory.transform.position.y;
        posZ = playerInventory.transform.position.z;
        rotX = playerInventory.transform.rotation.x;
        rotY = playerInventory.transform.rotation.y;
        rotZ = playerInventory.transform.rotation.z;
        rotW = playerInventory.transform.rotation.w;

        camrotX = playerInventory.transform.GetChild(0).localRotation.x;
        camrotY = playerInventory.transform.GetChild(0).rotation.y;
        camrotZ = playerInventory.transform.GetChild(0).rotation.z;
        camrotW = playerInventory.transform.GetChild(0).rotation.w;

        for (int i = 0; i < playerInventory.heldObjects.Count; ++i)
        {
            if (playerInventory.heldObjects[i] == null)
                continue;
            Item item = playerInventory.heldObjects[i].GetComponent<Item>();
            if (item == null)
            {
                Debug.Log("PlayerInventory trying to save held item without Item script?, Item.name = " + item.name);
                continue;
            }
        }
    }

    public GameObject LoadObject()
    {

        if (PlayerInventory.instance != null)
        {
            PlayerInventory.instance.money = money;
            PlayerInventory.instance.transform.position = new Vector3(posX, posY, posZ);
            //  PlayerInventory.instance.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().disabled = true;
            //  PlayerInventory.instance.transform.rotation = Quaternion.identity;
            // PlayerInventory.instance.transform.Rotate(0, rotY, 0);

            //  PlayerInventory.instance.transform.GetChild(0).rotation = Quaternion.identity;
            //  PlayerInventory.instance.transform.GetChild(0).Rotate(camrotX, 0, 0);


            PlayerInventory.instance.transform.rotation = new Quaternion(rotX, rotY, rotZ, rotW);
            PlayerInventory.instance.transform.GetChild(0).localRotation = new Quaternion(camrotX, 0, 0, 0);


        }
        else
        {
            Debug.Log("Failed to load player inventory, PlayerInventory.instance = " + PlayerInventory.instance);
        }
        return null;
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




//-----------------------------------------//
//---------_______________________---------//
//---------| Goblin Hammer Games |---------//
//---------|    Harvest Hands    |---------//
//---------_______________________---------//
//---------_______________________---------//
//---------|    Last Modified    |---------//
//---------|      27/2/2017      |---------//
//---------|         By          |---------//
//---------|    Kayne Palmer     |---------//
//---------_______________________---------//
//-----------------------------------------//



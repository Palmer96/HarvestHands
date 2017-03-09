using UnityEngine;
using System.Collections;

public class HandTool : MonoBehaviour
{

    public GameObject heldItem;
    public float rayMaxDist;
    public float constructionMaxDist;
    public float constructionGridDist;


    public bool ConstructionMode;
    public GameObject Dirt;

    public bool usingHand;

    public string Interact;
    // Use this for initialization
    void Start()
    {
        ConstructionMode = false;
        Interact = GetComponent<PlayerInventory>().iInteract.ToString();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E) || Input.GetButton("Controller_" + Interact))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
            if (Physics.Raycast(ray, out hit, rayMaxDist))
            {
                if (hit.transform.tag == "NoticeBoard")
                {
                    hit.transform.GetComponent<PrototypeObjectiveBoard>().GetRandomQuest();
                }
                if (hit.transform.GetComponent<VIDE_Assign>())
                {
                    //Lets grab the NPC's DialogueAssign script... if there's any
                    VIDE_Assign assigned;
                    if (hit.collider.GetComponent<VIDE_Assign>() != null)
                        assigned = hit.collider.GetComponent<VIDE_Assign>();
                    else return;

                    if (!Conversation.instance.dialogue.isLoaded)
                    {
                        //Check if have quest to talk to NPC, returns -1 if no
                        int startNode = PrototypeQuestManager.instance.CheckTalkChat(hit.transform.GetComponent<NPC>().npcName);
                        //else check if npc has new potential quest
                        if (startNode == -1)
                        {
                            NPC npc = hit.transform.GetComponent<NPC>();
                            if (npc != null)
                            {
                                npc.CheckForNewPotentialQuests();
                                npc.AcceptQuest();
                                startNode = PrototypeQuestManager.instance.CheckTalkChat(npc.npcName);
                            }
                        }

                        //... and use NPC's DialogueAssign to begin the conversation
                        Conversation.instance.BeginConversation(assigned, startNode);
                    }
                    else
                    {
                        //If conversation already began, let's just progress through it
                        Conversation.instance.NextNode();
                    }
                }
                if (hit.transform.tag == "CraftingBenchButton")
                {
                    //hit.transform.GetComponent<CraftingBenchButton>().ActivateButton();
                    CraftingMenu.instance.ActivateMenu();
                }
                if (hit.transform.tag == "CraftingBench")
                {
                    //hit.transform.GetComponent<CraftingBench>().MakeItem();
                    CraftingMenu.instance.ActivateMenu();
                }
                if (hit.transform.tag == "Livestock")
                {
                    hit.transform.GetComponent<Livestock>().Interact();
                }
              
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Inside handtool F Pressed");
            RaycastHit hit;
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
            if (Physics.Raycast(ray, out hit, rayMaxDist))
            {
                if (hit.transform.tag == "CraftingBench")
                {
                    hit.transform.GetComponent<CraftingBench>().NextRecipeChoice();
                }
            }
        }
    }
    //       if (Input.GetMouseButtonDown(0))
    //       {
    //           RaycastHit hit;
    //           Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
    //
    //           if (Physics.Raycast(ray, out hit, rayMaxDist))
    //           {
    //               Debug.Log(hit.transform.name);
    //               if (hit.transform.CompareTag("StoreItem"))
    //               {
    //                   hit.transform.GetComponent<StoreItem>().BuyObject();
    //
    //               }
    //
    //               else if (!PlayerInventory.instance.usingTools)
    //               {
    //                  // if ((PlayerInventory.instance.selectedToolNum != 0))
    //                   {
    //                       switch (hit.transform.tag)
    //                       {
    //                           case "Tool":
    //                               Debug.Log("Tool");
    //                               PlayerInventory.instance.AddTool(hit.transform.gameObject);
    //                               //PickUp(hit.transform.gameObject);
    //                               break;
    //                           case "Item":
    //                               PlayerInventory.instance.AddItem(hit.transform.gameObject);
    //                               //(hit.transform.gameObject);
    //                               break;
    //                               // case "StoreItem":
    //                               //     hit.transform.GetComponent<StoreItem>().BuyObject();
    //                              // break;
    //                           case "Bed":
    //                               DayNightController.instance.DayJump();
    //                               break;
    //
    //                           default:
    //                               PlayerInventory.instance.UseItem();// hit.transform.gameObject);
    //                               break;
    //
    //                       }
    //                   }
    //               }
    //               else
    //                   if (PlayerInventory.instance.selectedToolNum != 0)
    //                   PlayerInventory.instance.UseTool();
    //               else
    //                   if (hit.transform.CompareTag("Item") || hit.transform.CompareTag("Tool") || hit.transform.CompareTag("Rabbit"))
    //                   PlayerInventory.instance.UseTool(hit.transform.gameObject);
    //
    //           }
    //
    //
    //       }
    //       if (Input.GetMouseButtonDown(1))// && heldItem)
    //       {
    //           if (ConstructionMode)
    //               ConstructionCancel();
    //           else
    //               if (!PlayerInventory.instance.usingTools)
    //               PlayerInventory.instance.RemoveItem();
    //           else
    //           {
    //               if (PlayerInventory.instance.selectedToolNum != 0)
    //                   PlayerInventory.instance.RemoveTool();
    //               else
    //                   PlayerInventory.instance.heldTools[0].GetComponent<Hand>().Throw();
    //           }
    //
    //           //Throw();
    //       }
    //
    //      if (ConstructionMode)
    //      {
    //          RaycastHit hit;
    //          Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
    //          if (Physics.Raycast(ray, out hit, constructionMaxDist))
    //          {
    //              heldItem.transform.position = GridPos(new Vector3(hit.point.x, hit.point.y + 0.25f, hit.point.z));
    //              heldItem.transform.up = hit.normal;
    //          }
    //          else if (Physics.Raycast(heldItem.transform.position, -transform.up, out hit, constructionMaxDist))
    //          {
    //              heldItem.transform.position = GridPos(new Vector3(hit.point.x, hit.point.y + 0.25f, hit.point.z));
    //              heldItem.transform.up = hit.normal;
    //          }
    //      }
    //  }
    //  Vector3 GridPos(Vector3 pos)
    //  {
    //      float x = pos.x;
    //      float z = pos.z;
    //
    //      x = (Mathf.Round(x / constructionGridDist)) * constructionGridDist;
    //      z = (Mathf.Round(z / constructionGridDist)) * constructionGridDist;
    //
    //      return new Vector3(x, pos.y, z);
    //  }


    //   public void PickUp(GameObject item)
    //   {
    //       if (!heldItem)
    //       {
    //           heldItem = item;
    //           heldItem.transform.SetParent(transform.GetChild(0));
    //           heldItem.transform.localPosition = new Vector3(1, 0, 2);
    //           heldItem.GetComponent<Rigidbody>().isKinematic = true;
    //
    //           heldItem.layer = 2;
    //
    //           if (heldItem.GetComponent<UnityEngine.AI.NavMeshAgent>() != null)
    //           {
    //               //Destroy(heldItem.GetComponent<NavMeshAgent>());
    //               // heldItem.GetComponent<NavMeshAgent>().updatePosition = false;
    //               // heldItem.GetComponent<NavMeshAgent>().updateRotation = false;
    //               heldItem.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
    //           }
    //       }
    //   }
    void ConstructionPlace()
    {
        if (heldItem.GetComponent<Construct>().canBuild)
        {
            heldItem.GetComponent<Construct>().Place();
            ConstructionMode = false;
            //       Drop();
        }
    }

    //   void Dig(Vector3 pos)
    //   {
    //       Instantiate(Dirt, pos, transform.rotation);
    //   }

    void ConstructionCancel()
    {
        ConstructionMode = false;
        transform.GetChild(0).DetachChildren();
        heldItem.layer = 0;
        heldItem = null;
    }

    //   void Drop()
    //   {
    //       if (heldItem)
    //       {
    //           heldItem.GetComponent<Rigidbody>().isKinematic = false;
    //           transform.GetChild(0).DetachChildren();
    //           heldItem.layer = 0;
    //
    //
    //           heldItem = null;
    //       }
    //   }
    //  void Throw()
    //  {
    //      if (heldItem)
    //      {
    //          heldItem.GetComponent<Rigidbody>().isKinematic = false;
    //          heldItem.GetComponent<Rigidbody>().AddForce(transform.GetChild(0).forward * 500, ForceMode.Force);
    //          transform.GetChild(0).DetachChildren();
    //          heldItem.layer = 0;
    //
    //          heldItem = null;
    //      }
    //  }
}

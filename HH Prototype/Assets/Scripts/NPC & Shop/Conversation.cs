using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conversation : MonoBehaviour
{
    public static Conversation instance = null;
    public VIDE_Data dialogue; //Reference to conversation info
    GameObject otherObj; //What you're talking to

    public UnityEngine.UI.Text npcText;
    public UnityEngine.UI.Text npcName;
    public UnityEngine.UI.Text playerText;
    public GameObject itemText;
    public GameObject uiContainer;

    bool animatingText = false; //Is text currently animating

    //We'll be using this to store the current player dialogue options
    private List<UnityEngine.UI.Text> currentOptions = new List<UnityEngine.UI.Text>();


    public Color highlightedOptionColor = Color.green;
    public Color unhighlightedOptionColor = Color.black;

    public KeyCode GoUpOptionKey = KeyCode.W;
    public KeyCode GoDownOptionKey = KeyCode.S;

    private float changeTimer = 0.2f;
    private float changeRate = 0.2f;

    // Use this for initialization
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        dialogue = gameObject.AddComponent<VIDE_Data>();
    }

    // Update is called once per frame
    void Update()
    {
        var data = dialogue.nodeData;

        //disable the entire UI if there aren't any loaded conversations
        if (!dialogue.isLoaded)
        {
            uiContainer.SetActive(false);
        }
        else
        {
            uiContainer.SetActive(true);

            //Player-NPC conversation text will be visible depending on whose turn it is
            playerText.transform.parent.gameObject.SetActive(data.currentIsPlayer);
            npcText.transform.parent.gameObject.SetActive(!data.currentIsPlayer);

            //Color the Player options. Blue for the selected one
            for (int i = 0; i < currentOptions.Count; i++)
            {
                currentOptions[i].color = unhighlightedOptionColor;
                if (i == data.selectedOption) currentOptions[i].color = highlightedOptionColor;
            }

            changeTimer -= Time.deltaTime;

            //Scroll through Player dialogue options
            if (!data.pausedAction)
            {
                if (Input.GetKeyDown(GoDownOptionKey) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("Vertical") < -0.2f)
                {
                    if (changeTimer < 0)
                        if (data.selectedOption < currentOptions.Count - 1)
                            data.selectedOption++;
                        else
                            data.selectedOption = 0;
                    changeTimer = changeRate;
                }
            }
            if (Input.GetKeyDown(GoUpOptionKey) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("Vertical") > 0.2f)
            {
                if (changeTimer < 0)
                {

                    if (data.selectedOption > 0)
                        data.selectedOption--;
                    else
                        data.selectedOption = currentOptions.Count - 1;
                    changeTimer = changeRate;
                }
            }


        }
    }

    public void BeginConversation(VIDE_Assign diagToLoad, int startNode = -1)
    {
        //First step is to call BeginDialogue, passing the required VIDE_Assign component 
        //This will store the first Node data in dialogue.nodeData
        dialogue.BeginDialogue(diagToLoad);
        PlayerInventory.instance.transform.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;

        //if given a node to start on
        if (startNode != -1)
            dialogue.SetNode(startNode);

        var data = dialogue.nodeData;

        //Safety check in case a null dialogue was sent
        if (dialogue.assigned == null)
        {
            dialogue.EndDialogue();
            PlayerInventory.instance.transform.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
            return;
        }

        //Let's clean the NPC text variables
        npcText.text = "";
        npcName.text = "";

        ////Let's specifically check for dynamic text change
        //if (!data.currentIsPlayer && data.extraData == "itemLookUp")
        //    ItemLookUp(data);

        //Everytime dialogue.nodeData gets updated, we update our UI with the new data
        UpdateUI();
    }

    //This will handle what happens when we want next message to appear 
    //(Also called by examplePlayer script)
    public void NextNode()
    {
        var data = dialogue.nodeData;

        //Let's not go forward if text is currently being animated, but let's speed it up.
        if (animatingText) { animatingText = false; return; }

        ////Check to see if there's extraData and if so, we do stuff
        //if (!data.currentIsPlayer && data.extraData != "" && !data.pausedAction)
        //{
        //    bool needsReturn = DoAction(data);
        //    if (needsReturn)
        //    {
        //        UpdateUI();
        //        return;
        //    }
        //}

        ////Let's specifically check for dynamic text change
        //if (!data.currentIsPlayer && data.extraData == "itemLookUp" && !data.pausedAction)
        //   ItemLookUp(data);

        //This will update the dialogue.nodeData with the next Node's data
        dialogue.Next();

        UpdateUI();
    }


    public void UpdateUI()
    {
        //VIDE_Data.nodeData is all we need to retrieve data
        var data = dialogue.nodeData;

        //VIDE provides a bool variable that will turn TRUE when we attempt to call Next() on a node that leads to nowhere.
        //You are free to end the conversation whenever you desire. Just read the variable and call EndDialogue() on the VIDE_Data.
        if (data.isEnd)
        {
            //This is called when we have reached the end of the conversation
            dialogue.EndDialogue(); //VIDE_Data will get reset along with nodeData.
            PlayerInventory.instance.transform.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
            return;
        }

        //If this new Node is a Player Node, set the selectable comments offered by the Node
        if (data.currentIsPlayer)
        {
            SetOptions(data.playerComments);
        }
        //If it's an NPC Node, let's just update NPC's text
        else
        {
            if (data.npcComment.Length > 0)
                if (npcText.text != data.npcComment[data.npcCommentIndex])
                {
                    npcText.text = "";
                    StartCoroutine(AnimateText());
                }

            npcName.text = data.tag;
            //if (otherObj != null)
            //    if (otherObj.GetComponent<NPC>() != null)
            //        npcName.text = otherObj.GetComponent<NPC>().npcName;
        }
    }

    //This uses the returned string[] from nodeData.playerComments to create the UIs for each comment
    //It first cleans, then it instantiates new options
    public void SetOptions(string[] opts)
    {
        //Destroy the current options
        foreach (UnityEngine.UI.Text op in currentOptions)
            Destroy(op.gameObject);

        //Clean the variable
        currentOptions = new List<UnityEngine.UI.Text>();

        //Create the options
        for (int i = 0; i < opts.Length; i++)
        {
            //This is just one way of creating endless options for Unity's UI class
            //Normally, you'd have an absolute number of options and you wouldn't have the need of doing this
            GameObject newOp = Instantiate(playerText.gameObject, playerText.transform.position, Quaternion.identity) as GameObject;
            newOp.SetActive(true);
            newOp.transform.SetParent(playerText.transform.parent, true);
            newOp.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 20 - (20 * i));
            newOp.GetComponent<UnityEngine.UI.Text>().text = opts[i];
            currentOptions.Add(newOp.GetComponent<UnityEngine.UI.Text>());
        }
    }

    //This will replace any "[NAME]" with the name of the gameobject holding the VIDE_Assign
    void ItemLookUp(VIDE_Data.NodeData data)
    {
        if (data.npcCommentIndex == 0)
        {
            data.npcComment[data.npcCommentIndex] = data.npcComment[data.npcCommentIndex].Replace("[NAME]", dialogue.assigned.gameObject.name);
        }
    }

    //Very simple text animation, not optimal
    //Use StringBuilder for better performance
    public IEnumerator AnimateText()
    {

        var data = dialogue.nodeData;
        animatingText = true;
        string c = data.npcComment[data.npcCommentIndex];

        if (!data.currentIsPlayer)
        {
            while (npcText.text != c)
            {
                if (!animatingText) break;
                string letterToAdd = c[npcText.text.Length].ToString();
                npcText.text += letterToAdd; //Actual text updates here
                yield return new WaitForSeconds(0.02f);
            }
        }

        npcText.text = data.npcComment[data.npcCommentIndex]; //And here		
        animatingText = false;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conversation : MonoBehaviour
{
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

    
    public Color highlightedOptionColor = Color.black;
    public Color unhighlightedOptionColor = Color.blue;

    public KeyCode GoUpOptionKey = KeyCode.W;
    public KeyCode GoDownOptionKey = KeyCode.S;



    // Use this for initialization
    void Start ()
    {
        dialogue = gameObject.AddComponent<VIDE_Data>();
    }
	
	// Update is called once per frame
	void Update ()
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

            //Scroll through Player dialogue options
            if (!data.pausedAction)
            {
                if (Input.GetKeyDown(GoDownOptionKey))
                {
                    if (data.selectedOption < currentOptions.Count - 1)
                        data.selectedOption++;
                }
                if (Input.GetKeyDown(GoUpOptionKey))
                {
                    if (data.selectedOption > 0)
                        data.selectedOption--;
                }
            }
        }
    }

    public void BeginConversation(VIDE_Assign diagToLoad)
    {

    }

}

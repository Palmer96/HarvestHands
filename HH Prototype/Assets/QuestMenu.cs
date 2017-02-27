using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestMenu : ScrollMenu
{
    public static QuestMenu instance = null;

    public Text questTrackerText;

    // Use this for initialization
    void Start()
    {
        if (instance == null)
            instance = this;
        //else
        //    Destroy(this);
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public override void ResetDisplay()
    {
        RemoveButtons();
        AddButtons();
        //UpdateDisplay();
    }

    public override void ActivateMenu()
    {
        //Debug.Log("Inside ActivateMenu");
        scrollView.gameObject.SetActive(true);
        //  PlayerInventory.instance.enabled = false;
        PlayerInventory.instance.inMenu = true;
        PlayerInventory.instance.transform.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        createButton.SetActive(true);
        returnButton.SetActive(true);
        background.gameObject.SetActive(true);
        selectedItemName.gameObject.SetActive(true);
        selectedItemDescription.gameObject.SetActive(true);
        //selectedItemResources.gameObject.SetActive(true);
        ResetDisplay();
        SelectButton(0);
    }

    public override void DeactivateMenu()
    {
        scrollView.gameObject.SetActive(false);
        //   PlayerInventory.instance.enabled = true;
        PlayerInventory.instance.inMenu = false;
        PlayerInventory.instance.transform.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        createButton.SetActive(false);
        returnButton.SetActive(false);
        background.gameObject.SetActive(false);
        selectedItemName.gameObject.SetActive(false);
        selectedItemDescription.gameObject.SetActive(false);
        //selectedItemResources.gameObject.SetActive(false);
    }

    public override void SelectButton(int i)
    {
        if (i < PrototypeQuestManager.instance.activeQuests.Count)
        {
            selectedButton = contentButtons[i];
            contentButtons[i].UpdateSelectedButton();
        }
        UpdateSelectedItemInfo();
    }

    public override void UpdateSelectedItemInfo()
    {
        if (selectedButton == null)
        {
            selectedItemName.text = "";
            selectedItemDescription.text = "";
            //selectedItemResources.text = "";
            return;
        }
        selectedItemName.text = (selectedButton as QuestMenuButton).recipe.questName;
        selectedItemDescription.text = (selectedButton as QuestMenuButton).recipe.questDescription;
        //selectedItemResources.text = (selectedButton as CraftingMenuButton).requirementText.text;
    }

    public override void AddButtons() // WHAT TYPE WAS THIS???
    {
        List<QuestPrototype> recipes = PrototypeQuestManager.instance.activeQuests;
        //haveResourceList = new List<CraftingRecipe>();
        //dontHaveResourceList = new List<CraftingRecipe>();
        int recipeIndex = 0;
        //Sort alphabetically
        recipes.Sort(delegate (QuestPrototype a, QuestPrototype b)
        {
            return a.questName.CompareTo(b.questName);
        }
        );

        //create ui buttons for each
        foreach (QuestPrototype recipe in PrototypeQuestManager.instance.activeQuests)
        {
            GameObject menuButton = Instantiate(contentButtonPrefab);
            QuestMenuButton craftingButton = menuButton.GetComponent<QuestMenuButton>();
            menuButton.transform.SetParent(contentPanel);
            contentButtons.Add(craftingButton);
            craftingButton.index = recipeIndex;
            recipeIndex++;
            craftingButton.recipe = recipe;

            craftingButton.UpdateDisplay();
        }

    }

    public void TrackSelectedQuest()
    {
        if (selectedButton == null)
            return;

        PrototypeQuestManager.instance.activeQuestIndex = selectedButton.index;
        PrototypeQuestManager.instance.UpdateQuestText();
    }
    


}

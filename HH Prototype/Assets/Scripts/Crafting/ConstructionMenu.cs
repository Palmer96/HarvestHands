using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionMenu : ScrollMenu
{
    public static ConstructionMenu instance = null;

    public Text selectedItemResources;

    // Use this for initialization
    void Start ()
    {
        Debug.Log("Construction Menu name = " + gameObject.name);
        if (instance == null)
            instance = this;
        //else
        //{
        //    Debug.Log("Instance != null - " + instance.name);
        //    Destroy(this);
        //}
        if (Blueprint.instance == null)
        {
            PlayerInventory.instance.GetComponent<Blueprint>().gameObject.SetActive(true);
            PlayerInventory.instance.GetComponent<Blueprint>().gameObject.SetActive(false);
        }
    }

    public void Awake()
    {
        if (instance == null)
            instance = this;
        //else
        //    Destroy(this);
    }

    public override void ResetDisplay()
    {
        RemoveButtons();
        AddButtons();
    }

    public override void ActivateMenu()
    {
        scrollView.gameObject.SetActive(true);
        PlayerInventory.instance.inMenu = true;
        PlayerInventory.instance.transform.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        createButton.SetActive(true);
        returnButton.SetActive(true);
        background.gameObject.SetActive(true);
        selectedItemName.gameObject.SetActive(true);
        selectedItemDescription.gameObject.SetActive(true);
        selectedItemResources.gameObject.SetActive(true);
        ResetDisplay();
        SelectButton(0);
    }

    public override void DeactivateMenu()
    {
        scrollView.gameObject.SetActive(false);
        PlayerInventory.instance.inMenu = false;
        PlayerInventory.instance.transform.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        createButton.SetActive(false);
        returnButton.SetActive(false);
        background.gameObject.SetActive(false);
        selectedItemName.gameObject.SetActive(false);
        selectedItemDescription.gameObject.SetActive(false);
        selectedItemResources.gameObject.SetActive(false);
    }

    public override void SelectButton(int i)
    {
        if (i < Blueprint.instance.Constructs.Count)
        {
            selectedButton = contentButtons[i];
            contentButtons[i].UpdateSelectedButton();
        }

        UpdateSelectedItemInfo();
    }



    public override void UpdateSelectedItemInfo()
    {
        if (selectedButton == null)
            Debug.Log("selected button == null");
        else
            Debug.Log("selected button = " + selectedButton.name);
        if (selectedButton == null)
        {
            selectedItemName.text = "";
            selectedItemDescription.text = "";
            selectedItemResources.text = "";
        }
        else
        {
            selectedItemName.text = (selectedButton as ConstructionMenuButton).recipe.constructName;
            selectedItemDescription.text = (selectedButton as ConstructionMenuButton).recipe.constructDescription;
            selectedItemResources.text = (selectedButton as ConstructionMenuButton).requirementText.text;
        }
    }

    public void UpdateSelectedItemInfo(ConstructionMenuButton button)
    {
        selectedButton = button;
        if (selectedButton == null)
            Debug.Log("selected button == null");
        else
            Debug.Log("selected button = " + selectedButton.name);
        if (button == null)
        {
            selectedItemName.text = "";
            selectedItemDescription.text = "";
            selectedItemResources.text = "";
        }
        else
        {
            selectedItemName.text =button.recipe.constructName;
            selectedItemDescription.text = button.recipe.constructDescription;
            selectedItemResources.text = button.requirementText.text;
        }
    }

    public override void AddButtons()
    {
        Debug.Log("Blue print name = " + Blueprint.instance.name.ToString());
        List<GameObject> recipes = Blueprint.instance.Constructs;
        int recipeIndex = 0;
        //Sort alphabetically
        recipes.Sort(delegate (GameObject a, GameObject b)
        {
            return a.GetComponent<Construct>().constructName.CompareTo(b.GetComponent<Construct>().constructName);
        }
        );
        
        //create ui buttons for each
        foreach (GameObject constructObject in Blueprint.instance.Constructs)
        {
            Construct constructScript = constructObject.GetComponent<Construct>();
            GameObject menuButton = Instantiate(contentButtonPrefab);
            ConstructionMenuButton craftingButton = menuButton.GetComponent<ConstructionMenuButton>();
            menuButton.transform.SetParent(contentPanel);
            contentButtons.Add(craftingButton);
            craftingButton.index = recipeIndex;
            recipeIndex++;
            craftingButton.recipe = constructScript;

            craftingButton.UpdateDisplay();
        }
    }

    public void CreateSelectedItem()
    {
        if (selectedButton == null)
        {
            Debug.Log("NULL BITCHES");
            return;
        }
        Debug.Log("Assinginged current construct");
        Blueprint.instance.currentConstruct = Instantiate((selectedButton as ConstructionMenuButton).recipe.gameObject);
        Blueprint.instance.currentConstruct.SetActive(true);
        PlayerInventory.instance.inMenu = false;
        PlayerInventory.instance.bookOpen = true;
        PlayerInventory.instance.transform.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
    }
}

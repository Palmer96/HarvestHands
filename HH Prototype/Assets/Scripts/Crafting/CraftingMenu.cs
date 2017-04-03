using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingMenu : ScrollMenu
{
    public enum DisplayItemType
    {
        Default = -1,
        ALL = 0,
        TOOLS = 1,
        BUILDINGS = 2,
    }
    public static CraftingMenu instance = null;

    public DisplayItemType currentRecipeType = DisplayItemType.ALL;
        
    public Text selectedItemResources;
    
    public List<CraftingRecipe> recipes = new List<CraftingRecipe>();
    public List<CraftingRecipe> haveResourceList = new List<CraftingRecipe>();
    public List<CraftingRecipe> dontHaveResourceList = new List<CraftingRecipe>();

    // Use this for initialization
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DeactivateMenu();
        }
    }


    public List<CraftingRecipe> GetRecipeList()
    {
        if (currentRecipeType == DisplayItemType.ALL)
            return CraftingManager.instance.knownRecipes;
        else if (currentRecipeType == DisplayItemType.TOOLS)
        {
            foreach (CraftingRecipe recipe in CraftingManager.instance.knownRecipes)
            {
                if (recipe.recipeType == CraftingRecipe.RecipeType.TOOL)
                {
                    //add to list
                }
            }
        }
        //else if (currentRecipeType == DisplayItemType.BUILDINGS)
        //    return buildingRecipes;


        return recipes;
    }

    public void SetDisplayRecipeType(DisplayItemType type)
    {
        currentRecipeType = type;
        GetRecipeList();
    }

    //public void UpdateDisplay()
    //{
    //    foreach (CraftingMenuButton button in craftingButtons)
    //    {
    //        button.UpdateDisplay();
    //    }
    //}

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
        selectedItemResources.gameObject.SetActive(true);
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
        selectedItemResources.gameObject.SetActive(false);
    }

    public override void SelectButton(int i)
    {
        if (i < CraftingManager.instance.knownRecipes.Count)
        {
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
            selectedItemResources.text = "";
        }
        selectedItemName.text = (selectedButton as CraftingMenuButton).recipe.recipeName;
        selectedItemDescription.text = (selectedButton as CraftingMenuButton).recipe.itemDescription;
        selectedItemResources.text = (selectedButton as CraftingMenuButton).requirementText.text;
    }

    public override void AddButtons() // WHAT TYPE WAS THIS???
    {
        List<CraftingRecipe> recipes = CraftingManager.instance.knownRecipes;
        haveResourceList = new List<CraftingRecipe>();
        dontHaveResourceList = new List<CraftingRecipe>();
        int recipeIndex = 0;
        //Sort alphabetically

        recipes.Sort(delegate (CraftingRecipe a, CraftingRecipe b)
        {
            return a.recipeName.CompareTo(b.recipeName);
        }
        );

        //recipes.Sort(delegate (CraftingRecipe a, CraftingRecipe b)
        //{
        //    return a.recipeName.CompareTo(b.recipeName);
        //}
        //);

        //Sort to have resource and dont have resources
        foreach (CraftingRecipe recipe in CraftingManager.instance.knownRecipes)
        {
            if (recipe.HaveResources())
                haveResourceList.Add(recipe);
            else
                dontHaveResourceList.Add(recipe);
        }        
        //create ui buttons for each
        foreach (CraftingRecipe recipe in haveResourceList)
        {
            GameObject menuButton = Instantiate(contentButtonPrefab);
            CraftingMenuButton craftingButton = menuButton.GetComponent<CraftingMenuButton>();
            menuButton.transform.SetParent(contentPanel);
            contentButtons.Add(craftingButton);
            craftingButton.index = recipeIndex;
            recipeIndex++;
            craftingButton.recipe = recipe;

            craftingButton.UpdateDisplay();
        }
        foreach (CraftingRecipe recipe in dontHaveResourceList)
        {
            GameObject menuButton = Instantiate(contentButtonPrefab);
            CraftingMenuButton craftingButton = menuButton.GetComponent<CraftingMenuButton>();
            menuButton.transform.SetParent(contentPanel);
            contentButtons.Add(craftingButton);
            craftingButton.index = recipeIndex;
            recipeIndex++;
            craftingButton.recipe = recipe;

            craftingButton.UpdateDisplay();
        }
    }
    
    public void CreateSelectedItem()
    {
        if (selectedButton == null)
            return;
        //PlayerInventory.instance.AddItem(selectedButton.recipe.Craft());
        
        GameObject newObj = (selectedButton as CraftingMenuButton).recipe.Craft();
        if (newObj != null)
            PlayerInventory.instance.AddItem(newObj);
        //CraftingManager.instance.knownRecipes[selectedButton.recipeIndex].Craft();
        CraftingRecipe selectedRecipe = (selectedButton as CraftingMenuButton).recipe;
        ResortLists();
        ResetDisplay();
        foreach (CraftingMenuButton button in contentButtons)
        {
            if (button.recipe.recipeName == selectedRecipe.recipeName)
            {
                selectedButton = button;
                break;
            }
        }

        UpdateDisplay(); // TODO Change this so it updates the list objects with the reduced player amounts, instead of destroying and recreating the entire list
    }

    public override void ResortLists()
    {
        //Debug.Log("In resort Lists");
        List<CraftingMenuButton> haveResourceButtonList = new List<CraftingMenuButton>();
        List<CraftingMenuButton> dontHaveResourceButtonList = new List<CraftingMenuButton>();
      //  scrollView.

        foreach (CraftingMenuButton menuButton in contentButtons)
        {
            if (menuButton.recipe.HaveResources())
                haveResourceButtonList.Add(menuButton);
            else
                dontHaveResourceButtonList.Add(menuButton);
        }
        for (int i = dontHaveResourceButtonList.Count; i > 0; --i)
        {
            dontHaveResourceButtonList[i - 1].transform.SetSiblingIndex(0);
        }
        for (int i = haveResourceButtonList.Count; i > 0; --i)
        {
            haveResourceButtonList[i - 1].transform.SetSiblingIndex(0);
        }
    }
}

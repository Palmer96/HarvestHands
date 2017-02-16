﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingMenu : MonoBehaviour
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

    public GameObject recipeListButtonPrefab;
    public Transform scrollView;
    public Transform contentPanel;
    public GameObject craftButton;
    public GameObject returnButton;
    public Color sufficientResourceColour = Color.green;
    public Color insufficientResourceColour = Color.gray;
    public Color canMakeColor = Color.green;
    public Color cantMakeColor = Color.grey;
    public CraftingMenuButton selectedButton = null;
    public List<CraftingMenuButton> craftingButtons = new List<CraftingMenuButton>();

    public Text selectedItemName;
    public Text selectedItemDescription;
    public Text selectedItemResources;
    public RawImage background;


    public List<CraftingRecipe> recipes = new List<CraftingRecipe>();
    private List<CraftingRecipe> haveResourceList = new List<CraftingRecipe>();
    private List<CraftingRecipe> dontHaveResourceList = new List<CraftingRecipe>();

    // Use this for initialization
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
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

    public void UpdateDisplay()
    {
        foreach (CraftingMenuButton button in craftingButtons)
        {
            button.UpdateDisplay();
        }
    }

    public void ResetDisplay()
    {
        RemoveButtons();
        AddButtons(CraftingManager.instance.knownRecipes);
        //UpdateDisplay();
    }

    public void ActivateMenu()
    {
        Debug.Log("Inside ActivateMenu");
        scrollView.gameObject.SetActive(true);
        //  PlayerInventory.instance.enabled = false;
        PlayerInventory.instance.inMenu = false;
        PlayerInventory.instance.transform.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        craftButton.SetActive(true);
        returnButton.SetActive(true);
        background.gameObject.SetActive(true);
        selectedItemName.gameObject.SetActive(true);
        selectedItemDescription.gameObject.SetActive(true);
        selectedItemResources.gameObject.SetActive(true);
        ResetDisplay();
        SelectButton(0);
    }

    public void DeactivateMenu()
    {
        scrollView.gameObject.SetActive(false);
        //   PlayerInventory.instance.enabled = true;
        PlayerInventory.instance.inMenu = false;
        PlayerInventory.instance.transform.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        craftButton.SetActive(false);
        returnButton.SetActive(false);
        background.gameObject.SetActive(false);
        selectedItemName.gameObject.SetActive(false);
        selectedItemDescription.gameObject.SetActive(false);
        selectedItemResources.gameObject.SetActive(false);
    }

    public void SelectButton(int i)
    {
        if (i < CraftingManager.instance.knownRecipes.Count)
        {
            craftingButtons[i].UpdateSelectedButton();
        }
        UpdateSelectedItemInfo();
    }


    public void UpdateSelectedItemInfo()
    {
        selectedItemName.text = selectedButton.recipe.recipeName;
        selectedItemDescription.text = selectedButton.recipe.itemDescription;
        selectedItemResources.text = selectedButton.requirementText.text;
    }

    public void AddButtons(List<CraftingRecipe> recipes)
    {
        haveResourceList = new List<CraftingRecipe>();
        dontHaveResourceList = new List<CraftingRecipe>();
        int recipeIndex = 0;
        //Sort alphabetically
        recipes.Sort(delegate (CraftingRecipe a, CraftingRecipe b)
        {
            return a.recipeName.CompareTo(b.recipeName);
        }
        );
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
            GameObject menuButton = Instantiate(recipeListButtonPrefab);
            CraftingMenuButton craftingButton = menuButton.GetComponent<CraftingMenuButton>();
            menuButton.transform.SetParent(contentPanel);
            craftingButtons.Add(craftingButton);
            craftingButton.recipeIndex = recipeIndex;
            recipeIndex++;
            craftingButton.recipe = recipe;

            craftingButton.UpdateDisplay();
        }
        foreach (CraftingRecipe recipe in dontHaveResourceList)
        {
            GameObject menuButton = Instantiate(recipeListButtonPrefab);
            CraftingMenuButton craftingButton = menuButton.GetComponent<CraftingMenuButton>();
            menuButton.transform.SetParent(contentPanel);
            craftingButtons.Add(craftingButton);
            craftingButton.recipeIndex = recipeIndex;
            recipeIndex++;
            craftingButton.recipe = recipe;

            craftingButton.UpdateDisplay();
        }
    }

    public void RemoveButtons()
    {
        while (craftingButtons.Count > 0)
        {
            Destroy(craftingButtons[0].gameObject);
            craftingButtons.RemoveAt(0);
        }
    }

    public void CreateSelectedItem()
    {
        if (selectedButton == null)
            return;
        //PlayerInventory.instance.AddItem(selectedButton.recipe.Craft());
        GameObject newObj = selectedButton.recipe.Craft();
        if (newObj != null)
            PlayerInventory.instance.AddItem(newObj);
        //CraftingManager.instance.knownRecipes[selectedButton.recipeIndex].Craft();
        ResortLists();
        UpdateDisplay(); // TODO Change this so it updates the list objects with the reduced player amounts, instead of destroying and recreating the entire list
    }

    public void ResortLists()
    {
        Debug.Log("In resort Lists");
        List<CraftingMenuButton> haveResourceButtonList = new List<CraftingMenuButton>();
        List<CraftingMenuButton> dontHaveResourceButtonList = new List<CraftingMenuButton>();
      //  scrollView.

        foreach (CraftingMenuButton menuButton in craftingButtons)
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
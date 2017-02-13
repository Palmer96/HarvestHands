using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Color sufficientResourceColour = Color.green;
    public Color insufficientResourceColour = Color.gray;
    public GameObject recipeListObject;
    public Transform scrollView;
    public GameObject craftButton;
    public GameObject returnButton;
    public Color canMakeColor = Color.green;
    public Color cantMakeColor = Color.grey;
    public CraftingMenuButton selectedButton = null;
    public List<CraftingMenuButton> craftingButtons = new List<CraftingMenuButton>();


    public List<CraftingRecipe> recipes = new List<CraftingRecipe>();
    public List<CraftingRecipe> toolRecipes = new List<CraftingRecipe>();
    public List<CraftingRecipe> buildingRecipes = new List<CraftingRecipe>();

    // Use this for initialization
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        Invoke("UpdateDisplay", 0.5f);
    }

    // Update is called once per frame
    void Update() {

    }

    public List<CraftingRecipe> GetDisplayList()
    {
        if (currentRecipeType == DisplayItemType.ALL)
            return recipes;
        else if (currentRecipeType == DisplayItemType.TOOLS)
            return toolRecipes;
        else if (currentRecipeType == DisplayItemType.BUILDINGS)
            return buildingRecipes;


        return recipes;
    }

    public void SetDisplayRecipeType(DisplayItemType type)
    {
        currentRecipeType = type;
        GetDisplayList();
    }
    public void UpdateDisplay()
    {
        RemoveButtons();
        AddButtons(CraftingManager.instance.knownRecipes);
    }

    public void ActivateMenu()
    {
        Debug.Log("Inside ActivateMenu");
        scrollView.gameObject.SetActive(true);
        PlayerInventory.instance.transform.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        craftButton.SetActive(true);
        returnButton.SetActive(true);
        UpdateDisplay();
    }

    public void DeactivateMenu()
    {
        scrollView.gameObject.SetActive(false);
        PlayerInventory.instance.transform.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        craftButton.SetActive(false);
        returnButton.SetActive(false);
    }
    
    public void AddButtons(List<CraftingRecipe> recipes)
    {
        int recipeIndex = 0;

        foreach (CraftingRecipe recipe in recipes)
        {
            GameObject menuButton = Instantiate(recipeListObject);
            CraftingMenuButton craftingButton = menuButton.GetComponent<CraftingMenuButton>();
            menuButton.transform.SetParent(scrollView);
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
            craftingButtons.RemoveAt(0);
        }
    }

    public void CreateSelectedItem()
    {
        if (selectedButton == null)
            return;
        CraftingManager.instance.knownRecipes[selectedButton.recipeIndex].Craft();
        UpdateDisplay(); // TODO Change this so it updates the list objects with the reduced player amounts, instead of destroying and recreating the entire list
    }
}

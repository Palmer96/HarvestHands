using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    [System.Serializable]
    public class ResourceRequirement
    {
        public string resourceName = "";
        public int numRequired = 0;
    }

    public static CraftingManager instance = null;
    public List<CraftingRecipe> knownRecipes = new List<CraftingRecipe>();
    
	// Use this for initialization
	void Start ()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        SaveAndLoadManager.OnSave += Save;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Debug.Log("crafting manager is on " + gameObject.name);
	}

    void OnDestroy()
    {
        SaveAndLoadManager.OnSave -= Save;
    }

    public virtual void Save()
    {
        SaveAndLoadManager.instance.saveData.craftingRecipeManagerSave = new CraftingRecipeManagerSave(this);
        //Debug.Log("Saved item = " + name);
    }

}


[System.Serializable]
public class CraftingRecipeManagerSave
{
    int recipeCount;
    List<string> recipeNames;

    public CraftingRecipeManagerSave(CraftingManager craftingManager)
    {
        recipeNames = new List<string>();
        recipeCount = craftingManager.knownRecipes.Count;
        foreach (CraftingRecipe craftingRecipe in craftingManager.knownRecipes)
        {
            recipeNames.Add(craftingRecipe.recipeName);
        }        
    }

    public GameObject LoadObject()
    {
        if (CraftingManager.instance == null)
        {
            Debug.Log("Cant load Crafting Manager, CraftingManager.instance = null");
            return null;
        }
       
        CraftingManager.instance.knownRecipes = new List<CraftingRecipe>();
        foreach (string recipeName in recipeNames)
        {
            foreach (CraftingRecipe craftingRecipeType in SaveAndLoadManager.instance.instantiateableCraftingRecipes)
            {
                if (recipeName == craftingRecipeType.recipeName)
                {
                    CraftingManager.instance.knownRecipes.Add(craftingRecipeType);
                    break;
                }
            }
        }
        if (recipeCount != CraftingManager.instance.knownRecipes.Count)
        {
            Debug.Log("CraftingManagerSave couldn't load all recipes, Saved = " + recipeCount.ToString() + " vs Loaded = " + CraftingManager.instance.knownRecipes.Count.ToString());
        }

        return CraftingManager.instance.gameObject;
    }
}
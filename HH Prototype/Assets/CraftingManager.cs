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
	}
	
	// Update is called once per frame
	void Update ()
    {
        Debug.Log("crafting manager is on " + gameObject.name);
	}        

}

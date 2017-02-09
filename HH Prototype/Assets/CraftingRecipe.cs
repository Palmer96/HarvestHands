using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingRecipe : MonoBehaviour
{
    public string recipeName = "";
    public GameObject result;
    public List<CraftingManager.ResourceRequirement> requiredItems = new List<CraftingManager.ResourceRequirement>();

	// Use this for initialization
	void Start ()
    {
        if (result == null)
            Debug.Log(name + " has no result for it's CraftingRecipe");
	}
}

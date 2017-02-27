using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

abstract public class ScrollMenu : MonoBehaviour
{
    public GameObject contentButtonPrefab;
    public Transform scrollView;
    public Transform contentPanel;
    public GameObject createButton;
    public GameObject returnButton;

    public Color sufficientResourceColour = Color.green;
    public Color insufficientResourceColor = Color.gray;
    public Color canMakeColor = Color.green;
    public Color cantMakeColor = Color.grey;

    public ScrollMenuButton selectedButton = null;
    public List<ScrollMenuButton> contentButtons = new List<ScrollMenuButton>();

    public Text selectedItemName;
    public Text selectedItemDescription;
    //public Text selecetedItemResources;
    public RawImage background;

    //public List<CraftingRecipe> recipes = new List<CraftingRecipe>();
    //public List<CraftingRecipe> haveResourceList = new List<CraftingRecipe>();
    //public List<CraftingRecipe> dontHaveResourceList = new List<CraftingRecipe>();

    // Use this for initialization
    void Start ()
    {
	    	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void UpdateDisplay()
    {
        foreach (ScrollMenuButton button in contentButtons)
        {
            button.UpdateDisplay();
        }
    }

    public virtual void ResetDisplay()
    {
        
    }

    public virtual void ActivateMenu()
    {

    }

    public virtual void DeactivateMenu()
    {

    }

    public virtual void SelectButton(int i)
    {
        
    }

    public virtual void UpdateSelectedItemInfo()
    {
        if (selectedButton == null)
        {
            if (selectedItemName != null)
                selectedItemName.text = "";
            if (selectedItemDescription != null)
                selectedItemDescription.text = "";
            //selectedItemResources.text = "";
        }
    }

    public virtual void AddButtons()
    {

    }

    public virtual void RemoveButtons()
    {
        while (contentButtons.Count > 0)
        {
            Destroy(contentButtons[0].gameObject);
            contentButtons.RemoveAt(0);
        }
    }

    public virtual void ResortLists()
    {

    }

    
}

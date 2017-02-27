using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

abstract public class ScrollMenuButton : MonoBehaviour
{
    public int index = -1;
    public Button button;
    public Text nameText;
    public Image iconImage;

    public virtual void UpdateSelectedButton()
    {

    }

    public virtual void SelectButton()
    {
        
    }

    public virtual void UnselectButton()
    {

    }

    public virtual void UpdateDisplay()
    {

    }
	
}

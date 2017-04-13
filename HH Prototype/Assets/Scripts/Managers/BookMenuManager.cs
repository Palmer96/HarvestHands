using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookMenuManager : MonoBehaviour
{
    public List<GameObject> menus;
    public GameObject activeMenu = null;
    public GameObject parentMenu = null;
    public KeyCode menuKey;
    public PlayerInventory.ControllerInput menuController;
    public PlayerInventory.ControllerInput menuExit;
    // Use this for initialization
    void Start()
    {
        foreach (GameObject menu in menus)
        {
            menu.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(menuKey))// || Input.GetButtonDown("Controller_" + menuController))
        {
            if (PlayerInventory.instance.transform.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled == true)
            {
                //If no menu open, open main one
                if (activeMenu == null)
                {
                    PlayerInventory.instance.inMenu = true;
                    PlayerInventory.instance.transform.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    parentMenu.SetActive(true);
                    activeMenu = parentMenu;
                }
                //if main menu open, close menus
                else if (activeMenu == parentMenu)
                {
                    CloseMenusAll();
                }
                else //if child menu open, go back to main
                {
                    CloseMenus();
                    parentMenu.SetActive(true);
                    activeMenu = parentMenu;
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                Debug.Log(Cursor.visible.ToString());
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Debug.Log(Cursor.visible.ToString());
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Controller_" + menuExit))
        {
            CloseMenusAll();
        }
    }

    public void ActivateMenu(int index)
    {
        if (index < menus.Count)
        {
            foreach (GameObject menu in menus)
            {
                menu.SetActive(false);
            }
            menus[index].SetActive(true);
            activeMenu = menus[index];
        }
    }

    public void CloseMenus()
    {
        foreach (GameObject menu in menus)
        {
            menu.SetActive(false);
        }
        activeMenu = null;
    }

    public void CloseMenusAll()
    {
        foreach (GameObject menu in menus)
        {
            menu.SetActive(false);
        }
        activeMenu = null;
        PlayerInventory.instance.inMenu = false;
        PlayerInventory.instance.transform.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}

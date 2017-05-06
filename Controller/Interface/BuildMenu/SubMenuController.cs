using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubMenuController : MonoBehaviour {

    bool subMenuOpened;
    GameObject openedSubMenu;


    void Start()
    {
        subMenuOpened = false;
        openedSubMenu = null;
    }


    void Update()
    {
        EscCloseSubMenu();
    }


    public void SubMenuSwitch(GameObject subMenu)
    {
        if (subMenuOpened == false)
        {
            openedSubMenu = subMenu;
            subMenu.SetActive(true);
            subMenuOpened = true;
        }
        else if (openedSubMenu == subMenu)
        {
            openedSubMenu = null;
            subMenu.SetActive(false);
            subMenuOpened = false;
        }
        else
        {
            openedSubMenu.gameObject.SetActive(false);
            subMenu.SetActive(true);
            openedSubMenu = subMenu;
            subMenuOpened = true;
        }
    }


    public void EscCloseSubMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (subMenuOpened == true && openedSubMenu != null)
            {
                openedSubMenu.SetActive(false);
                openedSubMenu = null;
                subMenuOpened = false;
            }
        }
    }
}

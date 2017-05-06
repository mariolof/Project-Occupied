using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour {

    public Transform mainMenu, optionMenu;

    public void LoadScene()
    {
        Application.LoadLevel("PreGame");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void OptionMenu(bool clicked)
    {
        if (clicked == true)
        {
            optionMenu.gameObject.SetActive(clicked);
            mainMenu.gameObject.SetActive(false);
        }
        else
        {
            optionMenu.gameObject.SetActive(clicked);
            mainMenu.gameObject.SetActive(true);
        }
    }


}

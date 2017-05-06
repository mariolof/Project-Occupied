using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageMenuController : MonoBehaviour{

    public GameObject ManageMenu;
    public GameObject currentSubPanel;


    void Start()
    {

    }


    void Update()
    {
        EscCloseMenu();
    }


    public void SelectCurrentSubPanel(GameObject subPanel)
    {
        if(currentSubPanel != null)
        {
            currentSubPanel.SetActive(false);
        }

        currentSubPanel = subPanel;
        currentSubPanel.SetActive(true);
    }


    public void OpenManageMenu()
    {
        ManageMenu.SetActive(true);
        Time.timeScale = 0;
    }


    public void CloseManageMenu()
    {
        ManageMenu.SetActive(false);
        Time.timeScale = 1;
    }


    public void EscCloseMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ManageMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }
}

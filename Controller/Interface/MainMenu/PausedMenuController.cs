using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausedMenuController : MonoBehaviour {

    public GameObject PausedMenu;
    public GameObject ExitPanel;


    void Start ()
    {
		
	}
	

	void Update ()
    {
		
	}


    public void OpenPauseMenu()
    {
        PausedMenu.SetActive(true);
        Time.timeScale = 0;
    }


    public void ClosePauseMenu()
    {
        PausedMenu.SetActive(false);
        Time.timeScale = 1;
    }


    public void OpenExitPanel()
    {
        ExitPanel.SetActive(true);
    }


    public void ExitPanelControl(int n)
    {
        switch (n)
        {
            case 0:
                break;

            case 1:
                Application.LoadLevel("MainMenu");
                break;

            case 2:
                ExitPanel.SetActive(false);
                break;

            default:
                break;
        }
    }
}

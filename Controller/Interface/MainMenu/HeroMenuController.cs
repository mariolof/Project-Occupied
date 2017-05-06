using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroMenuController : MonoBehaviour {

    public GameObject HeroMenu;

    int heroIndex;


	void Start ()
    {
        UpdateHeroInfo();
    }
	

	void Update ()
    {
		
	}


    public void OpenHeroMenu()
    {
        HeroMenu.SetActive(true);
        Time.timeScale = 0;
        heroIndex = 0;
    }


    public void CloseHeroMenu()
    {
        HeroMenu.SetActive(false);
        Time.timeScale = 1;
        heroIndex = 0;
    }


    public void SwitchHero(int direction)
    {
        if(direction == 1)
        {
            if (heroIndex < HumanController.Instance.heroList.Count - 1)
            {
                heroIndex += 1;
            }
            else
            {
                heroIndex = 0;
            }
        }

        if (direction == -1)
        {
            if (heroIndex != 0)
            {
                heroIndex -= 1;
            }
            else
            {
                heroIndex = HumanController.Instance.heroList.Count - 1;
            }
        }

        UpdateHeroInfo();
    }


    void UpdateHeroInfo()
    {
        Human hero = HumanController.Instance.heroList[heroIndex];
        HeroMenu.transform.Find("HeroSwitch").Find("Name").GetComponent<Text>().text = hero.name;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreGameSetting : MonoBehaviour {

    [SerializeField]
    public float enemyPower = 0.5f;

    public Transform worldSet, detailSet;




    public void BackToMain()
    {
        Application.LoadLevel("MainMenu");
    }


    public void ChooseEasy()
    {
        enemyPower = 0.5f;

        worldSet.gameObject.SetActive(false);
        detailSet.gameObject.SetActive(true);

    }


    public void ChooseNormal()
    {
        enemyPower = 0.7f;

        worldSet.gameObject.SetActive(false);
        detailSet.gameObject.SetActive(true);

    }


    public void ChooseHard()
    {
        enemyPower = 0.9f;

        worldSet.gameObject.SetActive(false);
        detailSet.gameObject.SetActive(true);

    }



    public void BackToWorldSet()
    {
        worldSet.gameObject.SetActive(true);
        detailSet.gameObject.SetActive(false);
    }


    public void StartGame()
    {
        Application.LoadLevel("Village");
    }

}

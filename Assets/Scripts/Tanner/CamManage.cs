using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Tanner Hedges
//Menu Button Functions
public class CamManage : MonoBehaviour
{

    //Tanner Hedges
    //Reset TimeScale when return to Main Menu
    void Start()
    {
        Time.timeScale = 1;
    }
    //Tanner Hedges
    //Start the Game
    public void gameStart()
    {
        SceneManager.LoadScene(1);
    }

    //Tanner Hedges
    //Return to the Main Menu
    public void menuBack()
    {
        SceneManager.LoadScene(0);
    }

    //Tanner Hedges
    //Quit game function
    public void quitGame()
    {
        Application.Quit();
    }
}

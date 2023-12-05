using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameCount : MonoBehaviour
{
    public int score;
    public GameObject winGames;
    public GameObject healthBar;
    public GameObject staminaBar;
    public TMP_Text currentScore;
    public int extraScore;
    public GameObject winMusic;
    public GameObject gameMusic;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        extraScore = 0;
        winMusic.SetActive(false);
        gameMusic.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        currentScore.SetText("Score: " + score);
        if(score > 5000)
        {
            GameWin();
        }
    }

    public void GameWin()
    {
        healthBar.SetActive(false);
        winGames.SetActive(true);
        staminaBar.SetActive(false);
        winMusic.SetActive(true);
        gameMusic.SetActive(false);
        Time.timeScale = 0;
        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

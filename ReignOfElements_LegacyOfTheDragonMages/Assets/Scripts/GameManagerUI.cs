using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManagerUI : MonoBehaviour
{
    public enum GameState { MainMenu, Paused, Playing, GameOver };
    public GameState currentState;
    public TextMeshProUGUI manaPotionText;
    public TextMeshProUGUI healthPotionText;
    public Image fireGem, iceGem, earthGem, key;
    public Slider healthBar, manaBar;
    public GameObject allGameUI, mainMenuPanel, pauseMenuPanel, gameOverPanel, titleText;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "Main_Menu")
        {
            CheckGameState(GameState.MainMenu);
        }
        else
        {
            CheckGameState(GameState.Playing);
        }
    }

    public void CheckGameState(GameState newGameState)
    {
        currentState = newGameState;
        switch (currentState)
        {
            case GameState.MainMenu:
                MainMenuSetup();
                break;
            case GameState.Paused:
                GamePaused();
                Time.timeScale = 0f;
                Manager.gamePaused = true;
                break;
            case GameState.Playing:
                GameActive();
                Time.timeScale = 1f;
                Manager.gamePaused = false;
                break;
            case GameState.GameOver:
                GameOver();
                Time.timeScale = 0f;
                Manager.gamePaused = true;
                break;
        }
    }

    public void MainMenuSetup()
    {
        allGameUI.SetActive(false);
        mainMenuPanel.SetActive(true);
        pauseMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        titleText.SetActive(true);
    }

    public void GameActive()
    {
        allGameUI.SetActive(true);
        mainMenuPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        titleText.SetActive(false);
    }

    public void GamePaused()
    {
        allGameUI.SetActive(true);
        mainMenuPanel.SetActive(false);
        pauseMenuPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        titleText.SetActive(true);
    }

    public void GameOver()
    {
        allGameUI.SetActive(false);
        mainMenuPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
        gameOverPanel.SetActive(true);
        titleText.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        CheckInputs();
    }

    void CheckInputs()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == GameState.Playing)
            {
                CheckGameState(GameState.Paused);
            }
            else if (currentState == GameState.Paused)
            {
                CheckGameState(GameState.Playing);
            }
        }
    }

    public void StartGame()
    {
        Manager.newGame = true;
        SceneManager.LoadScene("Level_01");
        CheckGameState(GameState.Playing);
    }

    public void PauseGame()
    {
        CheckGameState(GameState.Paused);
    }

    public void ResumeGame()
    {
        CheckGameState(GameState.Playing);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main_Menu");
        CheckGameState(GameState.MainMenu);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    //public void UpdateCoins()
    //{
    //    coinText.text = Manager.coins.ToString();
    //}

    //public void UpdateLives()
    //{
    //    lifeText.text = Manager.lives.ToString();
    //}


    //public void UpdateKey(Manager.DoorKeyColours keyColours)
    //{
    //    switch (keyColours)
    //    {
    //        case Manager.DoorKeyColours.Red:
    //            redKeyUI.GetComponent<Image>().color = Color.red;
    //            break;
    //        case Manager.DoorKeyColours.Blue:
    //            blueKeyUI.GetComponent<Image>().color = Color.blue;
    //            break;
    //        case Manager.DoorKeyColours.Yellow:
    //            yellowKeyUI.GetComponent<Image>().color = Color.yellow;
    //            break;
    //    }
    //}
}

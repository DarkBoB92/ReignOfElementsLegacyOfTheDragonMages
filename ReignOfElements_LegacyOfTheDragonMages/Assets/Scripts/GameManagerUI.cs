using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class GameManagerUI : MonoBehaviour
{
    public enum GameState { MainMenu, Paused, Playing, GameOver };
    public GameState currentState;
    public TextMeshProUGUI manaPotionText;
    public TextMeshProUGUI healthPotionText;
    public Image fireGem, iceGem, earthGem, key, healthPotion, manaPotion;
    public Slider healthBar, manaBar;
    public GameObject allGameUI, mainMenuPanel, pauseMenuPanel, gameOverPanel, titleText;
    Health playerHealth;
    Mana playerMana;
    Inventory playerInventory;

    private void Awake()
    {
        CheckGameState(GameState.MainMenu);
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        playerMana = GameObject.FindGameObjectWithTag("Player").GetComponent<Mana>();
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        if (playerHealth != null )
        {
            healthBar.maxValue = playerHealth.GetMaxHealth();
            healthBar.value = healthBar.maxValue;
        }
        if (playerMana != null )
        {
            manaBar.maxValue = playerMana.GetMaxMana();
            manaBar.value = manaBar.maxValue;
        }
    }

    private void Update()
    {
        if(currentState == GameState.Playing)
        {
            UpdateHealthBar();
            UpdateManaBar();
            UpdateGemPockets();
            UpdateKeyPocket();
            UpdatePotionPockets();
        }
    }

    public void CheckGameState(GameState newGameState)
    {
        currentState = newGameState;
        switch (currentState)
        {
            case GameState.MainMenu:
                MainMenuSetup();
                Time.timeScale = 0f;
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

    void OnPauseButton(InputValue input)
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

    public void StartGame()
    {
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

    public void QuitGame()
    {
        Application.Quit();
    }


    public void UpdateHealthBar()
    {
        if(playerHealth != null)
        {
            healthBar.value = playerHealth.currentHealth;
        }
    }

    public void UpdateManaBar()
    {
        if (playerMana != null)
        {
            manaBar.value = playerMana.currentMana;
        }
    }

    void UpdateGemPockets()
    {
        if(playerInventory != null)
        {
            Color tempColor;
            if (playerInventory.fire)
            {
                tempColor = fireGem.color;
                tempColor.a = 1;
                fireGem.color = tempColor;
            }
            else
            {
                tempColor = fireGem.color;
                tempColor.a = 0.3f;
                fireGem.color = tempColor;
            }
            if (playerInventory.water)
            {
                tempColor = iceGem.color;
                tempColor.a = 1;
                iceGem.color = tempColor;
            }
            else
            {
                tempColor = iceGem.color;
                tempColor.a = 0.3f;
                iceGem.color = tempColor;
            }
            if (playerInventory.earth)
            {
                tempColor = earthGem.color;
                tempColor.a = 1;
                earthGem.color = tempColor;
            }
            else
            {
                tempColor = earthGem.color;
                tempColor.a = 0.3f;
                earthGem.color = tempColor;
            }
        }
    }

    void UpdateKeyPocket()
    {
        if (playerInventory != null)
        {
            Color tempColor;
            if (playerInventory.hasKey)
            {
                tempColor = key.color;
                tempColor.a = 1;
                key.color = tempColor;
            }
            else
            {
                tempColor = key.color;
                tempColor.a = 0.3f;
                key.color = tempColor;
            }
        }
    }

    void UpdatePotionPockets()
    {
        if (playerInventory != null)
        {
            Color tempColor;
            if (playerInventory.health > 0)
            {
                tempColor = healthPotion.color;
                tempColor.a = 1;
                healthPotion.color = tempColor;                
            }
            else
            {
                tempColor = healthPotion.color;
                tempColor.a = 0.3f;
                healthPotion.color = tempColor;
            }

            if (playerInventory.mana > 0)
            {
                tempColor = manaPotion.color;
                tempColor.a = 1;
                manaPotion.color = tempColor;
            }
            else
            {
                tempColor = manaPotion.color;
                tempColor.a = 0.3f;
                manaPotion.color = tempColor;
            }
            healthPotionText.text = playerInventory.health.ToString();
            manaPotionText.text = playerInventory.mana.ToString();
        }
    }
}

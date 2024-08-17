using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI_Manager : MonoBehaviour
{
    public GameManager GM;

    public TextMeshProUGUI coinText;
    public Slider healthSlider;


    public GameObject pauseUI;
    public GameObject finishedUI;
    public GameObject gameOverUI;

    public enum GameUIState
    {
        GamePlay,
        Pause, 
        GameOver,
        GameFinished
    }

    private void Start()
    {
        SwitchGameUI( GameUIState.GamePlay);
    }

    GameUIState currentState;

    void Update()
    {
        healthSlider.value = GM.playerCharacter.GetComponent<Health>().CurrentHealthProgress;
        coinText.text = GM.playerCharacter.coin.ToString();
    }


    void SwitchGameUI(GameUIState state)
    {
        pauseUI.gameObject.SetActive(false);
        finishedUI.gameObject.SetActive(false);
        gameOverUI.gameObject.SetActive(false);

        Time.timeScale = 1;

        switch (state)
        {
            case GameUIState.Pause:
                Time.timeScale = 0;
                pauseUI.gameObject.SetActive(true);
                break;

            case GameUIState.GameFinished:
                finishedUI.gameObject.SetActive(true);
                break;
            case GameUIState.GameOver:
                gameOverUI.gameObject.SetActive(true);
                break;
            default:
                break;
        }


        currentState = state;

    }


    public void TogglePauseGame()
    {
        if (currentState== GameUIState.GamePlay)
        {
            SwitchGameUI( GameUIState.Pause);
        }
        else
        {
            SwitchGameUI(GameUIState.GamePlay);
        }
    }

    public void Button_ReturnMainMenu()
    {
        GM.ReturnMainMenu();
    }

    public void Button_Restart()
    {
        GM.Restart();
    }

    public void GameFinish()
    {
        SwitchGameUI( GameUIState.GameFinished);
    }

    public void GameOver()
    {
        SwitchGameUI(GameUIState.GameOver);

    }

}

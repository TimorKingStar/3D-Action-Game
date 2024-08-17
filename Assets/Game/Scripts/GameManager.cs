using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public Character playerCharacter;

    public GameUI_Manager UI_Manager;

    bool isGameOver;

    private void Awake()
    {
        playerCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
    }
    public void GameOver()
    {
        UI_Manager.GameOver();
    }


    public void GameFinish()
    {
        UI_Manager.GameFinish();
    }


    void Update()
    {

        if (isGameOver)
            return;


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UI_Manager.TogglePauseGame();
        }


        if (playerCharacter.currentState== Character.CharacterState.Dead)
        {
            isGameOver = true;
            GameOver();
        }
    }


    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    } 
}

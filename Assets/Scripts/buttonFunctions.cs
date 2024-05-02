using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    
    public void StartButton()
    {
        AudioManager.instance.PlayMenuSound();
        InputManager.instance.playerInput.enabled = true;
        SceneManager.LoadScene("Prototype 2 Map");
    }

    public void Resume()
    {
        AudioManager.instance.PlayMenuSound();
        gamemanager.instance.StateUnPaused();
    }

    public void Restart()
    {
        AudioManager.instance.PlayMenuSound();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gamemanager.instance.StateUnPaused();
    }

    public void Exit()
    {
        PlayerPrefs.DeleteKey("Difficulty");
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }

    public void QuitToMenu()
    {
        if (gamemanager.instance != null)
        {
            gamemanager.instance.StateMainMenu();
        }

        PlayerPrefs.DeleteKey("Difficulty");

        AudioManager.instance.PlayMenuSound();
        SceneManager.LoadScene("Main Menu");
    }

    public void NextWave()
    {
        AudioManager.instance.PlayMenuSound();
        gamemanager.instance.StateUnPaused();
        gamemanager.instance.StartCountDown();
    }

    public void CreditsButton()
    {
        AudioManager.instance.PlayMenuSound();
        SceneManager.LoadScene("Credits");
    }
}

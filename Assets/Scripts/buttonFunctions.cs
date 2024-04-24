using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip menuAud;
    
    public void StartButton()
    {
        aud.PlayOneShot(menuAud);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Resume()
    {
        aud.PlayOneShot(menuAud);
        gamemanager.instance.StateUnPaused();
    }

    public void Restart()
    {
        aud.PlayOneShot(menuAud);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gamemanager.instance.StateUnPaused();
    }

    public void Quit()
    {
        PlayerPrefs.DeleteKey("Difficulty");
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }

    public void NextWave()
    {
        aud.PlayOneShot(menuAud);
        gamemanager.instance.StateUnPaused();
        gamemanager.instance.StartCountDown();
    }

    public void MainMenuButton()
    {
        aud.PlayOneShot(menuAud);
        SceneManager.LoadScene(0);
    }

    public void CreditsButton()
    { 
        aud.PlayOneShot(menuAud);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }
}

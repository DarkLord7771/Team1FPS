using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip[] menuAud;
    

    public void StartButton()
    {
        //aud.PlayOneShot(menuAud[0]);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Resume()
    {
        //aud.PlayOneShot(menuAud[0]);
        gamemanager.instance.StateUnPaused();
    }

    public void Restart()
    {
        //aud.PlayOneShot(menuAud[0]);
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
        //aud.PlayOneShot(menuAud[0]);
        gamemanager.instance.StateUnPaused();
        gamemanager.instance.StartCountDown();
    }
}

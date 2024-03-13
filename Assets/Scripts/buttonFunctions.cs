using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    public void resume()
    {
        gamemanager.instance.stateUnPaused();
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gamemanager.instance.stateUnPaused();
    }

    public void quit()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }

    public void buyHP()
    {
        // Add to player HP.
    }

    public void buySpeed()
    {
        // Add to player speed.
    }

    public void buyJumpDistance()
    {
        // Add to player jump speed.
    }

    public void buyDamage()
    {
        // Add to player damage.
    }
}

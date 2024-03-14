using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    public void StartButton()
    {
        //gamemanager.instance.statePaused();
        SceneManager.LoadScene("Level 0");
        //gamemanager.instance.stateUnPaused();
    }

    public void Resume()
    {
        gamemanager.instance.stateUnPaused();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gamemanager.instance.stateUnPaused();
    }

    public void Quit()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }

    public void NextWave()
    {
        SceneManager.LoadScene("Level " + gamemanager.instance.GetWave());
        gamemanager.instance.stateUnPaused();
    }

    public void BuyHP()
    {
        // Add to player HP if player has 10 gold.
        if (gamemanager.instance.GetGold() == 10)
        {
            gamemanager.instance.playerScript.UpgradeHealth(1);
        }
    }

    public void BuySpeed()
    {
        // Add to player speed if player has 10 gold.
        if (gamemanager.instance.GetGold() == 10)
        {
            gamemanager.instance.playerScript.UpgradeSpeed(1);
        }
    }

    public void BuyJumpDistance()
    {
        // Add to player jump speed if player has 10 gold.
        if (gamemanager.instance.GetGold() == 10)
        {
            gamemanager.instance.playerScript.UpgradeJumpSpeed(1);
        }
    }

    public void BuyDamage()
    {
        if(gamemanager.instance.GetGold() == 10)
        {
            //gamemanager.instance.playerScript.UpgradeDamage(1);
        }
    }
}

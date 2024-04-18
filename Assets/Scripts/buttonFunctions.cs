using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    public void StartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Resume()
    {
        gamemanager.instance.StateUnPaused();
    }

    /*public void openOptions()
    {

    }

    public void closeOptions()
    {

    }*/

    public void Restart()
    {
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
        gamemanager.instance.StateUnPaused();
        gamemanager.instance.StartCountDown();
    }

    public void BuyHP(int cost)
    {
        int playerGold = gamemanager.instance.playerScript.GetGold();

        // Add to player HP if player has enough gold to cover cost, reduce player gold.
        if (playerGold >= cost)
        {
            gamemanager.instance.playerScript.UpgradeHealth(1);
            gamemanager.instance.playerScript.SetGold(-cost);
            gamemanager.instance.UpdateGoldDisplay();
        }
    }

    public void BuySpeed(int cost)
    {
        int playerGold = gamemanager.instance.playerScript.GetGold();

        // Add to player speed if player has 10 gold.
        if (playerGold >= cost)
        {
            gamemanager.instance.playerScript.UpgradeSpeed(1);
            gamemanager.instance.playerScript.SetGold(-cost);
            gamemanager.instance.UpdateGoldDisplay();
        }
    }

    public void BuyJumpDistance(int cost)
    {
        int playerGold = gamemanager.instance.playerScript.GetGold();

        // Add to player jump speed if player has 10 gold.
        if (playerGold >= cost)
        {
            gamemanager.instance.playerScript.UpgradeJumpSpeed(1);
            gamemanager.instance.playerScript.SetGold(-cost);
            gamemanager.instance.UpdateGoldDisplay();
        }
    }

    public void BuyDamage(int cost)
    {
        int playerGold = gamemanager.instance.playerScript.GetGold();

        if (playerGold >= cost)
        {
            gamemanager.instance.playerScript.UpgradeDamage(1);
            gamemanager.instance.playerScript.SetGold(-cost);
            gamemanager.instance.UpdateGoldDisplay();
        }
    }
}

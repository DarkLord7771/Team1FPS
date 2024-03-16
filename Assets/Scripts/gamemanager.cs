using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class gamemanager : MonoBehaviour
{
    public static gamemanager instance;

    // UI Menus
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject menuShop;
    [SerializeField] GameObject menuStart;

    // UI Text
    [SerializeField] TMP_Text enemyCountText;
    [SerializeField] TMP_Text waveCountText;
    [SerializeField] TMP_Text goldTotalText;

    // Player
    public GameObject playerDamageFlash;
    public Image playerHPBar;
    public GameObject player;
    public PlayerController playerScript;

    // Pause
    public bool isPaused;

    // Time
    float timeScaleOrig;

    // Wave function variables
    [SerializeField] int waveCount;
    int totalWaves;

    // Game Mechanics
    int enemyCount;
    int gold;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        timeScaleOrig = Time.timeScale;

        totalWaves = waveCount;

        waveCountText.text = waveCount.ToString("F0");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && menuActive == null)
        {
            statePaused();
            menuActive = menuPause;
            menuActive.SetActive(true);
        }
    }

    public void statePaused()
    {
        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;

        // .None OR .Confined
        Cursor.lockState = CursorLockMode.None;
    }

    public void stateUnPaused()
    {
        isPaused = !isPaused;
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;
    }

    public void updateGameGoal(int amount)
    {
        if (amount < 0)
        {
            gold += 2;
        }

        enemyCount += amount;
        
        // Update enemy count, wave count, and gold text.
        enemyCountText.text = enemyCount.ToString("F0");
        waveCountText.text = waveCount.ToString("F0");
        goldTotalText.text = gold.ToString("F0");

        if (enemyCount <= 0)
        {
            waveCount -= 1;

            if (waveCount > 0)
            {
                statePaused();
                menuActive = menuShop;
                menuActive.SetActive(true);
            }
            else if(waveCount <= 0)
            {
                statePaused();
                menuActive = menuWin;
                menuActive.SetActive(true);
            }
        }
    }

    public void playerHasLost()
    {
        statePaused();
        menuActive = menuLose;
        menuActive.SetActive(true);
    }

    public int GetGold()
    {
        return gold;
    }

    public int GetWave() 
    { 
        return totalWaves - waveCount; 
    }
}

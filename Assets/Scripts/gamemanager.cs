using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class gamemanager : MonoBehaviour
{
    public static gamemanager instance;

    // UI Menus
    [Header("---- UI Menus -----")]
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject menuShop;
    [SerializeField] GameObject menuStart;

    // UI Text
    [Header("---- UI Text -----")]
    [SerializeField] TMP_Text enemyCountText;
    [SerializeField] TMP_Text waveCountText;
    [SerializeField] TMP_Text goldTotalText;

    // Shop variables
    [Header("----- Shop -----")]
    [SerializeField] int cost;

    // Wave function
    [Header("---- Wave Components -----")]
    [SerializeField] GameObject spawner;
    [SerializeField] Spawner spawnerScript;

    [Header("----- Wave Function -----")]
    [SerializeField] int waveCount;
    [SerializeField] int goldDropped;
    int totalWaves;
    int enemyCount;

    // Player
    [Header("----- Player -----")]
    public GameObject player;
    public PlayerController playerScript;
    public GameObject playerDamageFlash;
    public Image playerHPBar;
    public GameObject playerStartPos;

    // Pause
    [Header("----- Game State -----")]
    public bool isPaused;

    // Time
    float timeScaleOrig;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        timeScaleOrig = Time.timeScale;
        playerStartPos = GameObject.FindWithTag("Player Spawn Pos");

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
            gamemanager.instance.playerScript.SetGold(goldDropped);
        }

        enemyCount += amount;
        
        // Update enemy count, wave count, and gold text.
        enemyCountText.text = enemyCount.ToString("F0");
        waveCountText.text = waveCount.ToString("F0");
        UpdateGoldDisplay();

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

    public int GetWave() 
    { 
        return totalWaves - waveCount; 
    }

    public void UpdateGoldDisplay()
    {
        goldTotalText.text = gamemanager.instance.playerScript.GetGold().ToString("F0");
    }
}

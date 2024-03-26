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
    [SerializeField] GameObject menuWaveTimer;

    // UI Text
    [Header("---- UI Text -----")]
    [SerializeField] TMP_Text enemyCountText;
    [SerializeField] TMP_Text waveCountText;
    [SerializeField] TMP_Text waveTimerText;
    [SerializeField] TMP_Text goldTotalText;

    // Shop variables
    [Header("----- Shop -----")]
    [SerializeField] int cost;

    // Wave function
    public int waveCount;
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

        // Check if WaveManager.instance is null or if it started first.
        if (WaveManager.instance)
        {
            //Debug.Log("WM First");
            SetWaveCount();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && menuActive == null)
        {
            StatePaused();
            menuActive = menuPause;
            menuActive.SetActive(true);
        }
    }

    public void StatePaused()
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

    public void UpdateGameGoal(int amount)
    {
        // Update enemy count and text.
        enemyCount += amount;
        enemyCountText.text = enemyCount.ToString("F0");

        // Update Gold Display.
        UpdateGoldDisplay();

        if (enemyCount <= 0)
        {
            // Update wave count and text.
            waveCount--;
            waveCountText.text = waveCount.ToString("F0");

            if (WaveManager.instance.waveCurrent < WaveManager.instance.spawners.Length)
            {
                PlayerBeatWave();
            }
            else if (WaveManager.instance.waveCurrent >= WaveManager.instance.spawners.Length)
            {
                PlayerHasWon();
            }
        }
    }

    public void PlayerHasLost()
    {
        StatePaused();
        menuActive = menuLose;
        menuActive.SetActive(true);
    }

    public void PlayerHasWon()
    {
        StatePaused();
        menuActive = menuWin;
        menuActive.SetActive(true);
    }

    public void PlayerBeatWave()
    {
        StatePaused();
        menuActive = menuShop;
        menuActive.SetActive(true);
    }

    public void WaveCountDown()
    {
        StatePaused();
        menuActive = menuWaveTimer;
        menuActive.SetActive(true);
    }

    public void UpdateGoldDisplay()
    {
        goldTotalText.text = gamemanager.instance.playerScript.GetGold().ToString("F0");
    }

    public void SetWaveCount()
    {
        waveCount = WaveManager.instance.spawners.Length;
        waveCountText.text = waveCount.ToString("F0");
    }
}

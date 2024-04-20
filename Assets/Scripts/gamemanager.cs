using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading;

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
    public GameObject menuFullAmmo;
    public GameObject menuNoAmmo;
    public GameObject menuBuyGun;
    public GameObject menuBuyMelee;
    public GameObject menuNotEnoughGold;

    // UI Text
    [Header("---- UI Text -----")]
    [SerializeField] TMP_Text enemyCountText;
    [SerializeField] TMP_Text waveCountText;
    [SerializeField] TMP_Text waveTimerText;
    [SerializeField] TMP_Text goldTotalText;
    public TMP_Text ammoCurrent;
    public TMP_Text ammoMax;
    public TMP_Text weaponBuyText;
    public TMP_Text weaponCostText;

    [Header("----- UI Variables -----")]
    [SerializeField] float menuDisplayTime;

    // Wave function
    [Header("----- Wave -----")]
    public int waveCount;
    float timeLeft;
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
    public bool isTimerRunning;

    [Header("----- Power Ups -----")]
    
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
            SetWaveCount();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && menuActive == null)
        {
            StatePaused();
            SetMenuActive(menuPause);
        }

        if (isTimerRunning)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                UpdateTimer(timeLeft);
            }
            else
            {
                CountDownComplete();
            }
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

    public void StateUnPaused()
    {
        isPaused = !isPaused;
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SetMenuInactive();
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
        SetMenuActive(menuLose);
    }

    public void PlayerHasWon()
    {
        StatePaused();
        SetMenuActive(menuWin);
    }

    public void PlayerBeatWave()
    {
        StatePaused();
        SetMenuActive(menuShop);
    }

    public void StartCountDown()
    {
        SetMenuActive(menuWaveTimer);
        timeLeft = WaveManager.instance.timeBetweenSpawns;
        isTimerRunning = true;
    }

    public void UpdateGoldDisplay()
    {
        goldTotalText.text = gamemanager.instance.playerScript.GetGold().ToString("F0");
        ShopManager.instance.UpdateShopGold();
    }

    public void SetWaveCount()
    {
        waveCount = WaveManager.instance.spawners.Length;
        waveCountText.text = waveCount.ToString("F0");
    }

    void UpdateTimer(float currentTime)
    {
        currentTime += 1;

        float time = Mathf.FloorToInt(currentTime % 60);

        waveTimerText.text = time.ToString("F0");
    }

    void CountDownComplete()
    {
        isTimerRunning = false;
        SetMenuInactive();
    }

    public void SetMenuActive(GameObject menu)
    {
        menuActive = menu;
        menuActive.SetActive(true);
    }

    public void SetMenuInactive()
    {
        if (menuActive != null)
        {
            menuActive.SetActive(false);
            menuActive = null;
        }
    }

    public IEnumerator DisplayMessage(GameObject menu)
    {
        SetMenuActive(menu);
        yield return new WaitForSeconds(menuDisplayTime);
        SetMenuInactive();
    }
}

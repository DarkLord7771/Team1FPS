using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading;
using Unity.VisualScripting;
using System.Linq;

public class gamemanager : MonoBehaviour
{
    public static gamemanager instance;

    // UI Menus
    [Header("---- UI Menus -----")]
    public GameObject menuActive;
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

    [Header("----- UI Elements -----")]
    public GameObject ammoDisplay;
    [SerializeField] GameObject goldDisplay;

    [Header("----- UI Variables -----")]
    [SerializeField] float menuDisplayTime;

    [Header("---- Power Up Components -----")]
    public PowerUpDisplay PowerUpDisplay;
    public GameObject[] powerUps;

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
    void Awake() //Initial startup
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

    public void StatePaused() //Sets game to paused state
    {
        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;

        // .None OR .Confined
        Cursor.lockState = CursorLockMode.None;
    }

    public void StateUnPaused() //Sets game to unpaused state
    {
        isPaused = !isPaused;
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SetMenuInactive();
    }

    public void UpdateGameGoal(int amount) //Sets enemy count, updates enemy count UI, updates gold display, checks for wave completion and start
    {
        // Update enemy count and text.
        enemyCount += amount;
        enemyCountText.text = enemyCount.ToString("F0");

        if (amount < 0 && !goldDisplay.activeSelf)
        {
            goldDisplay.SetActive(true);
        }

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

    public void PlayerHasLost() //Triggers when player has lost
    {
        StatePaused();
        SetMenuActive(menuLose);
    }

    public void PlayerHasWon()
    {
        StatePaused();
        SetMenuActive(menuWin);
    } //Triggers when player has won

    public void PlayerBeatWave() //Opens shop at end of wave
    {
        StatePaused();
        SetMenuActive(menuShop);
    }

    public void StartCountDown() //Starts countdown for next wave after button is pressed
    {
        SetMenuActive(menuWaveTimer);
        timeLeft = WaveManager.instance.timeBetweenSpawns;
        isTimerRunning = true;
    }

    public void UpdateGoldDisplay() //Updates gold display in UI
    {
        goldTotalText.text = gamemanager.instance.playerScript.GetGold().ToString("F0");
        ShopManager.instance.UpdateShopGold();
    }

    public void SetWaveCount() //Sets wave count and updates wave count UI
    {
        waveCount = WaveManager.instance.spawners.Length - 1;
        waveCountText.text = waveCount.ToString("F0");
    }

    void UpdateTimer(float currentTime) //Updates countdown timer
    {
        currentTime += 1;

        float time = Mathf.FloorToInt(currentTime % 60);

        waveTimerText.text = time.ToString("F0");
    }

    void CountDownComplete()
    {
        isTimerRunning = false;
        SetMenuInactive();
    }//Turns off timer popup when timer is done

    public void SetMenuActive(GameObject menu)
    {
        if (menuActive != null)
        {
            SetMenuInactive();
        }

        menuActive = menu;
        menuActive.SetActive(true);
    } //Sets passed in menu to active

    public void SetMenuInactive()
    {
        if (menuActive != null)
        {
            menuActive.SetActive(false);
            menuActive = null;
        }
    } //Sets current active menu to inactive

    public IEnumerator DisplayMessage(GameObject menu) //Displays message on screen for set amount of time
    {
        SetMenuActive(menu);
        yield return new WaitForSeconds(menuDisplayTime);
        SetMenuInactive();
    }

    public void SetDisplayMessageActive(GameObject display)  //Turns on Display menu for countdown timer
    {
        if (menuActive == null)
        {
            menuActive = display;
            menuActive.SetActive(true);
        }
    }
}

using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

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
    [SerializeField] GameObject menuWaveTimer;

    [Header("----- UI Messages -----")]
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
    public TMP_Text weaponBuyText;
    public TMP_Text weaponCostText;

    [Header("----- UI Variables -----")]
    [SerializeField] float menuDisplayTime;

    [Header("---- Power Up Components -----")]
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
    public GameObject playerStartPos;
    public PlayerUI playerUI;
    public PlayerUpgrade playerUpgrade;

    // Pause
    [Header("----- Game State -----")]
    public bool isPaused;
    public bool isTimerRunning;
    public float difficultyMod;
    public float difficulty;

    // Time
    float timeScaleOrig;

    bool backgroundMusic;

    // Start is called before the first frame update
    void Awake() //Initial startup
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        playerUpgrade = player.GetComponent<PlayerUpgrade>();
        timeScaleOrig = Time.timeScale;
        playerStartPos = GameObject.FindWithTag("Player Spawn Pos");

        // Check if WaveManager.instance is null or if it started first.
        if (WaveManager.instance)
        {
            SetWaveCount();
        }

        difficulty = PlayerPrefs.GetInt("Difficulty");

        if (PlayerPrefs.GetInt("Difficulty") > 0)
        {
            difficultyMod = Mathf.Log10(PlayerPrefs.GetInt("Difficulty")) * 2;
        }
        else
        {
            difficultyMod = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.instance.MenuOpenCloseInput)
        {
            if(!isPaused)
            {
                StatePaused();
                SetMenuActive(menuPause);
            }
            else
            {
                StateUnPaused();
            }
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
        InputManager.instance.playerInput.enabled = false;
    }

    public void StateUnPaused() //Sets game to unpaused state
    {
        isPaused = !isPaused;
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SetMenuInactive();
        InputManager.instance.playerInput.enabled = true;
    }

    public void StateMainMenu()
    {
        isPaused = false;
        Time.timeScale = timeScaleOrig;
    }

    public void UpdateGameGoal(int amount) //Sets enemy count, updates enemy count UI, updates gold display, checks for wave completion and start
    {
        // Update enemy count and text.
        enemyCount += amount;
        enemyCountText.text = enemyCount.ToString("F0");

        if (amount < 0)
        {
            playerUI.DisplayGold();
        }

        // Update Gold Display.
        playerUI.UpdateGold();

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
        SetDisplayMessageActive(menu);
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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpDisplay : MonoBehaviour
{
    [Header("---- Power Up Components -----")]
    public Transform powerUpContentWindow;
    public GameObject powerUpPrefab;
    [SerializeField] TMP_Text powerUpTimer;

    public List<PowerUpEffects> activePowerUps;

    private void Update()
    {
        if (activePowerUps.Count > 0)
        {
            CountDownTimer(activePowerUps);
        }
    }

    public void CreateDisplay(PowerUpEffects powerUp)
    {
        GameObject powerUpDisplay = Instantiate(powerUpPrefab, powerUpContentWindow);
        Transform powerUpTransform = powerUpDisplay.transform;
        powerUp.powerUpRef = powerUpDisplay;

        SetPowerUpSprite(powerUpTransform, powerUp);
        SetPowerUpText(powerUpTransform, powerUp.remainingTime);
    }

    private void SetPowerUpSprite(Transform powerUpTransform, PowerUpEffects powerUp)
    {
        powerUpTransform.Find("DisplayImage").GetComponent<Image>().sprite = powerUp.powerUpSprite;
        powerUpTransform.Find("DisplayImage").GetComponent<Image>().color = powerUp.powerUpColor;
    }

    private void CountDownTimer(List<PowerUpEffects> powerUps)
    {
        foreach (var power in powerUps.ToArray())
        {
            if (power != null && power.powerUpTime != 0)
            {
                if (power.remainingTime > 0)
                {
                    power.remainingTime -= Time.deltaTime;
                    SetPowerUpText(power.powerUpRef.transform, power.remainingTime);
                }
                if (power.remainingTime <= 0)
                {
                    activePowerUps.Remove(power);
                    Destroy(power.powerUpRef);
                }
            }
            else if (!gamemanager.instance.playerScript.hasShield)
            {
                activePowerUps.Remove(power);
                Destroy(power.powerUpRef);
            }
        }
    }

    public void SetPowerUpText(Transform powerUpTransform, float time)
    {
        if (time > 0)
        {
            time += 1;

            float timeRemaining = Mathf.FloorToInt(time % 60);

            powerUpTransform.Find("Timer").GetComponent<TMP_Text>().text = timeRemaining.ToString("F0") + " s";
        }
        else
        {
            powerUpTransform.Find("Timer").GetComponent<TMP_Text>().text = string.Empty;
        }
    }
}

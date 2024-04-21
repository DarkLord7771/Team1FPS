using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpDisplay : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] TMP_Text powerUpTimer;

    public void CreateDisplay(PowerUpEffects powerUp)
    {
        GameObject powerUpDisplay = Instantiate(gamemanager.instance.powerUpPrefab, gamemanager.instance.powerUpContentWindow);
        Transform powerUpTransform = powerUpDisplay.transform;


        UpdateDisplay(powerUpTransform, powerUp);
    }

    private void UpdateDisplay(Transform powerUpTransform, PowerUpEffects powerUp)
    {
        //powerUpTransform.Find("Image").GetComponent<Image>().sprite = powerUp.powerUpSprite;
        powerUpTransform.Find("Timer").GetComponent<TMP_Text>().text = powerUp.powerUpTime.ToString("F0");
        if (powerUp.powerUpTime > 0)
        {
            powerUpTransform.Find("Timer").GetComponent<TMP_Text>().text += " s";
        }
    }
}

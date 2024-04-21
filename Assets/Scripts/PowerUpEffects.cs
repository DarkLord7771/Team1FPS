using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class PowerUpEffects : ScriptableObject
{
    public AudioClip audPowerUp;
    [Range(0f, 1f)] public float audPowerUpVol;

    public Sprite powerUpSprite;
    public Color powerUpColor;
    public float powerUpTime;
    public GameObject powerUpRef;
    public float remainingTime;

    public abstract IEnumerator ApplyEffect();
}

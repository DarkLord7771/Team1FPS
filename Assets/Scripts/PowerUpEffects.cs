using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class PowerUpEffects : ScriptableObject
{
    public Sprite powerUpSprite;
    public Color powerUpColor;
    public float powerUpTime;

    public abstract IEnumerator ApplyEffect();
}

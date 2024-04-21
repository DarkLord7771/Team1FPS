using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Invincibility")]
public class Invincibility : PowerUpEffects
{
    public override IEnumerator ApplyEffect()
    {
        gamemanager.instance.playerScript.SetInvincible();
        yield return new WaitForSeconds(powerUpTime);
        gamemanager.instance.playerScript.SetInvincible();
    }
}
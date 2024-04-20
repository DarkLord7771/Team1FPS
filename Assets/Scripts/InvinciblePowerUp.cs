using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Invincibility")]
public class InvinciblePowerUp : PowerUpEffects
{
    public float powerUpTime;

    public override IEnumerator ApplyEffect(float time)
    {
        gamemanager.instance.playerScript.SetInvincible(true);
        yield return new WaitForSeconds(time);
        gamemanager.instance.playerScript.SetInvincible(false);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/SpeedUp")]
public class SpeedUp : PowerUpEffects
{
    [SerializeField] float speedModifier;
    
    public override IEnumerator ApplyEffect()
    {
        gamemanager.instance.playerScript.speed *= speedModifier;
        yield return new WaitForSeconds(powerUpTime);
        gamemanager.instance.playerScript.speed /= speedModifier;
    }
}

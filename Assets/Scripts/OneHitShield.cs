using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/OneHitShield")]
public class OneHitShield : PowerUpEffects
{
    public override IEnumerator ApplyEffect()
    {
        gamemanager.instance.playerScript.SetShield();
        yield return null;
    }
}
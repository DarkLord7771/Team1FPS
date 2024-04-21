using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/JumpHeightUp")]
public class JumpHeightUp : PowerUpEffects
{
    [SerializeField] float jumpModifier;

    public override IEnumerator ApplyEffect()
    {
        gamemanager.instance.playerScript.jumpSpeed *= jumpModifier;
        yield return new WaitForSeconds(powerUpTime);
        gamemanager.instance.playerScript.jumpSpeed /= jumpModifier;
    }
}

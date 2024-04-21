using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/DamageUp")]
public class DamageUp : PowerUpEffects
{
    [SerializeField] float damageModifier;

    private void Update()
    {

    }

    public override IEnumerator ApplyEffect()
    {
        if (gamemanager.instance.playerScript.HasGuns())
        {
            int startingDamage = gamemanager.instance.playerScript.GetCurrentDamage();
            gamemanager.instance.playerScript.shootDamage = gamemanager.instance.playerScript.GetCurrentDamage();
            yield return new WaitForSeconds(powerUpTime);
            gamemanager.instance.playerScript.shootDamage = gamemanager.instance.playerScript.GetCurrentDamage();
        }
    }


}

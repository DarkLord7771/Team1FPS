using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] Transform attckPos;
    [SerializeField] bool isAttacking;

    public void AttackMelee(AudioSource source, MeleeStats melee, int meleeCount)
    {
        if (InputManager.instance.FireInput && !isAttacking && melee.durabilityCur > 0)
        {
            StartCoroutine(Attack(source, melee));
        }
        
    }

    IEnumerator Attack(AudioSource aud, MeleeStats melee)
    {
        isAttacking = true;

        if(melee.durabilityCur > 0)
        {
            aud.PlayOneShot(melee.meleeSound, melee.meleeSoundVolume);
            melee.durabilityCur--;
        }

        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out RaycastHit hit, melee.meleeDist))
        {
            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if (hit.transform != transform && dmg != null && !hit.collider.CompareTag("Player"))
            {
                dmg.TakeDamage(melee.meleeDamage);
            }
            else
            {
                // Instantiate hit affect and expand reticle.
                Instantiate(melee.hitEffect, hit.point, melee.hitEffect.transform.rotation);
                gamemanager.instance.playerScript.reticle.Expand(gamemanager.instance.playerScript.reticleRecoil);
            }
        }

        yield return new WaitForSeconds(melee.meleeSpeed);
        isAttacking = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class GunAttack : MonoBehaviour
{
    [SerializeField] Laser laser;
    [SerializeField] Transform shootPos;
    Coroutine lastRoutine = null;
    public ProceduralRecoil recoil;
    public bool isShooting;

    public void FireWeapon(GunStats gun, int gunCount)
    {
        // If gun has changed, stop coroutine to avoid waiting for previous guns fire rate.
        if (gamemanager.instance.playerScript.gunHandler.ChangedGun)
        {
            StopCoroutine(lastRoutine);
        }

        if (InputManager.instance.FireInput && (!isShooting || gamemanager.instance.playerScript.gunHandler.ChangedGun) && gun.ammoCur > 0)
        {
            lastRoutine = StartCoroutine(Shoot(gun));
            recoil.Recoil(gun);
        }
        else if (gunCount > 0 && Input.GetButton("Fire1") && !isShooting && gun.ammoCur <= 0)
        {
            StartCoroutine(gamemanager.instance.DisplayMessage(gamemanager.instance.menuNoAmmo));
        }
    }

    public IEnumerator Shoot(GunStats gun)
    {
        isShooting = true;


        if (gun.ammoCur > 0)
        {
            AudioManager.instance.PlayShootSound();
            gun.ammoCur--;

            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out RaycastHit hit, gun.shootDist))
            {
                IDamage dmg = hit.collider.GetComponent<IDamage>();

                // If object hit is not the player, deal damage to object.
                if (hit.transform != transform && dmg != null && !hit.collider.CompareTag("Player"))
                {
                    // Deal damage to object
                    dmg.TakeDamage(gun.shootDamage);

                    // Instantiate blood effect and expand reticle.
                    Instantiate(gun.bloodEffect, hit.point, gun.bloodEffect.transform.rotation);
                    gamemanager.instance.playerScript.reticle.Expand(gamemanager.instance.playerScript.reticleRecoil);
                }
                else
                {
                    // Instantiate hit affect and expand reticle.
                    Instantiate(gun.hitEffect, hit.point, gun.hitEffect.transform.rotation);
                    gamemanager.instance.playerScript.reticle.Expand(gamemanager.instance.playerScript.reticleRecoil);
                }
            }
            // If beam is enabled, shoot beam from trail renderer.
            if (gun.isLaserWeapon)
            {
                StartCoroutine(laser.ShootBeam(gun, shootPos));
            }
        }

        gamemanager.instance.playerScript.gunHandler.ChangedGun = false;
        yield return new WaitForSeconds(gun.fireRate);

        isShooting = false;
    }
}

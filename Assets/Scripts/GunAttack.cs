using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GunAttack : MonoBehaviour
{
    [SerializeField] Laser laser;
    [SerializeField] Transform shootPos;
    [SerializeField] bool isShooting;

    public void FireWeapon(AudioSource source, GunStats gun, int gunCount)
    {

        if (InputManager.instance.FireInput && !isShooting && gun.ammoCur > 0)
        {
            StartCoroutine(Shoot(source, gun));
        }
        else if (gunCount > 0 && Input.GetButton("Fire1") && !isShooting && gun.ammoCur <= 0)
        {
            //StartCoroutine(NoAmmoFlash());
        }
    }

    IEnumerator Shoot(AudioSource aud, GunStats gun)
    {
        isShooting = true;


        if (gun.ammoCur > 0)
        {
            aud.PlayOneShot(gun.shootSound, gun.shootSoundVolume);
            gun.ammoCur--;
            //UpdatePlayerUI();

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

        yield return new WaitForSeconds(gun.fireRate);
        isShooting = false;
    }
}

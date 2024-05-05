using System.Collections;
using UnityEngine;

public class GunAttack : MonoBehaviour
{
    [SerializeField] Laser laser;
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject playerBullet;
    Coroutine lastRoutine = null;
    public ProceduralRecoil recoil;
    public bool isShooting;

    Vector3 target;

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

            Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));

            if (Physics.Raycast(ray, out RaycastHit hit, gun.shootDist))
            {
                IDamage dmg = hit.collider.GetComponent<IDamage>();
                target = hit.point;

                // If object hit is not the player, deal damage to object.
                if (hit.transform != transform && dmg != null && !hit.collider.CompareTag("Player"))
                {
                    // Deal damage to object
                    dmg.TakeDamage(gun.shootDamage);

                    // Instantiate blood effect.
                    Instantiate(gun.bloodEffect, target, gun.bloodEffect.transform.rotation);
                }
                else
                {
                    // Instantiate hit affect.
                    Instantiate(gun.hitEffect, target, gun.hitEffect.transform.rotation);
                }

                // Expand reticle.
                gamemanager.instance.playerScript.reticle.Expand(gamemanager.instance.playerScript.reticleRecoil);
            }
            else
            {
                target = ray.GetPoint(gun.shootDist);
            }
            // If beam is enabled, shoot beam from trail renderer.
            if (gun.isLaserWeapon)
            {
                StartCoroutine(laser.ShootBeam(gun, shootPos));
            }
            else if (!gun.isLaserWeapon)
            {
                CreateBullet(target);
            }
        }

        gamemanager.instance.playerScript.gunHandler.ChangedGun = false;
        yield return new WaitForSeconds(gun.fireRate);

        isShooting = false;
    }

    void CreateBullet(Vector3 target)
    {
        GameObject bullet = Instantiate(playerBullet, shootPos.transform.position, Quaternion.identity);
        PlayerBullet shotBullet = bullet.GetComponent<PlayerBullet>();

        Destroy(bullet, shotBullet.destroyTime);
        bullet.GetComponent<Rigidbody>().AddForce((target - bullet.transform.position).normalized * shotBullet.speed, ForceMode.Impulse);
    }
}

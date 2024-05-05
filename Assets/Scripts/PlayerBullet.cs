using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerBullet : MonoBehaviour
{
    [Header("----- Stats -----")]
    public int speed;
    public int destroyTime;

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        IDamage dmg = other.GetComponent<IDamage>();

        if (dmg != null && !other.CompareTag("Enemy"))
        {
            
            dmg.TakeDamage(gamemanager.instance.playerScript.gunHandler.SelectedGun().shootDamage);
        }

        Destroy(gameObject);
    }
}

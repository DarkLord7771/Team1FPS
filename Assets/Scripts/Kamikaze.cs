using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kamikaze : MonoBehaviour
{
    public AudioClip explosionClip;
    [Range(0.0f, 1.0f)] public float volume;
    
    public GameObject explosion;
    public int explosionDamage;
    [SerializeField] float explosionTime;

    public IEnumerator Explode(IDamage hitEnemy)
    {
        Instantiate(explosion, transform.position, transform.rotation);
        yield return new WaitForSeconds(explosionTime);

        if (hitEnemy != null)
        {
            hitEnemy.TakeDamage(explosionDamage);
        }
    }
}

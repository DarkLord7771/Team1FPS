using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kamikaze : MonoBehaviour
{
    public AudioClip explosionClip;
    [Range(0.0f, 1.0f)] public float volume;
    
    public GameObject explosion;
    public int explosionDamage;

    public void Explode()
    {
        Instantiate(explosion, transform.position, transform.rotation);
    }
}

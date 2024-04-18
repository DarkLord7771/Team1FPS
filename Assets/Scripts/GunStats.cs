using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class GunStats : ScriptableObject
{
    public string gunName;
    public int cost;

    // Shooting Stats
    public int shootDamage;
    public int shootDist;
    public float shootRate;
    public int ammoCur;
    public int ammoMax;

    // Components
    public GameObject model;
    public ParticleSystem hitEffect;
    public AudioClip shootSound;
    [Range(0, 2)] public float shootSoundVolume;
    public Transform gunTransform;
}

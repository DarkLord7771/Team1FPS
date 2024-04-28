using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class GunStats : ScriptableObject
{
    [Header("----- Components -----")]
    public GameObject model;
    public ParticleSystem hitEffect;
    public ParticleSystem bloodEffect;
    public AudioClip shootSound;
    [Range(0, 2)] public float shootSoundVolume;
    public Transform gunTransform;

    [Header("----- Gun Details -----")]
    public string gunName;
    public int cost;
    public bool isLaserWeapon;

    [Header("----- Base Stats -----")]
    public int baseDamage;
    public float baseShootDist;
    public float baseFireRate;
    public int baseMaxAmmo;
    public int baseReloadSpeed;

    [Header("----- Current Stats -----")]
    public int shootDamage;
    public float shootDist;
    public float fireRate;
    public float reloadSpeed;

    [Header("----- Ammo Stats -----")]
    public int ammoCur;
    public int ammoMax;
}

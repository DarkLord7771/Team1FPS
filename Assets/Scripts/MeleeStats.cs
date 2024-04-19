using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeStats : MonoBehaviour
{
    public string meleeName;
    public int cost;

    // melee Stats
    public int meleeDamage;
    public int meleeDist;
    public float meleeRate;
    public int durCur; //dur short for durability
    public int durMax;

    // Components
    public GameObject model;
    public ParticleSystem hitEffect;
    public AudioClip meleeSound;
    [Range(0, 2)] public float meleeSoundVolume;
    public Transform meleeTransform;
}

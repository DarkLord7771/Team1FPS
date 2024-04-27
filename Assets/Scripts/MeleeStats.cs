using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeStats : MonoBehaviour
{
    [Header("----- Components -----")]
    public GameObject model;
    public ParticleSystem hitEffect;
    [Range(0, 2)] public float meleeSoundVolume;
    public Transform meleeTransform;

    [Header("----- Melee Details -----")]
    public string meleeName;
    public int cost;

    [Header("----- Base Stats -----")]
    public int baseMeleeDamage;
    public int baseMeleeDist;
    public float baseMeleeSpeed;

    [Header("----- Current Stats -----")]
    public int meleeDamage;
    public int meleeDist;
    public float meleeSpeed;

    [Header("----- Durability Stats -----")]
    public int durabilityCur;
    public int durabilityMax;
}

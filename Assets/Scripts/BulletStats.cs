using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]

public class BulletStats : ScriptableObject
{
    [Header("----- Stats -----")]
    public int damage;
    public int speed;
    public int destroyTime;
}

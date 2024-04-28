using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heavy : MonoBehaviour
{
    public bool IsLaserShot()
    {
        if (gamemanager.instance.playerScript.HasLaserWeaponEquipped())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

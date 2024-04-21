using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heavy : MonoBehaviour
{
    public bool IsLaserShot()
    {
        if (gamemanager.instance.playerScript.IsNotLaserWeapon())
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}

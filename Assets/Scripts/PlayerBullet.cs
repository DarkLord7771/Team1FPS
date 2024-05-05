using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerBullet : MonoBehaviour
{
    [Header("----- Stats -----")]
    public int speed;
    public int destroyTime;

    void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        Destroy(gameObject);
    }
}

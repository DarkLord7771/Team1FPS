using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBillboaring : MonoBehaviour
{
    private Camera cameraMain;

    private void Awake()
    {
        cameraMain = Camera.main;
    }


    private void Update()
    {
        transform.forward = Camera.main.transform.forward;
    }
}

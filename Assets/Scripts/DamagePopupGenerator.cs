using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopupGenerator : MonoBehaviour
{
    public static DamagePopupGenerator instance;
    public GameObject damagePopupPrefab;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
    }

    public void DisplayPopUp(int amount, Transform popupParent)
    {
        GameObject popup = Instantiate(damagePopupPrefab, popupParent.transform.position, Quaternion.identity, popupParent);
        popup.GetComponent<DamagePopup>().SetUp(amount);
    }
}

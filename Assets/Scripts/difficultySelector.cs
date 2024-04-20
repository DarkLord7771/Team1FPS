using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class difficultySelector : MonoBehaviour
{
    [SerializeField] TMP_Dropdown dropdown;
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip menuAud;

    public int difficulty;

    public void getDropdownValue()
    {
        aud.PlayOneShot(menuAud);
        difficulty = dropdown.value;
        SaveDifficulty();
        //Debug.Log(difficulty);
    }

    public void SaveDifficulty()
    {
        PlayerPrefs.SetInt("Difficulty", difficulty);
    }
}

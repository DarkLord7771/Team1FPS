using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class difficultySelector : MonoBehaviour
{
    [SerializeField] TMP_Dropdown dropdown;

    public int difficulty;

    public void getDropdownValue()
    {
        difficulty = dropdown.value;
        SaveDifficulty();
        //Debug.Log(difficulty);
    }

    public void SaveDifficulty()
    {
        PlayerPrefs.SetInt("Difficulty", difficulty);
    }
}

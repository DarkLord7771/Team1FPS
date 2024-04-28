using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHandler : MonoBehaviour
{
    [SerializeField] PlayerController player;

    [Header("----- Gun Stats -----")]
    [SerializeField] List<MeleeStats> meleeList = new List<MeleeStats>();
    [SerializeField] GameObject meleeModel;
    [SerializeField] int totalMeleeAllowed;
    int selectedMelee;

    private void Start()
    {
        player = gamemanager.instance.playerScript;
    }

    private void Update()
    {
        if (!gamemanager.instance.isPaused)
        {

        }
    }

    public bool BuyMelee(MeleeStats melee)
    {
        if (player.Gold >= melee.cost)
        {
            RemoveMelee();
            GetMeleeStats(melee);
            player.Gold -= melee.cost;

            return true;
        }

        return false;
    }

    public void GetMeleeStats(MeleeStats melee) //Gets melee stats for melee weapon
    {
        meleeList.Add(melee);

        meleeModel.GetComponent<MeshFilter>().sharedMesh = melee.model.GetComponent<MeshFilter>().sharedMesh;
        meleeModel.GetComponent<MeshRenderer>().sharedMaterial = melee.model.GetComponent<MeshRenderer>().sharedMaterial;

        // Set scale of gun model based off of gun's transform.
        meleeModel.GetComponent<Transform>().localScale = melee.meleeTransform.localScale;

        selectedMelee = meleeList.Count - 1;
    }

    void ChangeMelee()
    {
        meleeModel.GetComponent<MeshFilter>().sharedMesh = meleeList[selectedMelee].model.GetComponent<MeshFilter>().sharedMesh;
        meleeModel.GetComponent<MeshRenderer>().sharedMaterial = meleeList[selectedMelee].model.GetComponent<MeshRenderer>().sharedMaterial;

        meleeModel.GetComponent<Transform>().localScale = meleeList[selectedMelee].meleeTransform.localScale;
    }

    void SelectMelee() //Gun selection
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedMelee < meleeList.Count - 1)
        {
            selectedMelee++;
            ChangeMelee();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedMelee > 0)
        {
            selectedMelee--;
            ChangeMelee();
        }
    }

    public MeleeStats SelectedMelee()
    {
        return meleeList[selectedMelee];
    }

    void RemoveMelee()
    {
        if (meleeList.Count >= totalMeleeAllowed)
        {
            meleeList.RemoveAt(0);
        }
    }

    public void RefillDur() //Refills ammo when triggered
    {
        for (int i = 0; i < meleeList.Count; i++)
        {
            meleeList[i].durabilityCur = meleeList[i].durabilityMax;
        }

        //gamemanager.instance.playerUI.UpdateAmmo(meleeList[selectedMelee]);
    }

    public bool HasMelees() //Checks to make sure player has weapons
    {
        if (meleeList.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool HasMissingDur() //checks to see if weapons in inventory are empty
    {
        if (meleeList.Count > 0)
        {
            for (int i = 0; i < meleeList.Count; i++)
            {
                if (meleeList[i].durabilityCur != meleeList[i].durabilityMax)
                {
                    return true;
                }
            }
        }

        return false;
    }
}

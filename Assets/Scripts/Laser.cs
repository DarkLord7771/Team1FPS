using System.Collections;
using System.Collections.Generic;
// using TMPro.EditorUtilities;
// using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Laser : MonoBehaviour
{
    [SerializeField] LineRenderer beam;
    [SerializeField] float beamDisplayTime;

    void Awake()
    {
        beam = GetComponentInChildren<LineRenderer>();
        beam.enabled = false;
    }

    void Activate()
    {
        // Enable line renderer.
        beam.enabled = true;
    }

    void Deactivate(Transform shootPos)
    {
        // Disable line renderer and reset shoot position.
        beam.enabled = false;
        beam.SetPosition(0, shootPos.position);
        beam.SetPosition(1, shootPos.position);
    }

    public IEnumerator ShootBeam(GunStats gun, Transform shootPos)
    {
        Activate();
        
        // Set beam start to shoot position and shoot position forward.
        beam.SetPosition(0, shootPos.position + shootPos.forward * Time.deltaTime);
        beam.SetPosition(1, shootPos.position + shootPos.forward * gun.shootDist);

        yield return new WaitForSeconds(beamDisplayTime);
        Deactivate(shootPos);
    }
}

using System.Collections;
using System.Collections.Generic;
// using TMPro.EditorUtilities;
// using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public LineRenderer beam;
    public Transform shootPos;
    public float beamLength;

    void Awake()
    {
        beam.enabled = false;
    }

    void Activate()
    {
        beam.enabled = true;
    }

    void Deactivate()
    {
        beam.enabled = false;
        beam.SetPosition(0, shootPos.position);
        beam.SetPosition(1, shootPos.position);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0)) Activate();
        else if (Input.GetMouseButtonUp(0)) Deactivate();
    }

    private void FixedUpdate()
    {
        if(!beam.enabled) return;

        Ray ray = new Ray(shootPos.position, shootPos.forward);
        bool cast = Physics.Raycast(ray, out RaycastHit hit, beamLength);
        Vector3 hitPosition = cast ? hit.point : shootPos.position + shootPos.forward * beamLength;

        beam.SetPosition(0, shootPos.position);
        beam.SetPosition(0, hitPosition);
    }
}

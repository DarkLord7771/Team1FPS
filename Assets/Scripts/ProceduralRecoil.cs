using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralRecoil : MonoBehaviour
{
    Vector3 currentRotation;
    Vector3 targetRotation;
    Vector3 targetPosition;
    Vector3 currentPosition;
    Vector3 initialGunPosition;

    public Transform cam;

    [SerializeField] PlayerController playerController;

    public float snappiness;
    public float returnAmount;

    void Start()
    {
        initialGunPosition = transform.localPosition;
    }

    private void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnAmount  * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness *  Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
        //cam.localRotation = Quaternion.Euler(currentRotation);

        Back(); // Kickback
    }

    public void Recoil(GunStats gun)
    {
        targetPosition -= new Vector3(0, 0, gun.kickbackZ);
        targetRotation += new Vector3(gun.recoilX, Random.Range(-gun.recoilY, gun.recoilY), Random.Range(-gun.recoilZ, gun.recoilZ));
        //playerController.transform.position -= new Vector3(playerController.transform.position.x * gun.kickbackZ, 0f, playerController.transform.position.z * gun.kickbackZ);
    }

    void Back()
    {
        targetPosition = Vector3.Lerp(targetPosition, initialGunPosition, returnAmount * Time.deltaTime);
        currentPosition = Vector3.Lerp(currentPosition, targetPosition, snappiness * Time.fixedDeltaTime);
        transform.localPosition = currentPosition;
    }
}

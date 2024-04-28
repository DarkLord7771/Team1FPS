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

    [SerializeField] float recoilX;
    [SerializeField] float recoilY;
    [SerializeField] float recoilZ;
    [SerializeField] float kickbackZ;

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
        cam.localRotation = Quaternion.Euler(new Vector3(playerController.transform.rotation.x * currentRotation.x, 0 , 0));

        Back(); // Kickback
    }

    public void Recoil()
    {
        targetPosition -= new Vector3(0, 0, kickbackZ);
        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
        playerController.transform.position -= playerController.transform.forward * kickbackZ;
    }

    void Back()
    {
        targetPosition = Vector3.Lerp(targetPosition, initialGunPosition, returnAmount * Time.deltaTime);
        currentPosition = Vector3.Lerp(currentPosition, targetPosition, snappiness * Time.fixedDeltaTime);
        transform.localPosition = currentPosition;
    }
}

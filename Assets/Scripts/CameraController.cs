using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField] int sensitivity;
    [SerializeField] int lockVertMin, lockVertMax;
    [SerializeField] bool invertY;
    [SerializeField] GunAttack gunAttack;
    [SerializeField] Slider senseSlider;

    float rotX;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        transform.forward = transform.parent.forward;
        sensitivity = PlayerPrefs.GetInt("Sensitivity");
        senseSlider.value = (float)sensitivity / 100;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseY = InputManager.instance.lookDirection.y * Time.deltaTime * sensitivity;
        float mouseX = InputManager.instance.lookDirection.x * Time.deltaTime * sensitivity;

        if (invertY)
            rotX += mouseY;
        else
            rotX -= mouseY;

        rotX = Mathf.Clamp(rotX, lockVertMin, lockVertMax);

        transform.localRotation = Quaternion.Euler(rotX, 0, 0);

        transform.parent.Rotate(Vector3.up * mouseX);
    }

    public void SetSensitivity(float sense)
    {
        sensitivity = (int)(sense * 100);
        SaveSense();
    }

    void SaveSense()
    {
        PlayerPrefs.SetInt("Sensitivity", sensitivity);
    }
}

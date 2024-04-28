using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    TextMeshPro textMesh;
    Color textColor;
    Transform playerTransform;

    float disappearTimer = 0.5f;
    float fadeOutSpeed = 5f;
    float moveYSpeed = 1f;

    public void SetUp(int amount)
    {
        textMesh = GetComponent<TextMeshPro>();
        playerTransform = Camera.main.transform;
        textColor = textMesh.color;
        textMesh.SetText(amount.ToString());
    }

    private void LateUpdate()
    {
        transform.LookAt(2 * transform.position - playerTransform.position);

        transform.position += new Vector3(0f, moveYSpeed * Time.deltaTime, 0f);

        disappearTimer -= Time.deltaTime;
        if (disappearTimer <= 0)
        {
            textColor.a -= fadeOutSpeed * Time.deltaTime;
            textMesh.color = textColor;

            if (textColor.a <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}

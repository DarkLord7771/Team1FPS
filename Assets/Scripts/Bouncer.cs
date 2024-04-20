using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : MonoBehaviour
{
    [SerializeField] float bounceHeight;
    [SerializeField] float bounceVariation;
    [SerializeField] float bounceSpeed;

    float startHeight;
    float offsetOfTime;

    // Start is called before the first frame update
    void Start()
    {
        startHeight = transform.localPosition.y;
        offsetOfTime = Random.value * Mathf.PI;
    }

    // Update is called once per frame
    void Update()
    {
        Bounce();
    }

    void Bounce()
    {
        float finalheight = startHeight + Mathf.Sin(Time.time * bounceSpeed + offsetOfTime) * bounceVariation;
        Vector3 position = transform.localPosition;
        position.y = finalheight;
        transform.localPosition = position;
    }
}

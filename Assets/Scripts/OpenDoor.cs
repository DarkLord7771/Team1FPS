using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public GameObject Door;

    public float risingMax = 10f;
    
    public float openSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Door.transform.position.y < risingMax)
        {
            Door.transform.Translate(0f, openSpeed * Time.deltaTime, 0f);
        }
    }


}

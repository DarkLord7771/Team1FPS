using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reticle : MonoBehaviour
{
    // Top, bottom and sides of the reticle
    public RawImage up;
    public RawImage down;
    public RawImage left;
    public RawImage right;

    // Default positions of the reticle
    Vector3 upDefaultPos;
    Vector3 downDefaultPos;
    Vector3 leftDefaultPos;
    Vector3 rightDefaultPos;

    // Speed of the reticle returning to the center
    float returnToCenterSpeed;

    // Start is called before the first frame update
    void Start()
    {
        // Default positions so the reticle can return to center
        upDefaultPos = up.transform.position;
        downDefaultPos = down.transform.position;
        leftDefaultPos = left.transform.position;
        rightDefaultPos = right.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Call the function to shrink the reticle to normal size on update
        ShrinkReticleToNormal();
    }

    // Shrink the reticle to normal size
    void ShrinkReticleToNormal()
    {
        // If the reticle y up is not in the center, return it to the center
        if (up.transform.position.y > upDefaultPos.y)
            up.transform.position = new Vector3(up.transform.position.x, up.transform.position.y - returnToCenterSpeed, up.transform.position.z);

        // If the reticle y down is not in the center, return it to the center
        if (down.transform.position.y < downDefaultPos.y)
            down.transform.position = new Vector3(down.transform.position.x, down.transform.position.y + returnToCenterSpeed, down.transform.position.z);

        // If the reticle x left is not in the center, return it to the center
        if (left.transform.position.x < leftDefaultPos.x)
            left.transform.position = new Vector3(left.transform.position.x + returnToCenterSpeed, left.transform.position.y, left.transform.position.z);

        // If the reticle x right is not in the center, return it to the center
        if (right.transform.position.x > rightDefaultPos.x)
            right.transform.position = new Vector3(right.transform.position.x - returnToCenterSpeed, right.transform.position.y, right.transform.position.z);
    }

    // Expand the reticle
    public void Expand(float expandAmount)
    {
        // Expand the reticle by expandAmount
        up.transform.position = new Vector3(up.transform.position.x, up.transform.position.y + expandAmount, up.transform.position.z);
        down.transform.position = new Vector3(down.transform.position.x, down.transform.position.y - expandAmount, down.transform.position.z);
        left.transform.position = new Vector3(left.transform.position.x - expandAmount, left.transform.position.y, left.transform.position.z);
        right.transform.position = new Vector3(right.transform.position.x + expandAmount, right.transform.position.y, right.transform.position.z);
    }

    // Set the speed of the reticle returning to the center
    public void SetReturnToCenterSpeed(float speed)
    {
        returnToCenterSpeed = speed;
    }
}

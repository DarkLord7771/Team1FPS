using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class HumanBone
{
    public HumanBodyBones bone;
    public float weight = 1.0f;
}

public class WeaponIk : MonoBehaviour
{
    [SerializeField] Transform targetTransform;
    [SerializeField] Transform aimTransform;
    [SerializeField] Vector3 targetOffset;

    [SerializeField] int iterations = 10;
    [Range(0, 1)] public float weight = 1.0f;

    [SerializeField] float angleLimit = 90.0f;
    [SerializeField] float distanceLimit = 1.5f;

    [SerializeField] HumanBone[] humanBones;
    Transform[] boneTransforms;

    // Start is called before the first frame update
    void Start()
    {
        Animator animator = GetComponent<Animator>();
        boneTransforms = new Transform[humanBones.Length];
        for (int i = 0; i < humanBones.Length; i++) 
        {
            boneTransforms[i] = animator.GetBoneTransform(humanBones[i].bone);
        }
    }
    Vector3 GetTargetPosition()
    {
        Vector3 targetDirection = (targetTransform.position + targetOffset) - aimTransform.position;
        Vector3 aimDirection = aimTransform.forward;
        float blendOut = 0.0f;

        Debug.DrawLine(targetDirection, aimDirection);

        // Get angle from target.
        float targetAngle = Vector3.Angle(targetDirection, aimDirection);
        if (targetAngle > angleLimit)
        {
            blendOut += (targetAngle - angleLimit) / 50.0f;
        }

        // Get distance from target.
        float targetDistance = targetDirection.magnitude;
        if (targetDistance < distanceLimit)
        {
            blendOut += distanceLimit - targetDistance;
        }

        Vector3 direction = Vector3.Slerp(targetDirection, aimDirection, blendOut);
        return aimTransform.position + direction;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (aimTransform == null)
        {
            return;
        }

        if (targetTransform == null)
        {
            return;
        }

        Vector3 targetPos = GetTargetPosition();
        for (int i = 0; i < iterations; i++)
        {
            for (int b = 0; b < boneTransforms.Length; b++)
            {
                Transform bone = boneTransforms[b];
                float boneWeight = humanBones[b].weight * weight;
                AimAtTarget(bone, targetPos, boneWeight);
            }
        }
    }

    private void AimAtTarget(Transform bone, Vector3 targetPos, float weight)
    {
        // Get Aim Direction and Target Direction.
        Vector3 aimDirection = aimTransform.forward;
        Vector3 targetDirection = targetPos - aimTransform.position;

        // Calculate rotation from aim direction and target direction.
        Quaternion aimTowards = Quaternion.FromToRotation(aimDirection, targetDirection);

        // Calculate influence of weight on rotation.
        Quaternion blendedRotation = Quaternion.Slerp(Quaternion.identity, aimTowards, weight);

        // Rotate bone using blended rotation.
        bone.rotation = blendedRotation * bone.rotation;
    }

    public void SetTargetTransform(Transform target)
    {
        targetTransform = target;
    }

    public void SetAimTransform(Transform aim)
    {
        aimTransform = aim;
    }
}

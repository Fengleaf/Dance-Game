using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionModel : MonoBehaviour
{
    private Animator animator;

    private Dictionary<int, Transform> boneTransforms;
    public Dictionary<int, Transform> BoneTransforms { get => boneTransforms; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        boneTransforms = new Dictionary<int, Transform>();

        boneTransforms[PositionIndex.rShldrBend.Int()] = animator.GetBoneTransform(HumanBodyBones.RightUpperArm);
        boneTransforms[PositionIndex.rForearmBend.Int()] = animator.GetBoneTransform(HumanBodyBones.RightLowerArm);
        //boneTransforms[PositionIndex.rHand.Int()] = animator.GetBoneTransform(HumanBodyBones.RightHand);
        //boneTransforms[PositionIndex.rThumb2.Int()] = animator.GetBoneTransform(HumanBodyBones.RightThumbIntermediate);
        //boneTransforms[PositionIndex.rMid1.Int()] = animator.GetBoneTransform(HumanBodyBones.RightMiddleProximal);
        // Left Arm
        boneTransforms[PositionIndex.lShldrBend.Int()] = animator.GetBoneTransform(HumanBodyBones.LeftUpperArm);
        boneTransforms[PositionIndex.lForearmBend.Int()] = animator.GetBoneTransform(HumanBodyBones.LeftLowerArm);
        //boneTransforms[PositionIndex.lHand.Int()] = animator.GetBoneTransform(HumanBodyBones.LeftHand);
        //boneTransforms[PositionIndex.lThumb2.Int()] = animator.GetBoneTransform(HumanBodyBones.LeftThumbIntermediate);
        //boneTransforms[PositionIndex.lMid1.Int()] = animator.GetBoneTransform(HumanBodyBones.LeftMiddleProximal);

        // Right Leg
        boneTransforms[PositionIndex.rThighBend.Int()] = animator.GetBoneTransform(HumanBodyBones.RightUpperLeg);
        boneTransforms[PositionIndex.rShin.Int()] = animator.GetBoneTransform(HumanBodyBones.RightLowerLeg);
        boneTransforms[PositionIndex.rFoot.Int()] = animator.GetBoneTransform(HumanBodyBones.RightFoot);
        //boneTransforms[PositionIndex.rToe.Int()] = animator.GetBoneTransform(HumanBodyBones.RightToes);

        // Left Leg
        boneTransforms[PositionIndex.lThighBend.Int()] = animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg);
        boneTransforms[PositionIndex.lShin.Int()] = animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg);
        boneTransforms[PositionIndex.lFoot.Int()] = animator.GetBoneTransform(HumanBodyBones.LeftFoot);
        //boneTransforms[PositionIndex.lToe.Int()] = animator.GetBoneTransform(HumanBodyBones.LeftToes);

        // etc
        //boneTransforms[PositionIndex.abdomenUpper.Int()] = animator.GetBoneTransform(HumanBodyBones.Spine);
        //boneTransforms[PositionIndex.hip.Int()] = animator.GetBoneTransform(HumanBodyBones.Hips);
        boneTransforms[PositionIndex.head.Int()] = animator.GetBoneTransform(HumanBodyBones.Head);
        //boneTransforms[PositionIndex.neck.Int()] = animator.GetBoneTransform(HumanBodyBones.Neck);
        //boneTransforms[PositionIndex.spine.Int()] = animator.GetBoneTransform(HumanBodyBones.Spine);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

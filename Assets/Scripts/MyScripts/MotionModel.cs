using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionModel : MonoBehaviour
{
    private Animator animator;

    private Dictionary<int, Transform> boneTransforms;
    public Dictionary<int, Transform> BoneTransforms { get => boneTransforms; }

    public List<Dictionary<int, Transform>> boneFrames = new List<Dictionary<int, Transform>>();

    private bool record = false;

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

        // Left Arm
        boneTransforms[PositionIndex.lShldrBend.Int()] = animator.GetBoneTransform(HumanBodyBones.LeftUpperArm);
        boneTransforms[PositionIndex.lForearmBend.Int()] = animator.GetBoneTransform(HumanBodyBones.LeftLowerArm);

        // Right Leg
        //boneTransforms[PositionIndex.rThighBend.Int()] = animator.GetBoneTransform(HumanBodyBones.RightUpperLeg);
        //boneTransforms[PositionIndex.rShin.Int()] = animator.GetBoneTransform(HumanBodyBones.RightLowerLeg);
        //boneTransforms[PositionIndex.rFoot.Int()] = animator.GetBoneTransform(HumanBodyBones.RightFoot);

        //// Left Leg
        //boneTransforms[PositionIndex.lThighBend.Int()] = animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg);
        //boneTransforms[PositionIndex.lShin.Int()] = animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg);
        //boneTransforms[PositionIndex.lFoot.Int()] = animator.GetBoneTransform(HumanBodyBones.LeftFoot);

        //boneTransforms[PositionIndex.head.Int()] = animator.GetBoneTransform(HumanBodyBones.Head);
    }

    // Update is called once per frame
    void Update()
    {
        if (record)
        {
            boneFrames.Add(new Dictionary<int, Transform>());
            foreach (KeyValuePair<int, Transform> pair in boneTransforms)
            {
                boneFrames[boneFrames.Count - 1].Add(pair.Key, pair.Value);
            }
        }
    }

    public void StartRecord()
    {
        boneFrames.Clear();
        record = true;
    }

    public void SaveRecord()
    {
        record = false;
    }
}

using UnityEngine;
using UnityEngine.Animations.Rigging;

/// <summary>
/// Class storing the names of arm bones
/// </summary>
sealed class ArmBones
{
    public readonly HumanBodyBones RootBone;
    public readonly HumanBodyBones MidBone;
    public readonly HumanBodyBones TipBone;

    public ArmBones(HumanBodyBones rootBone, HumanBodyBones midBone, HumanBodyBones tipBone)
    {
        RootBone = rootBone;
        MidBone = midBone;
        TipBone = tipBone;
    }
}

/// <summary>
/// <para>Avatar XR control setup.</para>
/// </summary>
public sealed class XRAvatarSetup : AbstractAvatarSetup
{
    private readonly ArmBones RightArm = 
        new ArmBones(HumanBodyBones.RightUpperArm, HumanBodyBones.RightLowerArm, HumanBodyBones.RightHand);

    private readonly ArmBones LeftArm =
        new ArmBones(HumanBodyBones.LeftUpperArm, HumanBodyBones.LeftLowerArm, HumanBodyBones.LeftHand);

    [SerializeField] private Transform _xrHead;
    [SerializeField] private Transform _xrLeftController;
    [SerializeField] private Transform _xrRightController;
    [SerializeField] private RuntimeAnimatorController _walkingAnimatorController;
    
    private Animator spawnedAvatar;
    private GameObject spawnedAvatarObject;
    private GameObject vrRigsObject;

    public override bool ComponentContainsErrors()
    {
        bool containsErrors = false;
        if (_xrHead == null)
        {
            CustomLogger.Error(this, "XR Head not set");
            containsErrors = true;
        }

        if (_xrLeftController == null)
        {
            CustomLogger.Error(this, "XR Left Controller not set");
            containsErrors = true;
        }

        if (_xrRightController == null)
        {
            CustomLogger.Error(this, "XR Right Controller not set");
            containsErrors = true;
        }

        if (_walkingAnimatorController == null)
        {
            CustomLogger.Error(this, "Walking Animator Controller not set");
            containsErrors = true;
        }

        return containsErrors;
    }

    public override void SetUp(ref Animator spawnedAvatar)
    {
        this.spawnedAvatar = spawnedAvatar;
        this.spawnedAvatarObject = this.spawnedAvatar.gameObject;

        RigBuilder rigBuilder = spawnedAvatarObject.AddComponent<RigBuilder>();
        this.vrRigsObject = CreateVRRigs(rigBuilder);

        GameObject rightArmTarget = CreateArmTarget("RightArm", RightArm);
        GameObject leftArmTarget = CreateArmTarget("LeftArm", LeftArm);
        GameObject head = CreateHead();

        CreateMapper(head, rightArmTarget, leftArmTarget);

        rigBuilder.Build();

        CreateWalkingAnimation();
    }

    private GameObject CreateVRRigs(RigBuilder rigBuilder)
    {
        GameObject vrRigs = Instantiate(new GameObject("VRRigs"), spawnedAvatarObject.transform, false);

        Rig rig = vrRigs.AddComponent<Rig>();
        RigLayer rigLayer = new RigLayer(rig);
        rigBuilder.layers.Add(rigLayer);

        return vrRigs;
    }

    private GameObject CreateArmTarget(string name, ArmBones bones)
    {
        GameObject arm = Instantiate(new GameObject(name), vrRigsObject.transform, false);
        TwoBoneIKConstraint constraintObject = arm.AddComponent<TwoBoneIKConstraint>();
        TwoBoneIKConstraintData constraintData = constraintObject.data;
        constraintData.root = spawnedAvatar.GetBoneTransform(bones.RootBone);
        constraintData.mid = spawnedAvatar.GetBoneTransform(bones.MidBone);
        constraintData.tip = spawnedAvatar.GetBoneTransform(bones.TipBone);

        GameObject target = Instantiate(new GameObject("Target"), arm.transform, false);
        GameObject hint = Instantiate(new GameObject("Hint"), constraintData.mid, false);

        AlignTransform(ref target, constraintData.tip.gameObject);
        AlignTransform(ref hint, constraintData.mid.gameObject);

        float xOffset = bones.TipBone == HumanBodyBones.LeftHand ? -0.2f : 0.2f;
        hint.transform.localPosition = 
            new Vector3(hint.transform.localPosition.x + xOffset, hint.transform.localPosition.y, hint.transform.localPosition.z);

        constraintData.target = target.transform;
        constraintData.hint = hint.transform;

        constraintData.maintainTargetPositionOffset = true;
        constraintData.maintainTargetRotationOffset = true;
        constraintData.targetPositionWeight = 1;
        constraintData.targetRotationWeight = 1;
        constraintData.hintWeight = 1;

        constraintObject.data = constraintData;

        return target;
    }

    private GameObject CreateHead()
    {
        GameObject head = Instantiate(new GameObject("Head"), vrRigsObject.transform, false);

        MultiParentConstraint constraintObject = head.AddComponent<MultiParentConstraint>();

        MultiParentConstraintData constraintData = constraintObject.data;
        constraintData.constrainedObject = spawnedAvatar.GetBoneTransform(HumanBodyBones.Head);
        
        WeightedTransformArray sourceObjects = new WeightedTransformArray();
        sourceObjects.Add(new WeightedTransform(head.transform, 1));
        
        constraintData.sourceObjects = sourceObjects;

        constraintData.constrainedPositionXAxis = true;
        constraintData.constrainedPositionYAxis = true;
        constraintData.constrainedPositionZAxis = true;
        constraintData.constrainedRotationXAxis = true;
        constraintData.constrainedRotationYAxis = true;
        constraintData.constrainedRotationZAxis = true;

        AlignTransform(ref head, constraintData.constrainedObject.gameObject);

        constraintObject.data = constraintData;

        return head;
    }

    private static void AlignTransform(ref GameObject target, GameObject alignTo)
    {
        target.transform.position = 
            new Vector3(alignTo.transform.position.x, alignTo.transform.position.y, alignTo.transform.position.z);
        target.transform.rotation = 
            Quaternion.Euler(alignTo.transform.rotation.x, alignTo.transform.rotation.y, alignTo.transform.rotation.z);
    }

    private void CreateMapper(GameObject head, GameObject rightArmTarget, GameObject leftArmTarget)
    {
        MapTransform headMapTransform = new MapTransform();
        headMapTransform.Rig = head.transform;
        headMapTransform.Target = _xrHead;
        headMapTransform.TrackingPositionOffset = new Vector3(0, -0.2f, 0);
        
        MapTransform rightArmMapTransform = new MapTransform();
        rightArmMapTransform.Rig = rightArmTarget.transform;
        rightArmMapTransform.Target = _xrRightController;
        
        Vector3 rightHandRotation = GetRotationRelativeToBackwardVector(-90, -90, 0);
        rightArmMapTransform.TrackingRotationOffset = 
            new Vector3(rightHandRotation.x, rightHandRotation.y, rightHandRotation.z);

        MapTransform leftArmMapTransform = new MapTransform();
        leftArmMapTransform.Rig = leftArmTarget.transform;
        leftArmMapTransform.Target = _xrLeftController;
        Vector3 leftHandRotation = GetRotationRelativeToBackwardVector(-90, 90, 0);
        leftArmMapTransform.TrackingRotationOffset = 
            new Vector3(leftHandRotation.x, leftHandRotation.y, leftHandRotation.z);

        AvatarXRMapper avatarXRMapper = spawnedAvatarObject.AddComponent<AvatarXRMapper>();
        avatarXRMapper.HeadRig = head.transform;
        avatarXRMapper.Head = headMapTransform;
        avatarXRMapper.LeftHand = leftArmMapTransform;
        avatarXRMapper.RightHand = rightArmMapTransform;
    }

    private Vector3 GetRotationRelativeToBackwardVector(float x, float y, float z)
    {
        Quaternion rotation = Quaternion.Euler(x, y, z) * Quaternion.Inverse( Quaternion.LookRotation(transform.forward));
        return rotation.eulerAngles;
    }

    private void CreateWalkingAnimation()
    {
        spawnedAvatar.runtimeAnimatorController = _walkingAnimatorController;
        LowerbodyAnimation lowerbodyAnimation = spawnedAvatarObject.AddComponent<LowerbodyAnimation>();
        
        WalkingAnimatorController walkingAnimatorController = spawnedAvatarObject.AddComponent<WalkingAnimatorController>();
        walkingAnimatorController.Animator = spawnedAvatar;
        walkingAnimatorController.Head = _xrHead;
        walkingAnimatorController.PreviousPosition = _xrHead.position;
    }
}
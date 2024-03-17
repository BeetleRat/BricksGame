using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// <para>Avatar kinect control setup.</para>
/// </summary>
public sealed class KinectAvatarSetup : AbstractAvatarSetup
{
    [Tooltip("TrackerHandler TrackerHandler located in the Kinect4AzureTrackerAllInOne prefab")]
    [SerializeField] private TrackerHandler _kinectDevice;
    [SerializeField] private PositionModification _positionModification;

    [Tooltip("pointBody object which is a child of Kinect4AzureTrackerAllInOne prefab")]
    [SerializeField] private Transform _rootPosition;
    
    public override bool ComponentContainsErrors()
    {
        if (base.ComponentContainsErrors())
        {
            return true;
        }

        if (_kinectDevice == null)
        {
            CustomLogger.Error(this, "No kinect device");
            return true;
        }

        return false;
    }

    /// <summary>
    /// Adds kinect control to the avatar.
    /// </summary>
    /// <param name="spawnedAvatar"><inheritdoc cref="AbstractAvatarSetup.SetUp"/></param>
    public override void SetUp(ref Animator spawnedAvatar)
    {
        ModifiedPuppetAvatar puppetAvatar = spawnedAvatar.AddComponent<ModifiedPuppetAvatar>();
        puppetAvatar.KinectDevice = _kinectDevice;
        puppetAvatar.RootPosition = _rootPosition;
        puppetAvatar.PositionModification = _positionModification;
        // RPM Avatar fix
        Transform armatureRoot = RPMAvatarCorrectionArmatureRoot(spawnedAvatar, puppetAvatar);

        puppetAvatar.CreateKinectAvatar(armatureRoot);
    }

    // RPM avatars have a special skeletal structure.
    // In RPM avatars, the root of the Hips skeleton is packed into an Armature object,
    // which should be considered the root of the avatar skeleton
    private Transform RPMAvatarCorrectionArmatureRoot(Animator spawnedPlayer, ModifiedPuppetAvatar puppetAvatar)
    {
        Transform armatureRoot = spawnedPlayer.GetBoneTransform(HumanBodyBones.Hips);
        if (armatureRoot.parent != puppetAvatar.transform)
        {
            return armatureRoot.parent;
        }

        return armatureRoot;
    }
}
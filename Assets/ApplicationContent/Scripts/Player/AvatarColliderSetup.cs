using UnityEngine;

/// <summary>
/// <para>Avatar colliders setup.</para>
/// <param name="playerBodypartsColliders">the <see cref="PlayerBodypartsColliders"/> of player</param>
/// </summary>
public class AvatarColliderSetup : AbstractAvatarSetup
{
    [SerializeField] private PlayerBodypartsColliders _playerBodypartsColliders;
    
    private PlayerBodypartsColliders spawnedBodypartsColliders;

    public override bool ComponentContainsErrors()
    {
        if (base.ComponentContainsErrors())
        {
            return true;
        }

        if (_playerBodypartsColliders == null)
        {
            CustomLogger.Error(this, "Humanoid colliders not selected");
            return true;
        }

        return false;
    }

    /// <summary>
    /// Adds colliders to the avatar.
    /// </summary>
    /// <param name="spawnedAvatar"><inheritdoc cref="AbstractAvatarSetup.SetUp"/></param>
    public override void SetUp(ref Animator spawnedAvatar)
    {
        spawnedBodypartsColliders = SpawnColliders();
        MapBodyparts(ref spawnedAvatar);
    }

    private PlayerBodypartsColliders SpawnColliders()
    {
        return Instantiate(_playerBodypartsColliders, transform, false);
    }

    private void MapBodyparts(ref Animator playerAnimator)
    {
        foreach (var playerBodypart in spawnedBodypartsColliders.PlayerBodyparts)
        {
            Transform boneTransform = playerAnimator.GetBoneTransform(playerBodypart.AttachToBone);
            playerBodypart.transform.parent = boneTransform;
            playerBodypart.transform.localPosition = Vector3.zero;
            playerBodypart.transform.localRotation = Quaternion.identity;
            playerBodypart.transform.localScale = Vector3.one;
        }

        Destroy(spawnedBodypartsColliders.gameObject);
    }
}
using Unity.XR.CoreUtils;
using UnityEngine;

/// <summary>
/// <para>Class that creates the player avatar in the scene.</para>
/// <param name="avatarSetups">the list of <see cref="AbstractAvatarSetup"/></param>
/// </summary>
public class PlayerAvatar : MonoBehaviour
{
    [SerializeField] private AbstractAvatarSetup[] _avatarSetups;

    /// <summary>
    /// The player avatar Animator spawned in the scene.
    /// </summary>
    protected Animator spawnedPlayer;

    private bool isPlayerSpawned;

    private void Awake()
    {
        isPlayerSpawned = false;
    }

    /// <summary>
    /// Creates a player avatar from the specified player's prefab animator.
    /// </summary>
    /// <param name="avatarPrefab">the specified player's prefab animator</param>
    public void CreatePlayerFromAvatar(Animator avatarPrefab)
    {
        if (DoesComponentContainErrors())
        {
            CustomLogger.Error(this, "The settings of the created avatar contain errors");
            return;
        }

        if (!avatarPrefab.avatar.isHuman)
        {
            CustomLogger.Error(this, "The avatar's skeleton is not humanoid");
            return;
        }

        spawnedPlayer = SpawnPlayer(avatarPrefab);
        AvatarSetup(ref spawnedPlayer);
        isPlayerSpawned = true;
    }

    private bool DoesComponentContainErrors()
    {
        if (isPlayerSpawned)
        {
            return true;
        }

        foreach (var avatarSetup in _avatarSetups)
        {
            if (avatarSetup.ComponentContainsErrors())
            {
                return true;
            }
        }

        return false;
    }

    private Animator SpawnPlayer(Animator avatarPrefab)
    {
        Animator spawnedAvatar = Instantiate(avatarPrefab, transform, false);
        
        SetAvatarsLayer(spawnedAvatar.gameObject);
        
        return spawnedAvatar;
    }

    private static void SetAvatarsLayer(GameObject spawnedAvatar)
    {
        int layerIndex = CheckCameraLayersRendering.GetLayerIndexOrElseThrow("MyAvatar");

        spawnedAvatar.SetLayerRecursively(layerIndex);
    }

    private void AvatarSetup(ref Animator playerAnimator)
    {
        foreach (var avatarSetup in _avatarSetups)
        {
            avatarSetup.SetUp(ref playerAnimator);
        }
    }

    /// <summary>
    /// Destroy a player avatar.
    /// </summary>
    public void DestroyPlayerAvatar()
    {
        if (spawnedPlayer != null)
        {
            Destroy(spawnedPlayer.gameObject);
        }
    }
}
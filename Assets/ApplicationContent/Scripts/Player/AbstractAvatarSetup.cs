using UnityEngine;

/// <summary>
/// <para>Abstract avatar setup class.</para>
/// </summary>
public abstract class AbstractAvatarSetup : MonoBehaviour
{
    /// <summary>
    /// Checks for component errors before performing setup.
    /// </summary>
    /// <returns>true if component contains errors, false if the component can make the setup</returns>
    public virtual bool ComponentContainsErrors()
    {
        return false;
    }

    /// <summary>
    /// Setup spawned avatar.
    /// </summary>
    /// <param name="spawnedAvatar">the avatar that was spawned</param>
    public abstract void SetUp(ref Animator spawnedAvatar);
}
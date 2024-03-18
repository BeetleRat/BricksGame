using UnityEngine;

/// <summary>
/// <para>Class storing colliders of player's bodyparts.</para>
/// </summary>
public sealed class PlayerBodypartsColliders : MonoBehaviour
{
    [SerializeField] private PlayerBodypart[] _playerBodyparts;

    /// <summary>
    /// The list of player bodyparts.
    /// </summary>
    public PlayerBodypart[] PlayerBodyparts => _playerBodyparts;
}
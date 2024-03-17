using System;
using UnityEngine;


[Serializable]
public enum PlayerControlType
{
    KEYBOARD,
    KINECT
}

/// <summary>
/// <para>Class of player object.</para>
/// <remarks>Player object must have a <see cref="PlayerAvatar"/> component among its children</remarks>
/// <param name="playerControlType">the player control type</param>
/// </summary>
public sealed class Player : MonoBehaviour
{
    [SerializeField] private PlayerControlType _playerControlType;

    /// <summary>
    /// The player control type.
    /// </summary>
    public PlayerControlType PlayerControlType => _playerControlType;

    private PlayerAvatar playerAvatar;

    /// <summary>
    /// The <see cref="PlayerAvatar"/> component.
    /// </summary>
    public PlayerAvatar PlayerAvatar => playerAvatar;

    private void Awake()
    {
        playerAvatar = GetComponentInChildren<PlayerAvatar>();
    }
}
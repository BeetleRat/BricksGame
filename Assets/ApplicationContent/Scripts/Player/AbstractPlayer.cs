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
/// </summary>
public abstract class AbstractPlayer : MonoBehaviour
{
    /// <summary>
    /// The player control type.
    /// </summary>
    public abstract PlayerControlType PlayerControlType { get; }

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
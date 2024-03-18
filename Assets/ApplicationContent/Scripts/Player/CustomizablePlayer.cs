using UnityEngine;

/// <summary>
/// <inheritdoc cref="Player"/>
/// <param name="playerControlType">the player control type</param>
/// </summary>
public sealed class CustomizablePlayer : Player
{
    [SerializeField] private PlayerControlType _playerControlType;
    public override PlayerControlType PlayerControlType => _playerControlType;
}
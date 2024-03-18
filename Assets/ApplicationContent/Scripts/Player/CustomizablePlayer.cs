using UnityEngine;

/// <summary>
/// <inheritdoc cref="AbstractPlayer"/>
/// <param name="playerControlType">the player control type</param>
/// </summary>
public sealed class CustomizablePlayer : AbstractPlayer
{
    [SerializeField] private PlayerControlType _playerControlType;
    public override PlayerControlType PlayerControlType => _playerControlType;
}
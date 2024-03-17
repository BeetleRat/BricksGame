using DG.Tweening;
using UnityEngine;

/// <summary>
/// <para>
/// A <see cref="AbstractProjectile">projectile</see> that looks like a brick.
/// If it comes in contact with player, it will give them health points.
/// </para>
/// </summary>
public sealed class HealingBrick : AbstractProjectile
{
    [Tooltip("The amount of health that will be given to the player when the projectile comes into contact with the players bodypart")]
    [Range(0, 100)]
    [SerializeField] private int _hpAdded = 1;

    [Header("Fade out settings")]
    [SerializeField] private float _transformMultiplier;
    [SerializeField] private float _magnificationSpeed;
    [SerializeField] private float _fadeOutSpeed;

    private bool isFadeOut;

    /// <summary>
    /// <inheritdoc cref="AbstractProjectile.PerformAction"/>
    /// </summary>
    protected override void PerformAction()
    {
        levelManager?.AddHealthPoint(_hpAdded);
        FadeOut();
    }

    private void FadeOut()
    {
        transform.DOScale(transform.localScale * _transformMultiplier, _magnificationSpeed)
            .OnComplete(() => transform.DOScale(0, _fadeOutSpeed));
    }
}
using DG.Tweening;
using UnityEngine;

/// <summary>
/// <para>
/// A <see cref="AbstractProjectile">projectile</see> that looks like a brick.
/// If it comes in contact with player, it will increase the player's score.
/// </para>
/// </summary>
public sealed class ScoreBrick : AbstractProjectile
{
    [Tooltip("The number of points a player will get")]
    [Range(1, 100)]
    [SerializeField] private int _scorePoints;

    [Header("Fade out settings")]
    [SerializeField] private float _transformMultiplier;
    [SerializeField] private float _fadeOutSpeed;

    private bool isFadeOut;
    private int activeRenderersCount;

    /// <summary>
    /// <inheritdoc cref="AbstractProjectile.PerformAction"/>
    /// </summary>
    protected override void PerformAction()
    {
        levelManager.AddScore(_scorePoints);
        FadeOut();
    }

    private void FadeOut()
    {
        transform.DOScale(transform.localScale * _transformMultiplier, _fadeOutSpeed);

        var renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length > 0)
        {
            activeRenderersCount = renderers.Length;
            foreach (var renderer in renderers)
            {
                renderer.material.DOFade(0f, _fadeOutSpeed)
                    .OnComplete(TryToDestroyObject);
            }
        }
    }

    private void TryToDestroyObject()
    {
        activeRenderersCount--;
        if (activeRenderersCount <= 0)
        {
            Destroy(gameObject);
        }
    }
}
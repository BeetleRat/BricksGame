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
    [SerializeField] private ParticleSystem _fadeParticles;

    [Header("Audio settings")]
    [SerializeField] private AudioSource _actionAudioSource;

    private bool isFadeOut;

    /// <summary>
    /// <inheritdoc cref="AbstractProjectile.PerformAction"/>
    /// </summary>
    protected override void PerformAction()
    {
        levelManager?.AddHealthPoint(_hpAdded);
        if (_actionAudioSource != null)
        {
            _actionAudioSource.Play();
        }
        SpawnParticles();
        FadeOut();
    }

    private void FadeOut()
    {
        transform.DOScale(transform.localScale * _transformMultiplier, _magnificationSpeed)
            .OnComplete(() => transform.DOScale(0, _fadeOutSpeed));
    }
    
    private void SpawnParticles()
    {
        Vector3 particlesPosition = transform.position + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(0.5f, 1.5f), Random.Range(-0.3f, 0.3f));
        GameObject particles = Instantiate(_fadeParticles.gameObject, particlesPosition, Quaternion.identity);
        Destroy(particles, _fadeParticles.main.duration);
    }
}
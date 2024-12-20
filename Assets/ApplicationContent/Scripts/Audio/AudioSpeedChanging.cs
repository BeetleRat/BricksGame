using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>Specific music speed settings for a particular game speed state.</para>
/// </summary>
[Serializable]
sealed class MusicSpeed
{
    public Speed ForSpeed;
    public AudioSource Audio;
}

/// <summary>
/// <para>Component that controls music switching depending on the game speed.</para>
/// It is a <see cref="AbstractSpeedChangingComponent"/>
/// </summary>
public sealed class AudioSpeedChanging : AbstractSpeedChangingComponent
{
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private List<MusicSpeed> _speedSettings;

    private MusicSpeed currentSpeed;
    private bool isMixingActive = false;

    private void Awake()
    {
        ValidateSettings(ref _speedSettings);
    }

    private void ValidateSettings(ref List<MusicSpeed> speedSettings)
    {
        speedSettings.Sort((ss1, ss2) => ss1.ForSpeed - ss2.ForSpeed);
        RemoveDuplicatesFrom(ref speedSettings);
    }

    private void RemoveDuplicatesFrom(ref List<MusicSpeed> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            if (list[i].ForSpeed == list[i - 1].ForSpeed)
            {
                list.Remove(list[i]);
            }
        }
    }

    private void Start()
    {
        if (_speedSettings.Count == 0)
        {
            CustomLogger.Error("Speed Settings List is empty");
            return;
        }
        else
        {
            currentSpeed = _speedSettings[0];
        }
        
        _levelManager.GameStarted += StartAudioMixing;
        _levelManager.GameEnded += StopAudioMixing;
    }

    private void OnDestroy()
    {
        _levelManager.GameStarted -= StartAudioMixing;
        _levelManager.GameEnded -= StopAudioMixing;
    }

    private void StartAudioMixing()
    {
        isMixingActive = true;
        if (currentSpeed.Audio != null)
        {
            PlayClip(currentSpeed.Audio);  
        }
    }
    
    private void StopAudioMixing()
    {
        isMixingActive = false;
        
        if (_speedSettings.Count == 0)
        {
            CustomLogger.Error("Speed Settings List is empty");
            return;
        }

        ShutDownMusic();
        PlayDefault();
    }
    
    private void ShutDownMusic()
    {
        foreach (MusicSpeed musicSpeed in _speedSettings)
        {
            if (musicSpeed.Audio != null)
            {
                musicSpeed.Audio.Stop();
            }
        }
    }
    
    private void PlayDefault()
    {
        if (_speedSettings[0].Audio != null)
        {
            PlayClip(_speedSettings[0].Audio);
        }
    }

    /// <summary>
    /// <inheritdoc cref="AbstractSpeedChangingComponent.ChangeSpeed"/>
    /// </summary>
    /// <param name="speed"><inheritdoc cref="AbstractSpeedChangingComponent.ChangeSpeed"/></param>
    public override void ChangeSpeed(Speed speed)
    {
        if (_speedSettings.Count == 0)
        {
            CustomLogger.Error("Speed Settings List is empty");
            return;
        }
        
        foreach (MusicSpeed musicSpeed in _speedSettings)
        {
            if (musicSpeed.ForSpeed == speed)
            {
                if (currentSpeed.Audio == null && musicSpeed.Audio != null && isMixingActive)
                {
                    PlayClip(musicSpeed.Audio);
                }

                currentSpeed = musicSpeed;

                return;
            }
        }

        LogNoSpeedSettingsFor(speed);
    }

    private void PlayClip(AudioSource audioSource)
    {
        if (audioSource.clip != null)
        {
            audioSource.Play();
            Invoke(nameof(OnClipEnd), audioSource.clip.length - 0.1f); // Запускаем событие по окончании
        }
    }

    private void OnClipEnd()
    {
        if (currentSpeed.Audio != null && isMixingActive)
        {
            PlayClip(currentSpeed.Audio);
        }
    }
}
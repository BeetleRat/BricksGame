using System;
using UnityEngine;

/// <summary>
/// The Biofeedback control type.
/// </summary>
[Serializable]
public enum BiofeedbackControlType
{
    NONE,
    HEART_RATE
    //,ACCELERATION
}

/// <summary>
/// <para>A class responsible for customizing the player's controllers.</para>
/// </summary>
public sealed class ControllerManager : MonoBehaviour
{
    [Header("Scene settings")] 
    [SerializeField] private PlayerControlType _playerControlType;
    [SerializeField] private BiofeedbackControlType _biofeedbackControlType;
    [SerializeField] private Animator _playerAvatar;

    [Header("Manager components")]
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private AbstractBiofeedbackManager[] _biofeedbackManagers;
    [SerializeField] private AbstractPlayer[] _availablePlayers;

    private void Start()
    {
        DisableBiofeedbackManagers();
        DisablePlayers();
    }

    private void DisableBiofeedbackManagers()
    {
        foreach (var biofeedbackManager in _biofeedbackManagers)
        {
            if (biofeedbackManager.PulseControl != _biofeedbackControlType)
            {
                biofeedbackManager.gameObject.SetActive(false);
            }
            else
            {
                _levelManager.BiofeedbackManager = biofeedbackManager;
            }
        }
    }

    private void DisablePlayers()
    {
        foreach (var player in _availablePlayers)
        {
            if (player.PlayerControlType != _playerControlType)
            {
                player.gameObject.SetActive(false);
            }
            else
            {
                player.PlayerAvatar.CreatePlayerFromAvatar(_playerAvatar);
            }
        }
    }
}
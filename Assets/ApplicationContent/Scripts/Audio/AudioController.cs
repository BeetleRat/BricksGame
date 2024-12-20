using UnityEngine;


/// <summary>
/// <para>A class that manipulates the sounds of the stage.</para>
/// </summary>
public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource[] _controlledSources;

    private bool _isMute;

    private void Start()
    {
        _isMute = false;
        SetMuteToAll();
    }

    /// <summary>
    /// <para>On/off the specific audio source.</para>
    /// <param name="index">switching source index</param>
    /// </summary>
    public void SwitchMute(int index)
    {
        if (index >= 0 && index < _controlledSources.Length)
        {
            _controlledSources[index].mute = !_controlledSources[index].mute;
        }
    }

    /// <summary>
    /// <para>On/off all audio sources.</para>
    /// </summary>
    public void SwitchMuteToAll()
    {
        _isMute = !_isMute;
        SetMuteToAll();
    }

    private void SetMuteToAll()
    {
        foreach (AudioSource audioSource in _controlledSources)
        {
            audioSource.mute = _isMute;
        }
    }
}
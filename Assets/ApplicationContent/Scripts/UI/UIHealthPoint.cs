using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// <para>Script for displaying one health point in UI.</para>
/// </summary>
public sealed class UIHealthPoint : MonoBehaviour
{
    [SerializeField] private Image _activeHeart;
    [SerializeField] private Image _notActiveHeart;
    [SerializeField] private bool _isActive;

    /// <summary>
    /// Sets active state for health point.
    /// </summary>
    /// <value>Is health point active</value>
    public bool IsActive
    {
        set
        {
            _isActive = value;
            _activeHeart.gameObject.SetActive(_isActive);
            _notActiveHeart.gameObject.SetActive(!_isActive);
        }
    }

    private void Start()
    {
        _activeHeart.gameObject.SetActive(_isActive);
        _notActiveHeart.gameObject.SetActive(!_isActive);
    }
}
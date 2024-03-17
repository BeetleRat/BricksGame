using UnityEngine;
using TMPro;

/// <summary>
/// TODO: XML-Doc
/// <para>Class description</para>
/// </summary>
public sealed class UINotificationBar : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    public string Text
    {
        get => _text.text;
        set => _text.text = value;
    }
}
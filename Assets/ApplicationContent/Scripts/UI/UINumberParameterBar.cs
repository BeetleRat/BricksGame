using System;
using TMPro;
using UnityEngine;

/// <summary>
/// <para>Class displays the value of a numeric parameter on the UI.</para>
/// Display format: "Parameter name: Value"
/// </summary>
public class UINumberParameterBar : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private string _parameterName;
    [Range(0, 6)]
    [SerializeField] private int _decimalPoint;

    [SerializeField] private bool _barVisibility;

    /// <summary>
    /// <para>Parameter value displayed in UI each frame.</para>
    /// </summary>
    protected float parameterValue = 0;

    private void Update()
    {
        OnUpdate();
    }

    protected virtual void OnUpdate()
    {
        if (_barVisibility)
        {
            _text.text = _parameterName + ": " + Math.Round(parameterValue, _decimalPoint);
        }
        else
        {
            _text.text = "";
        }
    }

    /// <summary>
    /// <para>Sets bar visibility.</para>
    /// </summary>
    /// <param name="visible">Is bar visible</param>
    public void SetVisible(bool visible)
    {
        _barVisibility = visible;
    }

    /// <summary>
    /// <para>Sets showed parameter name.</para>
    /// </summary>
    /// <param name="parameterName">The parameter name</param>
    public void SetParameterName(string parameterName)
    {
        _parameterName = parameterName;
    }

    /// <summary>
    /// <para>Sets showed parameter value.</para>
    /// </summary>
    /// <param name="parameterValue">The parameter value</param>
    public virtual void SetParameterValue(float parameterValue)
    {
        this.parameterValue = parameterValue;
    }

    /// <summary>
    /// <para>Sets the color of the bar text.</para>
    /// </summary>
    /// <param name="newColor">The new text color</param>
    public void SetColor(Color newColor)
    {
        _text.color = newColor;
    }
}
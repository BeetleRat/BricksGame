using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// <para>Enumeration of heart rate states returned by biofeedback.</para>
/// </summary>
[Serializable]
public enum PulseCondition
{
    SLOW,
    NORMAL,
    FAST,
    CRITICAL
}

/// <summary>
/// <para>Structure correlating the numerical value of the biofeedback indicator, heart rate state and color of the debug bar.</para>
/// </summary>
[Serializable]
public class PulseRateCondition
{
    [Tooltip("If pulse rate lower then selected, then set selected condition")]
    public float Rate;

    public PulseCondition Condition;
    public Color UIBarColor;
}

/// <summary>
/// <para> Abstract biofeedback manager class.</para>
/// It provides his subclass PulseConditionChange action that they must call to interact with game managers.
/// It also provides a list of <see cref="PulseRateCondition"/> and debug bar as script parameters.
/// And it provides <see cref="PulseReceiver"/> component and <see cref="PulseCondition"/> variable for biofeedback interaction.
/// </summary>
[RequireComponent(typeof(PulseReceiver))]
public abstract class AbstractBiofeedbackManager : MonoBehaviour
{
    /// <summary>
    /// <para>Action triggered when <see cref="PulseCondition"/> change.</para>
    /// </summary>
    public abstract event UnityAction<PulseCondition> PulseConditionChange;

    /// <summary>
    /// <para>List of <see cref="PulseRateCondition"/> used to determine heart rate status.</para>
    /// </summary>
    [SerializeField] protected List<PulseRateCondition> _pulseRateConditions;

    /// <summary>
    /// <para><see cref="UINumberParameterBar"/> used to display the current parameter value on UI in debug mode.</para>
    /// </summary>
    [SerializeField] protected UINumberParameterBar _debugUiBar;

    [SerializeField] private bool _showDebugUiBar;

    [Header("[Optional]")] 
    [SerializeField] private string _uiBarParameterName;

    /// <summary>
    /// <para><see cref="PulseReceiver"/> component.</para>
    /// </summary>
    protected PulseReceiver pulseReceiver;

    /// <summary>
    /// <para>The <see cref="BiofeedbackControlType"/></para>
    /// </summary>
    protected BiofeedbackControlType _pulseControl;

    /// <summary>
    /// <para>Current <see cref="PulseCondition"/> state.</para>
    /// </summary>
    protected PulseCondition pulseCondition = PulseCondition.NORMAL;

    /// <summary>
    /// <para>The <see cref="BiofeedbackControlType"/></para>
    /// </summary>
    public BiofeedbackControlType PulseControl => _pulseControl;

    /// <summary>
    /// <para>Current <see cref="PulseCondition"/> state.</para>
    /// </summary>
    public PulseCondition PulseCondition => pulseCondition;

    private void Start()
    {
        OnStart();
    }

    protected virtual void OnStart()
    {
        pulseReceiver = GetComponent<PulseReceiver>();
        ValidateConditionsList();
        _debugUiBar?.SetVisible(_showDebugUiBar);
        if (_uiBarParameterName.Length > 0)
        {
            _debugUiBar?.SetParameterName(_uiBarParameterName);
        }
    }

    private void ValidateConditionsList()
    {
        if (_pulseRateConditions.Count == 0)
        {
            CustomLogger.Error(this, "Conditions List is empty");
            return;
        }

        _pulseRateConditions.Sort((prc1, prc2) => (int)(prc1.Rate * 100 - prc2.Rate * 100));
        RemoveDuplicate(ref _pulseRateConditions);
    }

    private void RemoveDuplicate(ref List<PulseRateCondition> pulseRateConditions)
    {
        for (int i = pulseRateConditions.Count - 1; i >= 1; i--)
        {
            if (pulseRateConditions[i].Rate == pulseRateConditions[i - 1].Rate)
            {
                pulseRateConditions.Remove(pulseRateConditions[i]);
            }
        }
    }
}
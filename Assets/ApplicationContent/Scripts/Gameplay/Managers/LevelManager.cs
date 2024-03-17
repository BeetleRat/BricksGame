using System.Threading;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// <para>Level manager script that controls events occurring in the level scene.</para>
/// </summary>
public sealed class LevelManager : MonoBehaviour
{
    /// <summary>
    /// <para>Action triggered when the game starts.</para>
    /// </summary>
    public event UnityAction GameStarted;

    /// <summary>
    /// <para>Action triggered when the game ends.</para>
    /// </summary>
    public event UnityAction GameEnded;

    [SerializeField] private HpManager _hpManager;
    [Range(1, 100)] 
    [SerializeField] private int _hpCount;
    [SerializeField] private UISmoothChangeNumberParameterBar _scoreBar;
    [SerializeField] private UINotificationBar _notificationBar;
    [Range(0, 20)]
    [SerializeField] private int _waitBeforeStart = 0;
    [SerializeField] private MultipleButtons _multipleButtons;
    [SerializeField] private AbstractSpeedChangingComponent[] _speedChangingComponents;
    
    private AbstractBiofeedbackManager biofeedbackManager;

    private bool isGameStart = false;
    private CancellationTokenSource timerToken;

    public AbstractBiofeedbackManager BiofeedbackManager
    {
        set
        {
            if (biofeedbackManager != null)
            {
                biofeedbackManager.PulseConditionChange -= ChangeGameSpeedAccordingToPulse;
            }

            biofeedbackManager = value;
            biofeedbackManager.PulseConditionChange += ChangeGameSpeedAccordingToPulse;
        }
    }

    private void Start()
    {
        Cursor.visible = false;
        _scoreBar.SetParameterValue(0);
        _notificationBar.Text = "";
        _hpManager.OutOfHP += EndGame;
        _multipleButtons.ButtonsPressed += StartGame;
        timerToken = new CancellationTokenSource();
        timerToken.Token.Register(() => CustomLogger.Log(this, "Async timer canceled"));
    }

    private void OnDestroy()
    {
        _hpManager.OutOfHP -= EndGame;
        _multipleButtons.ButtonsPressed -= StartGame;
        timerToken.Cancel();
        if (biofeedbackManager != null)
        {
            biofeedbackManager.PulseConditionChange -= ChangeGameSpeedAccordingToPulse;
        }
    }

    /// <summary>
    /// <para>Start game method.</para>
    /// </summary>
    public void StartGame()
    {
        AsyncCountDown.Setup(timerToken.Token)
            .Callback(CountDownBeforeStart)
            .Count(_waitBeforeStart + 1)
            .PeriodBetweenCallback(1, TimeUnit.SECONDS)
            .Start();
    }

    private void CountDownBeforeStart(long count)
    {
        _notificationBar.Text = count.ToString();
        if (count == 0)
        {
            _notificationBar.Text = "";
            StartLevel();
        }
    }

    private void StartLevel()
    {
        _multipleButtons.Hide();
        if (biofeedbackManager?.PulseCondition == PulseCondition.CRITICAL)
        {
            CustomLogger.Log(this, "Critical pulse condition. Request to start the game rejected");
            _multipleButtons.Show();
            return;
        }
        
        CustomLogger.Log(this, "The game has started");
        _hpManager.HpCount = _hpCount;
        _scoreBar.SetParameterValue(0);
        
        GameStarted?.Invoke();
    }

    /// <summary>
    /// <para>End game method.</para>
    /// </summary>
    public void EndGame()
    {
        CustomLogger.Log(this, "The game is over");
        isGameStart = false;
        GameEnded?.Invoke();
        _multipleButtons.Show();
    }

    /// <summary>
    /// <para>Method that takes away selected count of health points from player.</para>
    /// </summary>
    /// <param name="count">Taken away health points count</param>
    public void DrainHealthPoint(int count)
    {
        _hpManager.SubtractHPFromCurrent(count);
    }

    /// <summary>
    /// <para>Method that increases a player's score by selected number of points.</para>
    /// </summary>
    /// <param name="count">Points count added to the player's score</param>
    public void AddScore(int count)
    {
        _scoreBar.AddValue(count);
    }

    /// <summary>
    /// <para>Method that give selected count of health points to player.</para>
    /// </summary>
    /// <param name="count">Given health points count</param>
    public void AddHealthPoint(int count)
    {
        _hpManager.AddHPToCurrent(count);
    }

    /// <summary>
    /// <para>Method that changes game speed according to <see cref="PulseCondition"/>.</para>
    /// </summary>
    /// <param name="pulseCondition"><see cref="PulseCondition"/> depending on which the game speed will be changed</param>
    public void ChangeGameSpeedAccordingToPulse(PulseCondition pulseCondition)
    {
        switch (pulseCondition)
        {
            case PulseCondition.CRITICAL:
                ChangeGameSpeed(Speed.STOP);
                break;
            case PulseCondition.SLOW:
                ChangeGameSpeed(Speed.FAST);
                break;
            case PulseCondition.NORMAL:
                ChangeGameSpeed(Speed.NORMAL);
                break;
            case PulseCondition.FAST:
                ChangeGameSpeed(Speed.SLOW);
                break;
        }
    }

    private void ChangeGameSpeed(Speed newSpeed)
    {
        foreach (AbstractSpeedChangingComponent speedChangingComponent in _speedChangingComponents)
        {
            speedChangingComponent.ChangeSpeed(newSpeed);
        }
    }

#if UNITY_EDITOR

    /// <summary>
    /// <para>Debug method for starting the game from the inspector by pressing the button.</para>
    /// </summary>
    [InspectorButton(Name = "Start game")]
    public void StartGameButton()
    {
        if (!isGameStart)
        {
            isGameStart = true;
            StartGame();
        }
    }

#endif
}
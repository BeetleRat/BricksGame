using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// <para>Component that controls the amount of player's health.</para>
/// <param name="hpCount">the amount of player's health</param>
/// <param name="hpBar">the <see cref="UIHpBar"/></param>
/// </summary>
public sealed class HpManager : MonoBehaviour
{
    /// <summary>
    /// <para>Action triggered when health points run out.</para>
    /// </summary>
    public event UnityAction OutOfHP;

    /// <summary>
    /// <para>Action triggered when the player's immortality starts.</para>
    /// </summary>
    public event UnityAction StartImmortality;

    /// <summary>
    /// <para>Action triggered when the player's immortality ends.</para>
    /// </summary>
    public event UnityAction StopImmortality;

    [SerializeField] private int _hpCount;
    [Range(0, 10)]
    [SerializeField] private float _immortalityAfterDamageTimeSec = 3f;
    [SerializeField] private UIHpBar _hpBar;

    private int currentHP;
    private float immortalityTime;
    private bool isPlayerImmortal = false;

    /// <summary>
    /// <para>Sets new health point count.</para>
    /// <value>new health point count</value>
    /// </summary>
    public int HpCount
    {
        set
        {
            if (_hpCount != value)
            {
                _hpCount = value;
                currentHP = value;
                _hpBar.CreateNewBar(_hpCount);
            }
            else
            {
                ResetHP();
            }
        }
    }

    private void Update()
    {
        if (immortalityTime <= 0)
        {
            StopPlayerImmortality();
        }
        else
        {
            immortalityTime -= Time.deltaTime;
        }
    }

    /// <summary>
    /// <para>Set all health points to the active state without changing their quantity.</para>
    /// </summary>
    public void ResetHP()
    {
        ChangeHP(_hpCount);
    }

    /// <summary>
    /// <para>Add health point to current.</para>
    /// </summary>
    /// <param name="value">Added value</param>
    public void AddHPToCurrent(int value)
    {
        int newHP = currentHP + value;
        ChangeHP(newHP);
    }

    /// <summary>
    /// <para>Subtract health point from current.</para>
    /// </summary>
    /// <param name="value">Subtracted value</param>
    public void SubtractHPFromCurrent(int value)
    {
        if (immortalityTime > 0)
        {
            return;
        }

        int newHP = currentHP - value;
        StartPlayerImmortality();
        ChangeHP(newHP);
    }

    private void StartPlayerImmortality()
    {
        isPlayerImmortal = true;
        immortalityTime = _immortalityAfterDamageTimeSec;
        StartImmortality?.Invoke();
    }

    private void StopPlayerImmortality()
    {
        if (isPlayerImmortal)
        {
            isPlayerImmortal = false;
            immortalityTime = 0f;
            StopImmortality?.Invoke();
        }
    }

    private void ChangeHP(int newHP)
    {
        if (newHP > _hpCount || newHP < 0)
        {
            return;
        }

        bool activeState = newHP > currentHP;
        while (currentHP != newHP)
        {
            currentHP = activeState ? currentHP : currentHP - 1;
            _hpBar[currentHP] = activeState;
            currentHP = activeState ? currentHP + 1 : currentHP;
        }

        if (currentHP == 0)
        {
            OutOfHP?.Invoke();
            _hpBar.RemoveOldBar();
        }
    }
}
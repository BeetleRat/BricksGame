using System;
using UnityEngine;

/// <summary>
/// Foot procedural animation settings
/// </summary>
[Serializable]
public class FootAnimationSettings
{
    [Range(0f, 1f)]
    public float FootPositionWeight = 1f;
    [Range(0f, 1f)]
    public float FootRotationWeight = 1f;
    public Vector3 RaycastOffset = new Vector3(0, 0.5f, 0);
}

/// <summary>
/// <para>Class providing foot position offset when stepping on a surface.</para>
/// </summary>
[RequireComponent(typeof(Animator))]
public sealed class LowerbodyAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Vector3 _footOffset = new Vector3(0f, 0.1f, 0f);
    
    [SerializeField] private FootAnimationSettings _leftFootSettings = new FootAnimationSettings();
    [SerializeField] private FootAnimationSettings _rightFootSettings = new FootAnimationSettings();

    /// <summary>
    /// The lowerbody animator
    /// </summary>
    public Animator Animator
    {
        set => _animator = value;
    }
    /// <summary>
    /// Foot offset from the ground surface
    /// </summary>
    public Vector3 FootOffset
    {
        set => _footOffset = value;
    }
    /// <summary>
    /// <see cref="FootAnimationSettings"/> for left foot
    /// </summary>
    public FootAnimationSettings LeftFootSettings
    {
        set => _leftFootSettings = value;
    }
    /// <summary>
    /// <see cref="FootAnimationSettings"/> for right foot
    /// </summary>
    public FootAnimationSettings RightFootSettings
    {
        set => _rightFootSettings = value;
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        SetFootPositionAndRotation(AvatarIKGoal.LeftFoot, _leftFootSettings);
        SetFootPositionAndRotation(AvatarIKGoal.RightFoot, _rightFootSettings);
    }

    private void SetFootPositionAndRotation(AvatarIKGoal avatarIKGoal, FootAnimationSettings footAnimationSettings)
    {
        Vector3 footPosition = _animator.GetIKPosition(avatarIKGoal);

        RaycastHit hitFloor;
        bool isFootOnFloor = Physics.Raycast(footPosition + footAnimationSettings.RaycastOffset, Vector3.down, out hitFloor);

        if (isFootOnFloor)
        {
            _animator.SetIKPositionWeight(avatarIKGoal, footAnimationSettings.FootPositionWeight);
            _animator.SetIKPosition(avatarIKGoal, hitFloor.point + _footOffset);

            Quaternion footRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, hitFloor.normal), hitFloor.normal);
            _animator.SetIKRotationWeight(avatarIKGoal, footAnimationSettings.FootRotationWeight);
            _animator.SetIKRotation(avatarIKGoal, footRotation);
        }
        else
        {
            _animator.SetIKPositionWeight(avatarIKGoal, 0);
        }
    }
}
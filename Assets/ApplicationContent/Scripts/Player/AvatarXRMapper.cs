using UnityEngine;

/// <summary>
/// <para>Support class that maps a bone object to a target object.</para>
/// </summary>
[System.Serializable]
public class MapTransform
{
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _rig;

    [SerializeField] private Vector3 _trackingPositionOffset;
    [SerializeField] private Vector3 _trackingRotationOffset;

    /// <summary>
    /// Target object transform
    /// </summary>
    public Transform Target
    {
        get => _target;
        set => _target = value;
    }
    /// <summary>
    /// Rig object transform
    /// </summary>
    public Transform Rig
    {
        set => _rig = value;
    }
    /// <summary>
    /// Mapping result position offset
    /// </summary>
    public Vector3 TrackingPositionOffset
    {
        set => _trackingPositionOffset = value;
    }
    /// <summary>
    /// Mapping result rotation offset
    /// </summary>
    public Vector3 TrackingRotationOffset
    {
        set => _trackingRotationOffset = value;
    }

    /// <summary>
    /// <para>Mat rig object position and rotation to target object</para>
    /// </summary>
    public void MapRig()
    {
        _rig.position = _target.TransformPoint(_trackingPositionOffset);
        _rig.rotation = _target.rotation * Quaternion.Euler(_trackingRotationOffset);
    }
}

/// <summary>
/// <para>Class that maps avatar reference parts to XR controls.</para>
/// </summary>
public sealed class AvatarXRMapper : MonoBehaviour
{
    [SerializeField] private MapTransform _head;
    [SerializeField] private MapTransform _leftHand;
    [SerializeField] private MapTransform _rightHand;
    
    [SerializeField] private Transform _headRig;
    
    [SerializeField] private float _turnSmoothness = 5f;
    [SerializeField] private Vector3 _headBodyOffset = new Vector3(0, -0.5f, 0);

    /// <summary>
    /// Head mapping
    /// </summary>
    public MapTransform Head
    {
        get => _head;
        set => _head = value;
    }
    /// <summary>
    /// Left hand mapping
    /// </summary>
    public MapTransform LeftHand
    {
        set => _leftHand = value;
    }
    /// <summary>
    /// Right mapping
    /// </summary>
    public MapTransform RightHand
    {
        set => _rightHand = value;
    }
    /// <summary>
    /// The head rig transform
    /// </summary>
    public Transform HeadRig
    {
        set => _headRig = value;
    }
    /// <summary>
    /// Smoothness of avatar rotation to the current XR headset position
    /// </summary>
    public float TurnSmoothness
    {
        set => _turnSmoothness = value;
    }
    /// <summary>
    /// Avatar head offset from XR headset position
    /// </summary>
    public Vector3 HeadBodyOffset
    {
        set => _headBodyOffset = value;
    }

    private void FixedUpdate()
    {
        transform.position = _headRig.position + _headBodyOffset;
        transform.forward = Vector3.Lerp(transform.forward, Vector3.ProjectOnPlane(_headRig.forward, Vector3.up).normalized, Time.deltaTime * _turnSmoothness);
        
        _head.MapRig();
        _leftHand.MapRig();
        _rightHand.MapRig();
    }
}
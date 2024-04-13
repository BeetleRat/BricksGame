using UnityEngine;

/// <summary>
/// <para>Class that controls the walking animation switching.</para>
/// </summary>
[RequireComponent(typeof(Animator), typeof(AvatarXRMapper))]
public sealed class WalkingAnimatorController : MonoBehaviour
{
    private const string IS_WALKING = "isWalking";
    private const string FORWARD_DIRECTION = "ForwardDirection";
    private const string RIGHT_DIRECTION = "RightDirection";
    
    [SerializeField] private float _speedThreshold = 0.1f;
    [Range(0.0f, 1.0f)]
    [SerializeField] private float _smoothAnimation = 0.3f;
    
    private Animator animator;
    private Vector3 previousPosition;
    private AvatarXRMapper avatarMapper;

    private void Start()
    {
        animator = GetComponent<Animator>();
        avatarMapper = GetComponent<AvatarXRMapper>();
        previousPosition = avatarMapper.Head.Target.position;
    }

    private void Update()
    {
        Vector3 headsetLocalSpeed = GetHeadsetLocalSpeed();

        SetAnimatorParameters(headsetLocalSpeed);
    }

    private Vector3 GetHeadsetLocalSpeed()
    {
        Vector3 currentPosition = avatarMapper.Head.Target.position;

        Vector3 headsetSpeed = (currentPosition - previousPosition) / Time.deltaTime;
        headsetSpeed.y = 0;

        Vector3 headsetLocalSpeed = transform.InverseTransformDirection(headsetSpeed);

        previousPosition = currentPosition;
        return headsetLocalSpeed;
    }

    private void SetAnimatorParameters(Vector3 speed)
    {
        float previousForwardDirection = animator.GetFloat(FORWARD_DIRECTION);
        float previousRightDirection = animator.GetFloat(RIGHT_DIRECTION);

        animator.SetBool(IS_WALKING, speed.magnitude > _speedThreshold);
        animator.SetFloat(FORWARD_DIRECTION, Mathf.Lerp(previousForwardDirection, Mathf.Clamp(speed.x, -1, 1), _smoothAnimation));
        animator.SetFloat(RIGHT_DIRECTION, Mathf.Lerp(previousRightDirection, Mathf.Clamp(speed.z, -1, 1), _smoothAnimation));
    }
}
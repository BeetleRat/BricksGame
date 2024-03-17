using UnityEngine;

/// <summary>
/// <para>A component used to specify the trajectory of an object.</para>
/// </summary>
[ExecuteAlways]
public sealed class ObjectMovement : MonoBehaviour
{
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _endPoint;

    /// <summary>
    /// The starting point of the object movement.
    /// </summary>
    public Transform StartPoint => _startPoint;

    /// <summary>
    /// The ending point of the object movement.
    /// </summary>
    public Transform EndPoint => _endPoint;

    private void Start()
    {
        if (Application.IsPlaying(this))
        {
            DestroyChildren(_startPoint);
            DestroyChildren(_endPoint);
        }
    }

    private void DestroyChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }

    private void Update()
    {
        if (_startPoint && _endPoint && !Application.isPlaying)
        {
            Color drawColor = _endPoint.localPosition.z - _startPoint.localPosition.z > 0 ? Color.blue : Color.red;
            _startPoint.localPosition = new Vector3(0, 0, 0);
            _endPoint.localPosition = new Vector3(0, 0, _endPoint.localPosition.z);
            Debug.DrawLine(_startPoint.position, _endPoint.position, drawColor);
        }
    }
}
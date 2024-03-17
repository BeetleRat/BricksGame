using UnityEngine;

/// <summary>
/// <para>Component used to define a player's body part.</para>
/// </summary>
public sealed class PlayerBodypart : MonoBehaviour
{
    [SerializeField] private HumanBodyBones _attachToBone;

    public HumanBodyBones AttachToBone => _attachToBone;
}
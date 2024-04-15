using UnityEngine;

/// <summary>
/// TODO: XML-Doc
/// <para></para>
/// </summary>
public sealed class UISetup : AbstractAvatarSetup
{
    [SerializeField] private GameObject _ui;
    [SerializeField] private Vector3 _position;
    [SerializeField] private float _width;
    [SerializeField] private float _height;
    [SerializeField] private Vector3 _scale = Vector3.one;

    public override bool ComponentContainsErrors()
    {
        bool containsErrors = false;

        if (_ui == null)
        {
            containsErrors = true;
            CustomLogger.Error(this, "UI not set");
        }

        return containsErrors;
    }

    public override void SetUp(ref Animator spawnedAvatar)
    {
        RectTransform rect = _ui.GetComponent<RectTransform>();
        rect.position = _position;
        rect.sizeDelta = new Vector2(_width, _height);
        rect.localScale = _scale;
    }
}
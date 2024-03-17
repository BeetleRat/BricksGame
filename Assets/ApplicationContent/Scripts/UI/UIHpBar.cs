using UnityEngine;

/// <summary>
/// <para>Script for displaying health bar in UI. Health bar consists of health points - UIHealthPoint.</para>
/// <param name="uiHealthPointPrefab">the <see cref="UIHealthPoint"/> prefab</param>
/// <param name="prefabSpacing">the spacing between spawn prefabs</param>
/// </summary>
public sealed class UIHpBar : MonoBehaviour
{
    [SerializeField] private UIHealthPoint _uiHealthPointPrefab;
    [Range(0, 100)] 
    [SerializeField] private float _prefabSpacing;

    private UIHealthPoint[] hpImagesArray;

    private float hpImageWidth;
    private float hpImageHeight;

    /// <summary>
    /// Sets the active status for health points.
    /// </summary>
    /// <param name="index">the index of health point</param>
    public bool this[int index]
    {
        set
        {
            if (index < 0 && index >= hpImagesArray.Length)
            {
                CustomLogger.Error(this, "Attempting to change the active status of a non-existing health point array index.");
                return;
            }

            hpImagesArray[index].IsActive = value;
        }
    }

    /// <summary>
    /// <para>Removes the old hp bar.</para>
    /// </summary>
    public void RemoveOldBar()
    {
        if (hpImagesArray != null)
        {
            for (int i = hpImagesArray.Length - 1; i >= 0; i--)
            {
                Destroy(hpImagesArray[i].gameObject);
            }

            hpImagesArray = null;
        }
    }

    /// <summary>
    /// <para>Creates a new hp bar with the specified amount of hp.</para>
    /// </summary>
    /// <param name="hpCount">the specified amount of hp</param>
    public void CreateNewBar(int hpCount)
    {
        if (hpImagesArray != null)
        {
            RemoveOldBar();
        }

        hpImagesArray = new UIHealthPoint[hpCount];
        Transform prefabTransform = _uiHealthPointPrefab.transform;
        Rect imageRectTransform = ((RectTransform)prefabTransform).rect;
        hpImageWidth = imageRectTransform.width * prefabTransform.localScale.x;
        hpImageHeight = imageRectTransform.height * prefabTransform.localScale.y;
        for (int i = 0; i < hpImagesArray.Length; i++)
        {
            var spawnedHeart = SpawnHeart(i);
            hpImagesArray[i] = spawnedHeart;
            spawnedHeart.IsActive = true;
        }
    }

    private UIHealthPoint SpawnHeart(int xOffset)
    {
        float fullXOffset = -xOffset * hpImageWidth - _prefabSpacing - hpImageWidth / 2;
        float fullYOffset = -hpImageHeight / 2;
        Vector3 spawnPosition = transform.position + new Vector3(fullXOffset, fullYOffset);

        UIHealthPoint spawnedUIHealthPoint =
            Instantiate(_uiHealthPointPrefab.gameObject, spawnPosition, Quaternion.identity)
                .GetComponent<UIHealthPoint>();
        spawnedUIHealthPoint.transform.SetParent(transform);

        return spawnedUIHealthPoint;
    }
}
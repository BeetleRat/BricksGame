using UnityEngine;


/// <summary>
/// <para>Component that enables/disables rendering of specified layers for the camera.</para>
/// <param name="layers">the specified layers</param>
/// <param name="renderLayers">true if the specified layers should be rendered, false otherwise</param>
/// </summary>
[RequireComponent(typeof(Camera))]
public sealed class CheckCameraLayersRendering : MonoBehaviour
{
    [SerializeField] private string[] _layers;
    [SerializeField] private bool _renderLayers;

    /// <summary>
    /// <para>Gets the layer index by its name.</para>>
    /// </summary>
    /// <param name="layerName">the layer name</param>
    /// <returns>the layer index if it exists</returns>
    /// <exception cref="UnassignedReferenceException">if there is no layer with this name</exception>
    public static int GetLayerIndexOrElseThrow(string layerName)
    {
        int layerIndex = LayerMask.NameToLayer(layerName);
        if (layerIndex == -1)
        {
            throw new UnassignedReferenceException($"Layer [{layerName}] must be assigned in Layer Manager.");
        }

        return layerIndex;
    }

    private void Start()
    {
        Camera camera = GetComponent<Camera>();

        foreach (string layerName in _layers)
        {
            GetLayerIndexOrElseThrow(layerName);
            if (_renderLayers)
            {
                camera.cullingMask |= LayerMask.GetMask(layerName);
            }
            else
            {
                camera.cullingMask ^= LayerMask.GetMask(layerName);
            }
        }
    }
}
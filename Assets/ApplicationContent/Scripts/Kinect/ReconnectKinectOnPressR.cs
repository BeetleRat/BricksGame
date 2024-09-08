using UnityEngine;

public sealed class ReconnectKinectOnPressR : MonoBehaviour
{
    [SerializeField] private PuppetAvatarDeviceConfiguration _deviceConfiguration;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            SkeletalTrackingProvider skeletalTrackingProvider = _deviceConfiguration.SkeletalTrackingProvider;
            skeletalTrackingProvider.ReconnectKinectDevice();
        }
    }
}
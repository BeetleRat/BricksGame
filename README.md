# BricksGame
## Before start
### Unzipping libraries
Unzip Plugins.zip. After unzipping Plugins.zip, the `directml.dll` file will appear in the root of the project. This file should be copied to the Unity editor folder .\Unity\Hub\Editor\version\Editor\Editor.

![explorer_PJVpTIbHIp](https://github.com/BeetleRat/BricksGame/assets/86663719/d0957dc7-671e-4def-bafc-03562fdaa4e4)
### Driver installation
If you do not already have drivers for Azure Kinect installed on your computer, you will need to install them.

Download [Kinect for Windows Runtime](https://www.microsoft.com/en-us/download/details.aspx?id=57578). Unzip the downloaded archive. Open the .\drivers\K4W\ folder. Right-click on the `kinectsensor.inf` file and click install.

![image](https://github.com/BeetleRat/BricksGame/assets/86663719/535af4ef-3e2c-4cdf-872b-1aeeae75bbbc)

Download [Kinect SDK](https://github.com/microsoft/Azure-Kinect-Sensor-SDK/blob/develop/docs/usage.md). In the repository you need to select the latest version of MSI. Install the downloaded file.

![image](https://github.com/BeetleRat/BricksGame/assets/86663719/4342fd97-c9cd-432d-bbe9-71ba2a73d4f7)

Then go back to the .\drivers\K4W\ folder. Run the KinectRintime-x64.msi installation file.

![image](https://github.com/BeetleRat/BricksGame/assets/86663719/dfdc78f1-8366-431a-b968-505bce93bd23)

Then install Microsoft Visual C++ 2015 Redistributable. The installation file is located in the same .\drivers\K4W\ folder.

![image](https://github.com/BeetleRat/BricksGame/assets/86663719/940f8262-0acd-4c18-9ba6-bc953f3bf950)

Once connected, the Kinect should be recognized as a camera. You can check this in Device Manager. Since the device is defined as a camera, applications must have permission to use the camera. To allow applications to use the camera, the appropriate permission must be set in the privacy settings.
## Game description
A game in which the player must dodge bricks flying in his direction and catch items that add points.
## Controls
The game has several types of controls:
- Keyboard control
- Azure Kinect control

## Biofeedback
An additional feature of the game is to speed up or slow down the gameplay depending on the player's heart rate. A special biofeedback device is used for this purpose.

# Azure Kinect OSC
A command line program that sends Azure Kinect body tracking data using [Open Sound Control (OSC)](https://ccrma.stanford.edu/groups/osc/index.html).

By default, `127.0.0.1` is the IP address used, and `12345` is the port used.

## Building and Usage
This project was created using .NET Framework 4.7.2 and Visual Studio 2019 version 16.11.1.

1. Ensure that you have the .NET Framework 4.7.2 development tools and Visual Studio 2019 version 16.11.1 or later installed.
2. Connect the Azure Kinect to your computer. You can check that the sensor is connected properly using the [Azure Kinect Viewer](https://learn.microsoft.com/en-us/azure/kinect-dk/azure-kinect-viewer) program included with the [Azure Kinect Sensor SDK](https://learn.microsoft.com/en-us/azure/kinect-dk/sensor-sdk-download).
3. Open `AzureKinectOSC.sln` in Visual Studio. Select x64 as the target platform and AzureKinectOSC as the startup project. Click the Start button.
4. Wait for Azure Kinect body tracking to start. The program displays the number of bodies detected for each frame of body data received. Press the Esc key to stop the program.

### Command-Line Options
- `address`: IP address the OSC sender sends to.
- `port`: Port the OSC sender sends to.
- `send-joint-orientation`: Whether to send joint orientation as a quaternion (set to true or false). False by default.
- `camera-fps`: Target body tracking frame rate (set to FPS30, FPS15, or FPS5). FPS30 by default.

#### Example Command-Line Usage
```powershell
.\AzureKinectOSC.exe --address 1.2.3.4 --port 7000 --send-joint-orientation false --camera-fps FPS30
```

## TestOSCDataReceiver Project
The TestOSCDataReceiver project included in the AzureKinectOSC solution can be used to verify that OSC data is being sent properly. This program will output any OSC packets received. The default IP address is `127.0.0.1`, and the default port is `12345`. The address and port to receive packets from can be set using command-line options in the same way as the AzureKinectOSC project.

## OSC Data Sent
For each frame of body data received, information for each joint is sent to `/bodies/{bodyId}/joints/{jointId}` for the specified address. Values sent:
- Joint X Position: float
- Joint Y Position: float
- Joint Z Position: float
If the `send-joint-orientation` option is set, the joint orientation quaternion is also sent for each joint:
- Joint X Orientation: float
- Joint Y Orientation: float
- Joint Z Orientation: float
- Joint W Orientation: float

## NuGet Package Dependencies
- Microsoft.Azure.Kinect.BodyTracking
- Rug.Osc ([source code](https://bitbucket.org/rugcode/rug.osc/src/master/))
- System.CommandLine.DragonFruit ([source code](https://github.com/dotnet/command-line-api))

## Credit
This repository uses some code from the `csharp_3d_viewer` sample in the Microsoft [`Azure-Kinect-Samples` repository](https://github.com/microsoft/Azure-Kinect-Samples/tree/master/body-tracking-samples/csharp_3d_viewer), which has the following license:

```
    MIT License

    Copyright (c) Microsoft Corporation. All rights reserved.

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE
```
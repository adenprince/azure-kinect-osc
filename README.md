# Azure Kinect OSC
A command line program that sends Azure Kinect body tracking data using [Open Sound Control (OSC)](https://ccrma.stanford.edu/groups/osc/index.html).

By default, `127.0.0.1` is the IP address used, and `12345` is the port used.

## Command-Line Options
- `address`: IP address the OSC sender sends to.
- `port`: Port the OSC sender sends to.
- `send-joint-orientation`: Whether to send joint orientation as a quaternion (set to true or false). False by default.
- `camera-fps`: Camera frame rate (set to FPS30, FPS15, or FPS5). FPS30 by default.

## Example Usage
```powershell
.\AzureKinectOSC.exe --address 1.2.3.4 --port 7000 --send-joint-orientation false --camera-fps FPS30
```

## OSC Data Sent
For each frame of body data received, information for each joint is sent to `/bodies/{bodyId}/joints/{jointId}` for the specified adddress. Values sent:
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
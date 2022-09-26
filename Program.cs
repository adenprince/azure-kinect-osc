namespace AzureKinectOSC
{
    using System;
    using Microsoft.Azure.Kinect.BodyTracking;
    using Microsoft.Azure.Kinect.Sensor;

    class Program
    {
        static void Main(string[] args)
        {
            // Open device
            using (Device device = Device.Open()) {
                device.StartCameras(new DeviceConfiguration() {
                    CameraFPS = FPS.FPS30,
                    ColorResolution = ColorResolution.Off,
                    DepthMode = DepthMode.NFOV_Unbinned,
                    WiredSyncMode = WiredSyncMode.Standalone,
                });

                var deviceCalibration = device.GetCalibration();

                using (Tracker tracker = Tracker.Create(deviceCalibration, new TrackerConfiguration() { ProcessingMode = TrackerProcessingMode.Gpu, SensorOrientation = SensorOrientation.Default })) {
                    while (true) {
                        using (Capture sensorCapture = device.GetCapture()) {
                            // Queue latest frame from the sensor
                            tracker.EnqueueCapture(sensorCapture);
                        }

                        // Try getting latest tracker frame
                        using (Frame frame = tracker.PopResult(TimeSpan.Zero, throwOnTimeout: false)) {
                            if (frame != null) {
                                Console.WriteLine($"{frame.NumberOfBodies} bodies detected");
                                for (uint i = 0; i < frame.NumberOfBodies; ++i) {
                                    var skeleton = frame.GetBodySkeleton(i);

                                    for (int jointId = 0; jointId < (int)JointId.Count; ++jointId) {
                                        var joint = skeleton.GetJoint(jointId);
                                    }
                                }
                            }
                        }

                        if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape) {
                            break;
                        }
                    }
                }
            }
        }
    }
}

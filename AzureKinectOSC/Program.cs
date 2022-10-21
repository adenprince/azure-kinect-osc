namespace AzureKinectOSC {
    using System;
    using System.Net;
    using Microsoft.Azure.Kinect.BodyTracking;
    using Microsoft.Azure.Kinect.Sensor;
    using Rug.Osc;

    class Program {
        static void Main(string address = "127.0.0.1", int port = 12345, bool sendJointOrientation = false, FPS cameraFps = FPS.FPS30) {
            if (cameraFps != FPS.FPS30 && cameraFps != FPS.FPS15 && cameraFps != FPS.FPS5) {
                Console.WriteLine("Set camera FPS to \"FPS30\", \"FPS15\", or \"FPS5\".");
                return;
            }

            Console.WriteLine($"Send joint orientation: {sendJointOrientation}.");
            Console.WriteLine($"Frame rate: {cameraFps}.");

            if (!TryOscSenderConnect(address, port, out OscSender sender)) {
                sender?.Dispose();
                return;
            }

            Console.WriteLine($"Connected to OSC Sender at {address}:{port}.");

            if (!TryDeviceOpen(out Device device)) {
                sender?.Dispose();
                device?.Dispose();
                return;
            }

            Console.WriteLine($"Opened Azure Kinect device with serial number: {device.SerialNum}.");

            try {
                device.StartCameras(new DeviceConfiguration() {
                    CameraFPS = cameraFps,
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
                                SendBodyData(sender, frame, sendJointOrientation);
                            }
                        }

                        if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape) {
                            break;
                        }
                    }
                }
            }
            finally {
                device?.Dispose();
                sender?.Dispose();
            }
        }

        public static bool TryOscSenderConnect(string address, int port, out OscSender sender) {
            sender = null;

            try {
                IPAddress ipAddress = IPAddress.Parse(address);
                sender = new OscSender(ipAddress, port);
                sender.Connect();
                return true;
            }
            catch (Exception e) {
                Console.WriteLine($"Failed to connect to OSC sender with exception message: {e.Message}");
                return false;
            }
        }

        public static bool TryDeviceOpen(out Device device) {
            device = null;

            try {
                device = Device.Open();
                return true;
            }
            catch (Exception e) {
                Console.WriteLine($"Failed to open Azure Kinect device with exception message: {e.Message}");
                return false;
            }
        }

        public static void SendBodyData(OscSender sender, Frame frame, bool sendJointOrientation) {
            Console.WriteLine($"{frame.NumberOfBodies} bodies detected");
            for (uint bodyId = 0; bodyId < frame.NumberOfBodies; ++bodyId) {
                var skeleton = frame.GetBodySkeleton(bodyId);

                for (int jointId = 0; jointId < (int)JointId.Count; ++jointId) {
                    var joint = skeleton.GetJoint(jointId);
                    string messageAddress = $"/bodies/{bodyId}/joints/{jointId}";
                    OscMessage message;

                    if (sendJointOrientation) {
                        message = new OscMessage(messageAddress,
                            joint.Position.X,
                            joint.Position.Y,
                            joint.Position.Z,
                            joint.Quaternion.X,
                            joint.Quaternion.Y,
                            joint.Quaternion.Z,
                            joint.Quaternion.W);
                    }
                    else {
                        message = new OscMessage(messageAddress,
                            joint.Position.X,
                            joint.Position.Y,
                            joint.Position.Z);
                    }

                    sender.Send(message);
                }
            }
        }
    }
}

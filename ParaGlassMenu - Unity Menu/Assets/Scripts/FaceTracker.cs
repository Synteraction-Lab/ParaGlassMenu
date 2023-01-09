using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;
using NRKernal;
using UnityEngine.UI;
using System.IO;
using System;

namespace FaceTracking
{
    public class FaceTracker : MonoBehaviour
    {
        // Start is called before the first frame update
        WebCamTexture webCamTexture;
        NRRGBCamTexture RGBCamTexture;
        private Texture2D CaptureImage;
        public RawImage presentedImage;
        public TextAsset faceXmlFile;
        public TextAsset eyeXmlFile;
        CascadeClassifier cascadeFace;
        CascadeClassifier cascadeEye;
        int count = 0;
        OpenCvSharp.Rect GuestFace;
        public float faceX;
        public float faceY;
        public GameObject menuOffset;
        public Text FrameCount;
        Texture previousFrame;
        public bool isTesting;
        public static bool debuggingFlag = false;
        public GameObject debugInfo;


        void Start()
        {
            isTesting = false;
            WebCamDevice[] devices = WebCamTexture.devices;
            if (devices.Length == 0)
            {
                RGBCamTexture = new NRRGBCamTexture();
                RGBCamTexture.Play();
                CaptureImage = RGBCamTexture.GetTexture();
            }
            else
            {
                webCamTexture = new WebCamTexture(devices[0].name);
                webCamTexture.Play();
                Debug.Log("Testing With Unity Editor");
                isTesting = true;
            }


            string facesCascadeData = faceXmlFile.text;

            if (null == facesCascadeData || facesCascadeData.Length == 0)
            {
                FrameCount.text = "ERROR";
                throw new Exception("FaceProcessor.Initialize: No face detector cascade passed, with parameter is required");
            }

            FileStorage storageFaces = new FileStorage(facesCascadeData, FileStorage.Mode.Read | FileStorage.Mode.Memory);
            cascadeFace = new CascadeClassifier();

            if (!cascadeFace.Read(storageFaces.GetFirstTopLevelNode()))
            {
                FrameCount.text = "ERROR";
                throw new Exception("FaceProcessor.Initialize: Failed to load faces cascade classifier");
            }


            string eyesCascadeData = eyeXmlFile.text;

            if (null == eyesCascadeData || eyesCascadeData.Length == 0)
            {
                FrameCount.text = "ERROR";
                throw new Exception("EyeProcessor.Initialize: No eye detector cascade passed, with parameter is required");
            }

            FileStorage storageEyes = new FileStorage(eyesCascadeData, FileStorage.Mode.Read | FileStorage.Mode.Memory);
            cascadeEye = new CascadeClassifier();

            if (!cascadeEye.Read(storageEyes.GetFirstTopLevelNode()))
            {
                FrameCount.text = "ERROR";
                throw new Exception("EyeProcessor.Initialize: Failed to load eyes cascade classifier");
            }

            if (debuggingFlag)
            {
                debugInfo.SetActive(true);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (count % 2 == 0)
            {
                Mat frame;
                if (isTesting)
                {
                    frame = OpenCvSharp.Unity.TextureToMat(webCamTexture);
                }
                else
                {
                    frame = OpenCvSharp.Unity.TextureToMat(CaptureImage);

                }

                FindNewFace(frame);

                if (debuggingFlag)
                {
                    debugInfo.SetActive(true);
                    Display(frame);
                }
                else
                {
                    debugInfo.SetActive(false);
                }

                count = 0;
            }
            count++;
        }

        void FindNewFace(Mat frame)
        {
            Mat gray = new Mat();
            Cv2.CvtColor(frame, gray, ColorConversionCodes.BGR2GRAY);

            // fix shadows
            Cv2.EqualizeHist(gray, gray);

            var faces = cascadeFace.DetectMultiScale(gray, 1.1, 5, HaarDetectionType.ScaleImage);

            //var faces = cascadeFace.DetectMultiScale(frame, 1.1, 5, HaarDetectionType.ScaleImage);
            if (faces.Length >= 1)
            {
                Mat candidateFace = new Mat(gray, faces[0]);
                var eyes = cascadeEye.DetectMultiScale(candidateFace);
                if (eyes.Length == 0 || eyes.Length > 2)
                {
                    FrameCount.text = "No Eyes";
                    return;
                }
                GuestFace = faces[0];
                //faceX = faces[0].X + faces[0].Width/2 - frame.Size().Width / 2;
                //faceY = faces[0].Y + faces[0].Height/2 - frame.Size().Height / 2;
                faceX = faces[0].X + faces[0].Width / 2;
                faceY = faces[0].Y + faces[0].Height / 2;

                float distance = 1.5f;

                TryGetWorldPosition(new Vector2(faceX, faceY), distance, out Vector3 worldPosition);
                FrameCount.text = GuestFace.Size.ToString() + "\n" + worldPosition.ToString() + "\nX:" + faceX + " Y:" + faceX + " frame:" + frame.Size().ToString();
                menuOffset.transform.position = worldPosition;
            }
            else
            {
                FrameCount.text = "No Face";
            }
        }

        void Display(Mat frame)
        {
            Texture2D.DestroyImmediate(previousFrame, true);
            if (GuestFace != null)
            {
                frame.Rectangle(GuestFace, new Scalar(255, 0, 0), 2);
            }

            Texture newTexture = OpenCvSharp.Unity.MatToTexture(frame);
            presentedImage.texture = newTexture;
            //Texture2D.DestroyImmediate(newTexture, true);
            previousFrame = newTexture;
        }


        public System.Numerics.Vector3 ToNumerics(Vector3 vector)
        {
            return new System.Numerics.Vector3(vector.x, vector.y, vector.z);
        }

        public System.Numerics.Quaternion ToNumerics(Quaternion quaternion)
        {
            return new System.Numerics.Quaternion(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
        }

        public Vector3 ToUnityEngine(System.Numerics.Vector3 vector)
        {
            return new Vector3(vector.X, vector.Y, vector.Z);
        }


        public static void SetDebuggingInfoActive()
        {
            debuggingFlag = true;
        }

        public static void SetDebuggingInfoInactive()
        {
            debuggingFlag = false;
        }

        public static void SwitchDebuggingInfoStatus()
        {
            debuggingFlag = !debuggingFlag;
        }


        public static System.Numerics.Matrix4x4 ConvertMatrix4x4(Matrix4x4 unityMatrix)
        {
            // Convert the UnityEngine Matrix4x4 to a System.Numerics Matrix4x4
            System.Numerics.Matrix4x4 snMatrix = new System.Numerics.Matrix4x4(
                unityMatrix.m00, unityMatrix.m01, unityMatrix.m02, unityMatrix.m03,
                unityMatrix.m10, unityMatrix.m11, unityMatrix.m12, unityMatrix.m13,
                unityMatrix.m20, unityMatrix.m21, unityMatrix.m22, unityMatrix.m23,
                unityMatrix.m30, unityMatrix.m31, unityMatrix.m32, unityMatrix.m33
            );

            return snMatrix;
        }

        // Reference: https://community.nreal.ai/t/aruco-pose-estimation-calibration-parameters/228/9
        bool TryGetWorldPosition(Vector2 pixelPosition, float distance, out Vector3 worldPosition)
        {
            var glassesPose = Pose.identity;
            ulong timestamp = 0;

            // Assume you are using the very latest frame so the image and 
            // glasses position are not too different! :)
            if (NRFrame.GetFramePresentHeadPose(ref glassesPose, ref timestamp))
            {
                // Get the projection transform.
                var projectionTransform = ConvertMatrix4x4(NRFrame.GetEyeProjectMatrix(out var _, 0.3f, 100f).RGBEyeMatrix);

                // Get the projection intrinsic values from the projection transform
                var focalLengthX = projectionTransform.M11;
                var focalLengthY = projectionTransform.M22;
                var centerX = projectionTransform.M13;
                var centerY = projectionTransform.M23;
                var normalFactor = projectionTransform.M33;

                // Normalize the center.
                centerX /= normalFactor;
                centerY /= normalFactor;

                // Get the pixel coords on a scale between -1 and 1.
                var pixelCoordinates = (new Vector2(pixelPosition.x / 1280f, 1 - (pixelPosition.y / 720f)) * 2f) - new Vector2(1, 1);

                // Create a directional ray using the principal point and the focal length.
                var dirRay = new Vector3(
                    (pixelCoordinates.x - centerX) / focalLengthX,
                    (pixelCoordinates.y - centerY) / focalLengthY,
                    1.0f);

                // Multiple the ray by the distance you want.
                var position = ToNumerics(dirRay * distance);

                // Get the RGB camera transform relative to the glasses.
                var cameraToGlassesTransform =
                    System.Numerics.Matrix4x4.CreateFromQuaternion(ToNumerics(NRFrame.EyePoseFromHead.RGBEyePose.rotation)) *
                    System.Numerics.Matrix4x4.CreateTranslation(ToNumerics(NRFrame.EyePoseFromHead.RGBEyePose.position));

                // Get the glasses transform relative to the world.
                var glassesToWorldTransform =
                    System.Numerics.Matrix4x4.CreateFromQuaternion(ToNumerics(glassesPose.rotation)) *
                    System.Numerics.Matrix4x4.CreateTranslation(ToNumerics(glassesPose.position));

                // Add these transform to create the full camera view transform
                var cameraViewTransform = cameraToGlassesTransform * glassesToWorldTransform;

                // Transform the position we have relative to the camera to make it relative to the world.
                worldPosition = ToUnityEngine(System.Numerics.Vector3.Transform(position, cameraViewTransform));
                return true;
            }

            worldPosition = Vector3.zero;
            return false;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;
using NRKernal;
using UnityEngine.UI;
using System.IO;
using System;

public class FaceTracker : MonoBehaviour
{
    // Start is called before the first frame update
    WebCamTexture webCamTexture;
    NRRGBCamTexture RGBCamTexture;
    public Texture2D CaptureImage;
    public RawImage presentedImage;
    public TextAsset xmlFile;
    CascadeClassifier cascade;
    int count = 0;
    OpenCvSharp.Rect GuestFace;
    public float faceX;
    public float faceY;
    public GameObject menuOffset;
    public Text FrameCount;
    Texture previousFrame;
    public bool isTesting;


    void Start()
    {
        isTesting = false;
        if (isTesting)
        {
            WebCamDevice[] devices = WebCamTexture.devices;
            webCamTexture = new WebCamTexture(devices[0].name);
            webCamTexture.Play();
            Debug.Log("Testing With Unity Editor");
        }
        else
        {
            RGBCamTexture = new NRRGBCamTexture();
            RGBCamTexture.Play();
            CaptureImage = RGBCamTexture.GetTexture();
        }

        string facesCascadeData = xmlFile.text;

        if (null == facesCascadeData || facesCascadeData.Length == 0)
        {
            throw new Exception("FaceProcessor.Initialize: No face detector cascade passed, with parameter is required");
        }

        FileStorage storageFaces = new FileStorage(facesCascadeData, FileStorage.Mode.Read | FileStorage.Mode.Memory);
        cascade = new CascadeClassifier();

        if (!cascade.Read(storageFaces.GetFirstTopLevelNode()))
        {
            throw new Exception("FaceProcessor.Initialize: Failed to load faces cascade classifier");
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (count % 5 == 0)
        {
            //FrameCount.text = RGBCamTexture.FrameCount.ToString();
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
            Display(frame);
            count = 0;
        }
        count++;
    }

    Vector3 Get3DPosition(float x, float y, float z)
    {


        var intrinsic = NRFrame.GetRGBCameraIntrinsicMatrix();
        int d_pix = 150;
        //float SensorWidth = 2;
        //float SensorHeight = 2;
        //var fx = intrinsic[0] * SensorWidth / Width;
        //var fy = intrinsic[4] * SensorHeight / Height;
        //var cx = ((Width / 2f) - intrinsic[2]) / Width * 2f;
        //var cy = (intrinsic[5] - (Height / 2f)) / Height * 2f;
        var fx = intrinsic[0];
        var fy = intrinsic[4];
        var cx = intrinsic[2];
        var cy = intrinsic[5];

        var Z = (fx * 0.2) / (d_pix);

        var X = ((x - cx) * Z) / fx;

        var Y = ((y - cy) * Z) / fy;

        return new Vector3((float)X, (float)Y, (float)Z);
    }

    void FindNewFace(Mat frame)
    {
        //Debug.Log("Frame: " + frame.Height);
        var faces = cascade.DetectMultiScale(frame, 1.1, 5, HaarDetectionType.ScaleImage, minSize:new Size(95, 95));
        //var faces = cascade.DetectMultiScale(frame, 1.1, 5, HaarDetectionType.ScaleImage);
        //Debug.Log("Face: " + faces.Length);
        if (faces.Length >= 1)
        {
            GuestFace = faces[0];
            //faceX = faces[0].X + faces[0].Width/2 - frame.Size().Width / 2;
            //faceY = faces[0].Y + faces[0].Height/2 - frame.Size().Height / 2;
            faceX = faces[0].X + faces[0].Width / 2;
            faceY = faces[0].Y + faces[0].Height / 2;
            //Debug.Log(GuestFace.Size +" "+new Vector2(faceX,faceY) + " " + menuOffset.transform.position);
            var new_position = Get3DPosition(faceX, faceY, menuOffset.transform.position.z);
            FrameCount.text = GuestFace.Size.ToString() + "\n" + new_position.ToString() + "\nX:"+ faceX + " Y:" + faceX + " frame:" + frame.Size().ToString();
            //menuOffset.transform.position = new Vector3(faceX/frame.Size().Width, -faceY/frame.Size().Height, menuOffset.transform.position.z);
            menuOffset.transform.position = new_position;
        }
    }

    void Display (Mat frame)
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
}

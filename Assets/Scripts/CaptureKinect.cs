using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureKinect : MonoBehaviour
{
    public RenderTexture kinectTexture;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void CaptureKinectImage()
    {
        // Create a new texture to hold the captured image
        Texture2D texture = new Texture2D(kinectTexture.width, kinectTexture.height, TextureFormat.RGBA32, false);
        RenderTexture.active = kinectTexture;
        texture.ReadPixels(new Rect(0, 0, kinectTexture.width, kinectTexture.height), 0, 0);
        texture.Apply();
        RenderTexture.active = null;

        // Encode the texture to PNG
        byte[] bytes = texture.EncodeToPNG();

        // Save the image to a file
        string filePath = System.IO.Path.Combine(Application.persistentDataPath, "kinect_image.png");
        System.IO.File.WriteAllBytes(filePath, bytes);

        Debug.Log("Kinect image saved to: " + filePath);
    }

    public void CapTureScreenShot()
    {
        // Create a new texture to hold the captured image
        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGBA32, false);
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture.Apply();

        // Encode the texture to PNG
        byte[] bytes = texture.EncodeToPNG();

        // Save the image to a file
        string filePath = System.IO.Path.Combine(Application.persistentDataPath, "screenshot.png");
        System.IO.File.WriteAllBytes(filePath, bytes);

        Debug.Log("Screenshot saved to: " + filePath);
    }
    // Update is called once per frame
    void Update()
    {

    }
}

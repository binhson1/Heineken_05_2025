using System.Collections;
using System.IO;
using UnityEngine;

public class CameraRecorder : MonoBehaviour
{
    public Camera targetCamera; // Camera bạn muốn record
    public int frameRate = 30;

    private bool isRecording = false;
    private int frameCount = 0;
    private string outputFolder;
    private RenderTexture renderTexture;
    private Texture2D screenShot;

    public void StartRecording()
    {
        if (targetCamera == null)
        {
            Debug.LogError("CameraRecorder: targetCamera is not set.");
            return;
        }

        outputFolder = Path.Combine(Application.dataPath, "CameraRecordings", System.DateTime.Now.ToString("yyyyMMdd_HHmmss"));
        Directory.CreateDirectory(outputFolder);

        renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

        isRecording = true;
        frameCount = 0;

        StartCoroutine(RecordFrames());
    }

    public void StopRecording()
    {
        isRecording = false;
        StopAllCoroutines();

        if (renderTexture != null)
        {
            renderTexture.Release();
            Destroy(renderTexture);
        }

        if (screenShot != null)
        {
            Destroy(screenShot);
        }

        Debug.Log("Recording stopped. Frames saved to: " + outputFolder);
    }

    private IEnumerator RecordFrames()
    {
        while (isRecording)
        {
            yield return new WaitForEndOfFrame();

            targetCamera.targetTexture = renderTexture;
            targetCamera.Render();

            RenderTexture.active = renderTexture;
            screenShot.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            screenShot.Apply();

            byte[] bytes = screenShot.EncodeToPNG();
            string filename = Path.Combine(outputFolder, $"frame_{frameCount:D04}.png");
            File.WriteAllBytes(filename, bytes);

            frameCount++;

            RenderTexture.active = null;
            targetCamera.targetTexture = null;

            yield return new WaitForSeconds(1f / frameRate);
        }
    }
}

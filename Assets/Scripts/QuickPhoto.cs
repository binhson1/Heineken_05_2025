using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickPhoto : MonoBehaviour
{
    public TextMeshProUGUI countDownText;
    public RawImage viewImage;
    public GameObject photoPanel;
    public GameObject capturePanel;

    private string filePath;
    private string photoName;
    private bool isTakingPhoto = false;

    void Start()
    {
        string baseDirectory = Path.GetDirectoryName(Application.dataPath);
        string fileName = "Data";
        filePath = Path.Combine(baseDirectory, fileName);
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.PageDown))
        {
            if (capturePanel.activeSelf && !isTakingPhoto)
            {
                StartTakingPhoto();
            }
        }
    }

    public void StartTakingPhoto()
    {
        if (capturePanel.activeSelf && !isTakingPhoto)
        {
            StartCoroutine(CountdownAndTakePhoto(3));
        }
    }

    public IEnumerator CountdownAndTakePhoto(int countdownTime)
    {
        isTakingPhoto = true;

        for (int i = countdownTime; i >= 0; i--)
        {
            countDownText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }

        countDownText.text = "";
        TakePhoto();
        yield return new WaitForSeconds(0.5f);
        ShowPhoto(photoName);

        isTakingPhoto = false;
    }

    public void ShowPhoto(string photoPath)
    {
        if (File.Exists(photoPath))
        {
            byte[] imageBytes = File.ReadAllBytes(photoPath);
            Texture2D texture = new Texture2D(3840, 2160);
            texture.LoadImage(imageBytes);
            viewImage.texture = texture;
            photoPanel.SetActive(true);
            capturePanel.SetActive(false);
        }
        else
        {
            Debug.LogError("Photo not found at " + photoPath);
        }
    }

    public void TakePhoto()
    {
        string timeStamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
        photoName = Path.Combine(filePath, $"photo_{timeStamp}.jpg");

        ScreenCapture.CaptureScreenshot(photoName, 1);
        Debug.Log("Photo taken and saved to " + photoName);
    }
}

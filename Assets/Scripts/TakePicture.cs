using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TakePicture : MonoBehaviour
{
    private WebCamTexture webCamTexture;
    public Renderer displayTexture;
    public GameObject QR;
    public RawImage cameraTexture;
    public Camera TakePhotoCamera;
    public Animator messiAnimator;
    public Renderer photoRender;
    public Animator ronaldoAnimator;
    public TextMeshProUGUI countdownText;
    public GameObject Photo;
    private string filePath;
    public GameObject Ronaldo;
    public GameObject Messi;
    public RawImage photoImage;
    // Start is called before the first frame update
    void Start()
    {
        // init camera
        WebCamDevice[] devices = WebCamTexture.devices;
        webCamTexture = new WebCamTexture(devices[0].name);
        webCamTexture.Play();
        displayTexture.material.mainTexture = webCamTexture;
        // cameraTexture.texture = webCamTexture;
        // cameraTexture.SetNativeSize();
        string baseDirectory = Path.GetDirectoryName(Application.dataPath);
        string fileName = "Data";
        filePath = Path.Combine(baseDirectory, fileName);
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
        Photo.SetActive(false);
        // transparent photoRender

    }


    // Update is called once per frame
    void Update()
    {

    }

    public void ChooseMessi()
    {
        messiAnimator.Play("Messi");
        // start countdown after 20s
        StartCoroutine(CountdownAndTakePhoto(20));
    }
    public IEnumerator CountdownAndTakePhoto(int countdownTime)
    {
        for (int i = countdownTime; i > 0; i--)
        {
            if (i <= 3)
            {
                countdownText.text = i.ToString();
            }
            yield return new WaitForSeconds(1);
        }
        countdownText.text = "";
        yield return new WaitForSeconds(1);
        QR.SetActive(true);
        TakePhotoOfUnityCamera();
        QR.SetActive(true);
        Ronaldo.SetActive(false);
        Messi.SetActive(false);
    }

    public void ChooseRonaldo()
    {
        ronaldoAnimator.Play("Ronaldo");
        StartCoroutine(CountdownAndTakePhoto(6));
    }
    public void TakePhoto()
    {
        // Create a texture to hold the photo
        Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
        // Read pixels from the camera
        photo.SetPixels(webCamTexture.GetPixels());
        photo.Apply();

        // Encode texture into PNG
        byte[] bytes = photo.EncodeToJPG();
        string currentTime = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
        System.IO.File.WriteAllBytes(filePath + "/photo_" + currentTime + ".jpg", bytes);

        Debug.Log("Photo taken and saved to " + filePath + "/photo_" + currentTime + ".jpg");
    }

    public void TakePhotoOfUnityCamera()
    {
        // Create a texture to hold the photo
        Texture2D photo = new Texture2D(1920, 1080, TextureFormat.RGB24, false);
        // Read pixels from the camera
        RenderTexture renderTexture = new RenderTexture(1920, 1080, 24);
        TakePhotoCamera.targetTexture = renderTexture;
        TakePhotoCamera.Render();
        RenderTexture.active = renderTexture;
        photo.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        photo.Apply();

        // Encode texture into PNG
        byte[] bytes = photo.EncodeToJPG();
        string currentTime = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
        System.IO.File.WriteAllBytes(filePath + "/photo_" + currentTime + ".jpg", bytes);

        Debug.Log("Photo taken and saved to " + filePath + "/photo_" + currentTime + ".jpg");

        // show the photo in the UI
        Texture2D photoTexture = new Texture2D(photo.width, photo.height, TextureFormat.RGB24, false);
        photoTexture.SetPixels(photo.GetPixels());
        photoTexture.Apply();
        Photo.SetActive(true);
        // photoRender.material.mainTexture = photoTexture;
        photoImage.texture = photoTexture;
        photoImage.SetNativeSize();
        // Clean up
        TakePhotoCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvasController : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI countDownText;
    public GameObject cr7Animator;
    public GameObject messiAnimator;
    public RawImage viewImage;
    public GameObject photoPanel;
    private string filePath;
    private string photoName;
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

    public void ChooseMessi()
    {
        messiAnimator.SetActive(true);
        messiAnimator.GetComponent<Animator>().Play("Messi");
        // messiAnimator.Play("Messi");
        StartCoroutine(CountdownAndTakePhoto(10));
    }
    public void ChooseRonaldo()
    {
        cr7Animator.SetActive(true);
        cr7Animator.GetComponent<Animator>().Play("Ronaldo");
        StartCoroutine(CountdownAndTakePhoto(10));
    }
    public IEnumerator CountdownAndTakePhoto(int countdownTime)
    {
        for (int i = countdownTime; i >= 0; i--)
        {
            countDownText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        countDownText.text = "";
        TakePhoto();
        yield return new WaitForSeconds(0.5f);
        messiAnimator.SetActive(false);
        cr7Animator.SetActive(false);
        ShowPhoto(photoName);
    }
    public void ShowPhoto(string photoPath)
    {
        if (File.Exists(photoPath))
        {
            byte[] imageBytes = File.ReadAllBytes(photoPath);
            Texture2D texture = new Texture2D(720, 1280);
            texture.LoadImage(imageBytes);
            viewImage.texture = texture;
            photoPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("Photo not found at " + photoPath);
        }
    }

    public void TakePhotoAgain()
    {
        messiAnimator.SetActive(true);
        cr7Animator.SetActive(true);
        StartCoroutine(CountdownAndTakePhoto(10));
    }
    public void TakePhoto()
    {
        string timeStamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
        photoName = filePath + "\\photo_" + timeStamp + ".jpg";

        ScreenCapture.CaptureScreenshot(photoName, 1);
        Debug.Log("Photo taken and saved to " + photoName);
    }
    // Update is called once per frame
    void Update()
    {

    }
}

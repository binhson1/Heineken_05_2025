using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class MainCanvasController : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI countDownText;
    public Animator cr7Animator;
    public Animator messiAnimator;
    private string filePath;

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
        messiAnimator.Play("Messi");
        StartCoroutine(CountdownAndTakePhoto(10));
    }
    public void ChooseRonaldo()
    {
        cr7Animator.Play("Ronaldo");
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
    }

    public void TakePhoto()
    {
        string timeStamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string photoName = filePath + "/photo_" + timeStamp + ".jpg";

        ScreenCapture.CaptureScreenshot(photoName, 1);
        Debug.Log("Photo taken and saved to " + photoName);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

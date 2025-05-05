using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;

public class ImageUploader : MonoBehaviour
{
    [SerializeField] private string uploadURL = "http://192.168.1.100:9789/api/image/upload";
    [SerializeField] private Texture2D imageToUpload;
    private string fileName = "default_filename.png";
    private string description = "";

    public void UploadImage()
    {
        StartCoroutine(UploadImageCoroutine());
    }

    IEnumerator UploadImageCoroutine()
    {
        byte[] imageData = imageToUpload.EncodeToPNG();

        WWWForm form = new WWWForm();

        form.AddBinaryData("image", imageData, fileName, "image/png");
        form.AddField("description", description);

        using (UnityWebRequest request = UnityWebRequest.Post(uploadURL, form))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Upload thành công: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Lỗi upload: " + request.error);
            }
        }
    }
}
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;

public class ImageUploader : MonoBehaviour
{
    [SerializeField] public string uploadURL = "http://192.168.1.100:9456/api/image/upload";
    [SerializeField] public string imageFolderPath = "Data";
    [SerializeField] public string imageFileName = "image.png";
    private string description = "";

    public void UploadImage()
    {
        StartCoroutine(UploadImageCoroutine());
    }

    IEnumerator UploadImageCoroutine()
    {
        string fullPath = Path.Combine(Application.dataPath, "..", imageFolderPath, imageFileName);
        fullPath = Path.GetFullPath(fullPath);

        if (!File.Exists(fullPath))
        {
            Debug.LogError("Không tìm thấy file ảnh tại đường dẫn: " + fullPath);
            yield break;
        }

        byte[] imageData = File.ReadAllBytes(fullPath);

        WWWForm form = new WWWForm();
        form.AddBinaryData("image", imageData, imageFileName, GetMimeTypeFromFileName(imageFileName));

        description = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");

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

    private string GetMimeTypeFromFileName(string fileName)
    {
        string extension = Path.GetExtension(fileName).ToLowerInvariant();

        switch (extension)
        {
            case ".jpg":
            case ".jpeg":
                return "image/jpeg";
            case ".png":
                return "image/png";
            case ".gif":
                return "image/gif";
            case ".bmp":
                return "image/bmp";
            case ".webp":
                return "image/webp";
            default:
                return "application/octet-stream";
        }
    }


    public void SetImagePath(string folderPath, string fileName)
    {
        imageFolderPath = folderPath;
        imageFileName = fileName;
    }

    public void SetDescription(string desc)
    {
        if (string.IsNullOrEmpty(desc))
        {
            description = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
            return;
        }
        description = desc + "_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
    }
}
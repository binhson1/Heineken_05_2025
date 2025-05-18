using UnityEngine;
using ZXing;
using UnityEngine.UI;
using System;
using System.IO;
using ZXing.QrCode;
using UnityEngine.Networking;
using System.Collections;

public class ImageUploader : MonoBehaviour
{
    [SerializeField] public string uploadURL = "http://localhost:9456/api/image/upload";
    [SerializeField] public string imageFolderPath = "Data";
    [SerializeField] public string imageFileName = "image.png";
    private string description = "";

    public RawImage QRcode;

    void Start()
    {

    }
    public void UploadImage()
    {
        StartCoroutine(UploadImageCoroutine());
    }

    IEnumerator UploadImageCoroutine()
    {
        string fullPath = PlayerPrefs.GetString("ImagePath");
        fullPath = Path.GetFullPath(fullPath);

        if (!File.Exists(fullPath))
        {
            Debug.LogError("Không tìm thấy file ảnh tại đường dẫn: " + fullPath);
            yield break;
        }

        byte[] imageData = File.ReadAllBytes(fullPath);

        WWWForm form = new WWWForm();
        form.AddBinaryData("image", imageData, imageFileName, GetMimeTypeFromFileName(imageFileName));

        description = DateTime.Now.ToString("yyyyMMdd_HHmmss");

        form.AddField("description", description);

        using (UnityWebRequest request = UnityWebRequest.Post(uploadURL, form))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Upload thành công: " + request.downloadHandler.text);

                // Cắt chuỗi URL từ response text
                string responseText = request.downloadHandler.text;
                string url = ExtractUrlFromResponse(responseText);

                if (!string.IsNullOrEmpty(url))
                {
                    // Tạo QR code từ URL
                    GenerateQRCode(url);
                }
                else
                {
                    Debug.LogError("Không tìm thấy URL trong phản hồi");
                }
            }
            else
            {
                Debug.LogError("Lỗi upload: " + request.error);
            }
        }
    }

    // Cắt chuỗi URL từ phản hồi JSON
    private string ExtractUrlFromResponse(string responseText)
    {
        string startMarker = "\"url\":\"";
        string endMarker = "\"";

        int startIndex = responseText.IndexOf(startMarker);
        if (startIndex == -1)
        {
            return null; // Không tìm thấy URL
        }

        startIndex += startMarker.Length;
        int endIndex = responseText.IndexOf(endMarker, startIndex);

        if (endIndex == -1)
        {
            return null; // Không tìm thấy URL
        }

        string url = responseText.Substring(startIndex, endIndex - startIndex);
        return url;
    }

    // Tạo QR Code từ URL và hiển thị trên RawImage
    private void GenerateQRCode(string url)
    {
        // Sử dụng ZXing để tạo QR code
        var barcodeWriter = new BarcodeWriterPixelData
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = 256,
                Width = 256
            }
        };

        // Tạo QR code dưới dạng Texture2D        
        var pixelData = barcodeWriter.Write(url);

        Texture2D texture = new Texture2D(pixelData.Width, pixelData.Height, TextureFormat.RGBA32, false);
        texture.LoadRawTextureData(pixelData.Pixels);
        texture.Apply();
        // Hiển thị QR code trên RawImage
        QRcode.texture = texture;
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
}

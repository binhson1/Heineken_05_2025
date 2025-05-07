using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;
using System.Security.Cryptography.X509Certificates;

public class ImageUploader : MonoBehaviour
{
    [SerializeField] public string uploadURL = "http://192.168.1.100:9456/api/image/upload";
    [SerializeField] public string imageFolderPath = "data"; // Thư mục chứa ảnh, tương đối với thư mục ứng dụng
    [SerializeField] public string imageFileName = "image.png"; // Tên file ảnh
    private string description = "";

    public void UploadImage()
    {
        StartCoroutine(UploadImageCoroutine());
    }

    IEnumerator UploadImageCoroutine()
    {
        // Xây dựng đường dẫn đầy đủ đến file ảnh
        string fullPath = Path.Combine(Application.dataPath, "..", imageFolderPath, imageFileName);
        fullPath = Path.GetFullPath(fullPath); // Chuẩn hóa đường dẫn

        // Kiểm tra xem file có tồn tại không
        if (!File.Exists(fullPath))
        {
            Debug.LogError("Không tìm thấy file ảnh tại đường dẫn: " + fullPath);
            yield break;
        }

        // Đọc dữ liệu ảnh trực tiếp từ file
        byte[] imageData = File.ReadAllBytes(fullPath);

        // Tạo form và thêm dữ liệu
        WWWForm form = new WWWForm();
        form.AddBinaryData("image", imageData, imageFileName, GetMimeTypeFromFileName(imageFileName));
        form.AddField("description", description);

        // Gửi request
        using (UnityWebRequest request = UnityWebRequest.Post(uploadURL, form))
        {
            // Cho phép kết nối không bảo mật (HTTP)
            request.certificateHandler = new BypassCertificate();
            request.disposeCertificateHandlerOnDispose = true;

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

    // Hàm xác định MIME type dựa trên phần mở rộng của file
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
                return "application/octet-stream"; // Default MIME type
        }
    }

    // Phương thức để thiết lập đường dẫn từ code
    public void SetImagePath(string folderPath, string fileName)
    {
        imageFolderPath = folderPath;
        imageFileName = fileName;
    }

    // Phương thức để thiết lập mô tả từ code
    public void SetDescription(string desc)
    {
        description = desc;
    }

    // Lớp để bỏ qua kiểm tra chứng chỉ SSL/TLS cho kết nối không bảo mật
    public class BypassCertificate : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            // Trả về true để luôn chấp nhận chứng chỉ
            return true;
        }
    }
}
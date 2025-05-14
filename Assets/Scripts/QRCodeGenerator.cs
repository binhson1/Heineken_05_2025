using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;
using System;
using TMPro;

public class QRCodeGenerator : MonoBehaviour
{
    public TMP_InputField inputField;
    public RawImage qrImage;
    public Button generateButton;

    void Start()
    {
        generateButton.onClick.AddListener(GenerateQR);
    }

    public void GenerateQR()
    {
        string text = inputField.text;

        if (string.IsNullOrEmpty(text))
        {
            Debug.LogWarning("Input is empty.");
            return;
        }

        Texture2D qrCodeTexture = GenerateQRCode(text);
        qrImage.texture = qrCodeTexture;
        qrImage.rectTransform.sizeDelta = new Vector2(qrCodeTexture.width, qrCodeTexture.height);
    }

    private Texture2D GenerateQRCode(string text)
    {
        var writer = new BarcodeWriterPixelData
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = 256,
                Width = 256,
                Margin = 1
            }
        };

        var pixelData = writer.Write(text);

        Texture2D texture = new Texture2D(pixelData.Width, pixelData.Height, TextureFormat.RGBA32, false);
        texture.LoadRawTextureData(pixelData.Pixels);
        texture.Apply();

        return texture;
    }
}

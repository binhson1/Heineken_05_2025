using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateFilenameBtn : MonoBehaviour
{
    string GenerateFileName(string prefix = "heineken")
    {
        string timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string uniqueFileName = prefix + "_" + timestamp;
        return uniqueFileName;
    }
    public void RandomFilename()
    {
        string fileName = GenerateFileName();
        PlayerPrefs.SetString("fileName", fileName);
        PlayerPrefs.Save();
        Debug.Log("Generated File Name: " + fileName);
    }
}

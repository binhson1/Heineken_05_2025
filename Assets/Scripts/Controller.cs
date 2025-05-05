using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public List<GameObject> Scene;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Scene.Count; i++)
        {
            Scene[i].SetActive(false);
        }
        Scene[0].SetActive(true);
        // Screen.SetResolution(1080, 1920, true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NextScene()
    {
        for (int i = 0; i < Scene.Count; i++)
        {
            if (Scene[i].activeSelf == true)
            {
                Scene[i].SetActive(false);
                if (i + 1 < Scene.Count)
                {
                    Scene[i + 1].SetActive(true);
                }
                else
                {
                    Scene[0].SetActive(true);
                }
                break;
            }
        }
    }
}

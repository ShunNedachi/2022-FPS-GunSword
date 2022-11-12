using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject thirdViewCamera;
    public static CameraController instance;

    public void Awake()
    {
        if(instance ==null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.Find("MainCamera");
        thirdViewCamera = GameObject.Find("thirdViewCamera");

        thirdViewCamera.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }
    //一人称視点に変更
    public void ChangeMainCamera()
    {
        mainCamera.SetActive(true);
        thirdViewCamera.SetActive(false);
    }
    //三人称視点に変更
    public void ChangeThirdViewCamera()
    {
        mainCamera.SetActive(false);
        thirdViewCamera.SetActive(true);
    }
}

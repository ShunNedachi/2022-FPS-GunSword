using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BayonetMove : MonoBehaviour
{
    public static BayonetMove instance;

    [SerializeField]public new GameObject camera;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = camera.transform.localRotation;

    }
    void FixedUpdate()
    {
    }
    public void SetActivity(bool activity)
    {
        gameObject.SetActive(activity);
    }
}

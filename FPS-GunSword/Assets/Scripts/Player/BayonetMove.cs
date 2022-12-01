using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BayonetMove : MonoBehaviour
{
    [SerializeField]public new GameObject camera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localRotation = camera.transform.localRotation;
        this.transform.localRotation *= Quaternion.Euler(0,180,0);
    }
    void FixedUpdate()
    {

    }
}

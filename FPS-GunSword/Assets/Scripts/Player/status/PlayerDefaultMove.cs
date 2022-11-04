using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefaultMove : MonoBehaviour
{
    public static PlayerDefaultMove instance;

    [SerializeField] public float moveSpeed = 0.5f;

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
    public void Update()
    {
        Transform trans = transform;
        transform.position = trans.position;
        trans.position += trans.TransformDirection(Vector3.forward) * Input.GetAxis("Vertical") * moveSpeed;
        trans.position += trans.TransformDirection(Vector3.right) * Input.GetAxis("Horizontal") * moveSpeed;
    }
}

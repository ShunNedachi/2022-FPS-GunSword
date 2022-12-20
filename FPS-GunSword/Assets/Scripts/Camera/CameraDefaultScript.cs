using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDefaultScript : MonoBehaviour
{
    public static CameraDefaultScript instance;

    [SerializeField]public float sensitivity = 0.1f;
    [SerializeField]public new GameObject camera;

    private Quaternion cameraRot, characterRot;

    private bool cursorLock = false;

    // �p�x�̐����p
    private float minX = -30.0f, maxX = 15.0f;
    float x,z;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    // Start is called before the first frame update
    public void Start()
    {
        cameraRot = camera.transform.localRotation;
        characterRot = transform.localRotation;
    }

    // Update is called once per frame
    public void Update()
    {
        if(PlayerDefaultMove.instance.GetIsMove())
        {
            float xRot = Input.GetAxis("Mouse X") * sensitivity;
            float yRot = Input.GetAxis("Mouse Y") * sensitivity;

            cameraRot *= Quaternion.Euler(-yRot, 0, 0);
            characterRot *= Quaternion.Euler(0,xRot, 0);

            cameraRot = ClampRotation(cameraRot);

            camera.transform.localRotation = cameraRot;
            transform.localRotation = characterRot;

            UpdateCursorLock();
        }
    }

    private void FixedUpdate()
    {
        if(PlayerDefaultMove.instance.GetIsMove())
        {
            x = 0;
            z = 0;

            x = Input.GetAxisRaw("Horizontal") * sensitivity;
            z = Input.GetAxisRaw("Vertical") * sensitivity;

            transform.position += camera.transform.forward * z + camera.transform.right * x;
            transform.position = new Vector3(transform.position.x,3.08f,transform.position.z);
        }
    }

    // �J�[�\���\���p(�N���b�N�Ŕ�\���@ESC�ŕ\��)
    public void UpdateCursorLock()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            cursorLock = false;
        }
        else if (Input.GetMouseButton(0))
        {
            cursorLock = true;
        }


        if (cursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (!cursorLock)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    //�p�x�����֐��̍쐬
    public Quaternion ClampRotation(Quaternion q)
    {
        //q = x,y,z,w (x,y,z�̓x�N�g���i�ʂƌ����j�Fw�̓X�J���[�i���W�Ƃ͖��֌W�̗ʁj)

        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = Mathf.Atan(q.x) * Mathf.Rad2Deg * 2f;
        //float angleY = Mathf.Atan(q.y) * Mathf.Rad2Deg * 2f;

        angleX = Mathf.Clamp(angleX, minX, maxX);
        //angleY = Mathf.Clamp(angleY, minY, minY);

        q.x = Mathf.Tan(angleX * Mathf.Deg2Rad * 0.5f);
        //q.y = Mathf.Tan(angleY * Mathf.Deg2Rad * 0.5f);

        return q;
    }
}

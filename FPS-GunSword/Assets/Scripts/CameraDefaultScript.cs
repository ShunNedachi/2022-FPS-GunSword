using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDefaultScript : MonoBehaviour
{
    [SerializeField]public float sensitivity = 0.1f;
    [SerializeField]public new GameObject camera;

    private Quaternion cameraRot, characterRot;

    private bool cursorLock = true;

    // 角度の制限用
    private float minX = -90.0f, maxX = 90.0f;

    // Start is called before the first frame update
    void Start()
    {
        cameraRot = camera.transform.localRotation;
        characterRot = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
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


    // カーソル表示用(クリックで非表示　ESCで表示)
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

    //角度制限関数の作成
    public Quaternion ClampRotation(Quaternion q)
    {
        //q = x,y,z,w (x,y,zはベクトル（量と向き）：wはスカラー（座標とは無関係の量）)

        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1f;

        float angleX = Mathf.Atan(q.x) * Mathf.Rad2Deg * 2f;

        angleX = Mathf.Clamp(angleX, minX, maxX);

        q.x = Mathf.Tan(angleX * Mathf.Deg2Rad * 0.5f);

        return q;
    }
}

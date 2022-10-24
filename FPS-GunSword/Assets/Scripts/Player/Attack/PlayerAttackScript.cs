using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackScript : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private float forwardPlusValue = 3.0f;
    [SerializeField] private float shootInterval = 1.0f;
    [SerializeField] private new GameObject camera;

    private float timer = 0;
         
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //test
        timer += Time.deltaTime;

        // 左クリックを押したとき かつ一定間隔をあけたら
        if(Input.GetMouseButton(0)&& timer >= shootInterval)
        {
            // プレイヤーの少し前に生成する
            Vector3 createPos = transform.position + camera.transform.forward * forwardPlusValue;


            // 弾の生成
            Instantiate(bullet, createPos, camera.transform.rotation);

            // タイマーの初期化
            timer = 0;
        }
        // 右クリックを押したとき
        else if(Input.GetMouseButton(1))
        {

        }
    }
}

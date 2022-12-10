using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemyBulletScript : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float shootRange = 100.0f;
    [SerializeField] private int damage = 30;

    private float totalMoveDistance = 0.0f;

    //　距離計算用
    private Vector3 initPosition;

    public Vector3 targetV
    {
        get { return targetV; }
        set { targetV = value; }
    }

    public bool IsDead
    {
        get { return IsDead; }
        set { IsDead = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        initPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // 正面ベクトル方向に加速していく
        var tempMoveVector = targetV.normalized * speed;

        transform.position += tempMoveVector;

        // 弾の消滅処理用に合計移動距離を計算
        totalMoveDistance = Vector3.Distance(initPosition, transform.position);


        if(totalMoveDistance > shootRange)
        {
            Destroy(transform.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // とりあえず障害物に当たったら消す
        Destroy(transform.gameObject);

        // プレイヤーに当たっていた時の処理
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerHPScript>().Sethp(damage);
        }
    }
}
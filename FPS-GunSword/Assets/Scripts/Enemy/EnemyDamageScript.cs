using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 敵がダメージを受けた時用
public class EnemyDamageScript : MonoBehaviour
{
    // ここを変更してダメージ量を調整
    [SerializeField] private bool isHead = false;
    [SerializeField] private float damageMaltiply = 2.0f; 

    // ステータス情報参照用
    DefaultEnemy enemy;

    // Start is called before the first frame update
    void Start()
    {
        // 変数に敵の情報を持ってくる
        if (transform.parent.tag == "RangeEnemy")
        {
            enemy = transform.parent.GetComponent<RangeEnemy>();
        }
        else if (transform.parent.tag == "MeleeEnemy")
        {
            enemy = transform.parent.GetComponent<MeleeEnemy>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // プレイヤーの攻撃が当たったとき用
    public void HitPlayerAttack(float baseDamage)
    {
        // 弱点に当たったとき
        if (isHead)
        {
            enemy.GetDamage(baseDamage * damageMaltiply);
            enemy.IsStun = true;
        }
        else enemy.GetDamage(baseDamage);

    }
}

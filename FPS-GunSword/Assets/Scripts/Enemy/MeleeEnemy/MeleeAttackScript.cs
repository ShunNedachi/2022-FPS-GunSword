using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackScript : MonoBehaviour
{
    MeleeEnemyScript enemy;

    // Start is called before the first frame update
    void Start()
    {
        // 親クラスのステータスを参照する用
        GameObject parent = transform.parent.parent.gameObject;
        enemy = parent.GetComponent<MeleeEnemyScript>();


    }

    // Update is called once per frame
    void Update()
    {
    }

    // 攻撃が当たったときの処理
    private void OnTriggerStay(Collider other)
    {
        // プレイヤーが当たっていたら
        if (other.gameObject.tag == "Player" && enemy.AttackActive)
        {
            Debug.Log("Hit Attack to Player");
            other.GetComponent<PlayerHPScript>().Sethp(enemy.TakeDamage());
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    [SerializeField] private float damage = 25.0f;
    [SerializeField] private float destroyTimer = 0;
    [SerializeField] private float DestroyInterval = 10.0f;

    private bool hit = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = PlayerSlashScript.instance.GetAttackPos();

        if(destroyTimer>DestroyInterval)
        {
            if(hit == false)
            {
                PlayerSlashScript.instance.ComboReset();
            }
            Destroy(this.gameObject);
        }
        destroyTimer++;
    }

    void OnTriggerEnter(Collider t)
    {
        if(t.gameObject.tag == "MeleeEnemy"
            ||t.gameObject.tag == "RangeEnemy")
        {
            hit = true;
            Debug.Log(t.gameObject.name);
            //damage処理
            t.gameObject.GetComponent<EnemyDamageScript>().HitPlayerAttack(damage);
            //エネルギー加算
            PlayerEnergyScript.instance.SlashChargeEnergy();
            //コンボ加算
            PlayerSlashScript.instance.AddCombo();
        }
        if(t.gameObject.tag == "Core")
        {
            //コアに対するダメージ処理
            t.gameObject.GetComponent<CoreScript>().MeleeHit();
            //エネルギー加算
            PlayerEnergyScript.instance.SlashChargeEnergy();
            //コンボ加算
            PlayerSlashScript.instance.AddCombo();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Excalibur : MonoBehaviour
{
    [SerializeField] private float damage = 25.0f;
    [SerializeField] private float destroyTimer = 0;
    [SerializeField] private float DestroyInterval = 100.0f;
    [SerializeField] private float speed = 0.01f;

    private bool hit = false;
    private Rigidbody rigidBody;


    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        // �e�̈ړ��p
        rigidBody.velocity = transform.forward * speed;

    }

    // Update is called once per frame
    void Update()
    {

        if(destroyTimer>DestroyInterval)
        {
            if(hit == false)
            {
                PlayerSlashScript.instance.ComboReset();
            }
            PlayerDefaultMove.instance.SetMove(true);
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
            t.gameObject.GetComponent<EnemyDamageScript>().HitPlayerAttack(damage,t.gameObject.transform.position);
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

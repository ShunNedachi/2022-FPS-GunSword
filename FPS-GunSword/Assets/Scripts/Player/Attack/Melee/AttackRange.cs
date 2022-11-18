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

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "MeleeEnemy"
            ||collision.gameObject.tag == "RangeEnemy")
            {
                hit = true;
                Debug.Log("Hit");
                //damage処理
                collision.gameObject.GetComponent<DefaultEnemy>().GetDamage(damage);
                //エネルギー加算
                PlayerEnergyScript.instance.SlashChargeEnergy();
                //コンボ加算
                PlayerSlashScript.instance.AddCombo();

            }
    }
}

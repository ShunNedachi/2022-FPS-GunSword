using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerULTScript : MonoBehaviour
{
    public static PlayerULTScript instance;
    
    [SerializeField] private int attackInterval = 50;
    [SerializeField] private int comboResetInterval = 300;
    [SerializeField] private GameObject ULTAttackRange;

    private float attackTimer = 0;
    private float comboResetTimer = 0;

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
        attackTimer = attackInterval;
    }

    // Update is called once per frame
    public void Update()
    {
        attackTimer++;

        if(Input.GetMouseButton(1) && attackTimer > attackInterval )
        {
            PlayerEnergyScript.instance.EnemyConsumptionSlash();
            attackTimer = 0;
            // プレイヤーの少し前に生成する
            Instantiate(ULTAttackRange, transform.position, transform.rotation);
        }
        if(PlayerSlashScript.instance.GetComboCount()>0)
        {
            comboResetTimer++;
            if(comboResetTimer>comboResetInterval)
            {
               PlayerSlashScript.instance.ComboReset();
            }
        }

        PlayerEnergyScript.instance.EnemyConsumption();

        if(PlayerEnergyScript.instance.GetEnergy()<0)
        {
            //ULT終了処理
            PlayerEnergyScript.instance.SetULTchack(false);
            PlayerScript.instance.SetULTchack(false);

        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerULTScript : MonoBehaviour
{
    public static PlayerULTScript instance;
    
    [SerializeField] private int attackInterval = 50;
    [SerializeField] private float damage = 25.0f;
    [SerializeField] private GameObject ULTAttackRange;

    private float attackTimer = 0;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        attackTimer = attackInterval;
    }

    // Update is called once per frame
    public void Update()
    {
        attackTimer++;

        if(Input.GetMouseButton(1) && attackTimer > attackInterval )
        {
            attackTimer = 0;
            // プレイヤーの少し前に生成する
            Instantiate(ULTAttackRange, transform.position, transform.rotation);
        }
        if(comboCount>0)
        {
            comboResetTimer++;
            if(comboResetTimer>comboResetInterval)
            {
                ComboReset();
            }
        }
    }


}

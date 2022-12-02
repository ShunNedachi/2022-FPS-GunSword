using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerULTScript : MonoBehaviour
{
    public static PlayerULTScript instance;
    
    [SerializeField] private int attackInterval = 50;
    [SerializeField] private float attackDictance = 5.0f;
    [SerializeField] private new GameObject camera;
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
    public void Start()
    {
        attackTimer = attackInterval;
        PlayerEnergyScript.instance.Start();
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
            Vector3 createPos = transform.position + camera.transform.forward * attackDictance;
            Instantiate(ULTAttackRange, createPos, camera.transform.rotation);
        }

        PlayerEnergyScript.instance.EnemyConsumption();

        if(PlayerEnergyScript.instance.GetEnergy()<0)
        {
            //ULT終了処理
            PlayerEnergyScript.instance.SetULTchack(false);
            PlayerScript.instance.SetULTchack(false);

        }
    }
    public Vector3 GetAttackPos()
    {
        Vector3 createPos = transform.position + camera.transform.forward * attackDictance;

        return createPos;
    }


}

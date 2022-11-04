using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PlayerSlashScript : MonoBehaviour
{
    public static PlayerSlashScript instance;
    
    [SerializeField] private GameObject enemy;
    [SerializeField] private int attackInterval = 50;

    private float attackTimer = 0;
    private int comboCount = 0;

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
        
    }

    // Update is called once per frame
    public void Update()
    {
        if(Input.GetMouseButton(1) && attackTimer >attackInterval)
        {
            Vector3 enemyPos = enemy.transform.position;
            enemyPos.y = transform.position.y;
            attackTimer = 0;

            if(Vector3.Distance(enemyPos,transform.position) <= 2.0f && Vector3.Dot(Vector3.Normalize(enemyPos-transform.position),transform.forward)>= 0.8f)
            {
                //damage処理
                PlayerEnergyScript.instance.SlashChargeEnergy();
                //コンボ加算
                comboCount++;
            }
            else
            {
                ComboReset();
            }
        }

        attackTimer++;
    }

    public void ModeChange()
    {
        attackTimer = 0;
    }
    public int GetComboCount()
    {
        return comboCount;
    }
    public void ComboReset()
    {
        comboCount = 0;
    }
}

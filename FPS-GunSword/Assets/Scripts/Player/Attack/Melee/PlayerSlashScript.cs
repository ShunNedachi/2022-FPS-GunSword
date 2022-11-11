using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PlayerSlashScript : MonoBehaviour
{
    public static PlayerSlashScript instance;
    
    [SerializeField] private int attackInterval = 50;
    [SerializeField] private float attackDictance = 5.0f;
    [SerializeField] private float damage = 25;
    [SerializeField] private GameObject camera;
    [SerializeField] private GameObject AttackRange;

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
            Vector3 createPos = transform.position + camera.transform.forward * attackDictance;
            Instantiate(AttackRange, createPos, camera.transform.rotation);
        }
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
    public void AddCombo()
    {
        comboCount++;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PlayerSlashScript : MonoBehaviour
{
    public static PlayerSlashScript instance;
    
    [SerializeField] private int attackInterval = 50;
    [SerializeField] private int comboResetInterval = 300;
    [SerializeField] private float attackDictance = 5.0f;
    [SerializeField] private new GameObject camera;
    [SerializeField] private GameObject AttackRange;
    [SerializeField] public AudioClip sword;

    private float attackTimer = 0;
    private float comboResetTimer = 0;
    private int comboCount = 0;
    AudioSource audioSource;

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
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    public void Update()
    {
        attackTimer++;

        if(Input.GetMouseButton(1) && attackTimer > attackInterval )
        {
            PlayerShotScript.instance.ModeChange();
            attackTimer = 0;
            // プレイヤーの少し前に生成する
            Vector3 createPos = transform.position + camera.transform.forward * attackDictance;
            Instantiate(AttackRange, createPos, camera.transform.rotation);

            audioSource.PlayOneShot(sword);
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
        comboResetTimer = 0;
        comboCount = 0;
    }
    public void AddCombo()
    {
        comboResetTimer = 0;
        comboCount++;
    }
    public Vector3 GetAttackPos()
    {
        Vector3 createPos = transform.position + camera.transform.forward * attackDictance;

        return createPos;
    }
}

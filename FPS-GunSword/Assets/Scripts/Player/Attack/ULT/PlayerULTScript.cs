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
    [SerializeField] public AudioClip sound;
    AudioSource audioSource;


    private float attackTimer = 0;
    private bool action = false;

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
        if(PlayerExcaliburScript.instance.GetAction() == false)
        {
            if(Input.GetMouseButton(1) && attackTimer > attackInterval )
            {
                action =  true;
                PlayerEnergyScript.instance.EnemyConsumptionSlash();
                attackTimer = 0;
                // プレイヤーの少し前に生成する
                Vector3 createPos = transform.position + camera.transform.forward * attackDictance;
                Instantiate(ULTAttackRange, createPos, camera.transform.rotation);
                audioSource.PlayOneShot(sound);
            }
        }
    }
    public Vector3 GetAttackPos()
    {
        Vector3 createPos = transform.position + camera.transform.forward * attackDictance;

        return createPos;
    }

    public bool GetAction()
    {
        return action;
    }

    public void ActionEND()
    {
        action = false;
    }

}

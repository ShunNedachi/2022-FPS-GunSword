using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExcaliburScript : MonoBehaviour
{
    public static PlayerExcaliburScript instance;
    [SerializeField] private int attackInterval = 50;
    [SerializeField] private float attackDictance = 5.0f;
    [SerializeField] private new GameObject camera;
    [SerializeField] private GameObject Excalibur;

    private float attackTimer = 0;
    private float objectRange = 0;
    private bool action = false;

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

            if(Input.GetMouseButton(0) && attackTimer > attackInterval )
            {
                action = true;
                PlayerEnergyScript.instance.EnemyConsumptionSlash();
                attackTimer = 0;
                // プレイヤーの少し前に生成する
                Vector3 createPos = transform.position + camera.transform.forward * attackDictance;
                Instantiate(Excalibur, createPos, transform.rotation);
                PlayerDefaultMove.instance.SetMove(false);
            }
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

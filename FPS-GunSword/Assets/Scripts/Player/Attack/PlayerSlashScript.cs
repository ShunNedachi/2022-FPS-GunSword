using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PlayerSlashScript : MonoBehaviour
{
    public static PlayerSlashScript instance;

    [SerializeField] private GameObject AttackRange;
    [SerializeField] private new GameObject camera;
    [SerializeField] private float forwardPlusValue = 3.0f;

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
        if(Input.GetMouseButtonDown(0))
        {
            // �v���C���[�̏����O�ɐ�������
            Vector3 createPos = transform.position + camera.transform.forward * forwardPlusValue;
            //�U���͈͂��o��������
            Instantiate(AttackRange, createPos, camera.transform.rotation);



        }
    }
}

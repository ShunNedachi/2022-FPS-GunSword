using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemyBulletScript : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float shootRange = 100.0f;
    [SerializeField] private int damage = 30;

    private float totalMoveDistance = 0.0f;

    //�@�����v�Z�p
    private Vector3 initPosition;

    public Vector3 targetV
    {
        get { return targetV; }
        set { targetV = value; }
    }

    public bool IsDead
    {
        get { return IsDead; }
        set { IsDead = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        initPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // ���ʃx�N�g�������ɉ������Ă���
        var tempMoveVector = targetV.normalized * speed;

        transform.position += tempMoveVector;

        // �e�̏��ŏ����p�ɍ��v�ړ��������v�Z
        totalMoveDistance = Vector3.Distance(initPosition, transform.position);


        if(totalMoveDistance > shootRange)
        {
            Destroy(transform.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // �Ƃ肠������Q���ɓ������������
        Destroy(transform.gameObject);

        // �v���C���[�ɓ������Ă������̏���
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerHPScript>().Sethp(damage);
        }
    }
}
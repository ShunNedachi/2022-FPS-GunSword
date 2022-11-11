using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �G���_���[�W���󂯂����p
public class EnemyDamageScript : MonoBehaviour
{
    // ������ύX���ă_���[�W�ʂ𒲐�
    [SerializeField] private bool isHead = false;
    [SerializeField] private float damageMaltiply = 2.0f; 

    // �X�e�[�^�X���Q�Ɨp
    DefaultEnemy enemy;

    // Start is called before the first frame update
    void Start()
    {
        // �ϐ��ɓG�̏��������Ă���
        if (transform.parent.tag == "RangeEnemy")
        {
            enemy = transform.parent.GetComponent<RangeEnemy>();
        }
        else if (transform.parent.tag == "MeleeEnemy")
        {
            enemy = transform.parent.GetComponent<MeleeEnemy>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // �v���C���[�̍U�������������Ƃ��p
    public void HitPlayerAttack(float baseDamage)
    {
        // ��_�ɓ��������Ƃ�
        if (isHead)
        {
            enemy.GetDamage(baseDamage * damageMaltiply);
            enemy.IsStun = true;
        }
        else enemy.GetDamage(baseDamage);

    }
}

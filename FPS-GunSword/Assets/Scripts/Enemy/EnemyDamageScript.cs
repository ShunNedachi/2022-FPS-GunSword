using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �G���_���[�W���󂯂����p
public class EnemyDamageScript : MonoBehaviour
{
    // ������ύX���ă_���[�W�ʂ𒲐�
    [SerializeField] private bool isHead = false;
    [SerializeField] private float damageMaltiply = 2.0f;

    [SerializeField] private AudioClip damageSE;

    // �X�e�[�^�X���Q�Ɨp
    DefaultEnemyScript enemy;

    AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponentInParent<AudioSource>();

        // �ϐ��ɓG�̏��������Ă���
        if (transform.parent.tag == "RangeEnemy")
        {
            enemy = transform.parent.GetComponentInParent<RangeEnemy>();
        }
        else if (transform.parent.tag == "MeleeEnemy")
        {
            enemy = transform.parent.GetComponentInParent<MeleeEnemyScript>();
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    // �v���C���[�̍U�������������Ƃ��p
    public void HitPlayerAttack(float baseDamage, Vector3 hitPoint)
    {
        audio.PlayOneShot(damageSE);
        // ��_�ɓ��������Ƃ�
        if (isHead)
        {
            enemy.GetDamage(baseDamage * damageMaltiply, hitPoint);
            enemy.IsStun = true;
        }
        else
        {
            enemy.GetDamage(baseDamage, hitPoint);
        }
    }

}

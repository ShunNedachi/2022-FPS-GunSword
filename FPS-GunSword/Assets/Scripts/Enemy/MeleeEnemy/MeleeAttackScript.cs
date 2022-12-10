using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackScript : MonoBehaviour
{
    MeleeEnemyScript enemy;

    // Start is called before the first frame update
    void Start()
    {
        // �e�N���X�̃X�e�[�^�X���Q�Ƃ���p
        GameObject parent = transform.parent.parent.gameObject;
        enemy = parent.GetComponent<MeleeEnemyScript>();


    }

    // Update is called once per frame
    void Update()
    {
    }

    // �U�������������Ƃ��̏���
    private void OnTriggerStay(Collider other)
    {
        // �v���C���[���������Ă�����
        if (other.gameObject.tag == "Player" && enemy.AttackActive)
        {
            Debug.Log("Hit Attack to Player");
            other.GetComponent<PlayerHPScript>().Sethp(enemy.TakeDamage());
        }

    }
}

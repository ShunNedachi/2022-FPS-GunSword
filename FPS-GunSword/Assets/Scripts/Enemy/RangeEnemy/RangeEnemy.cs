using UnityEngine;

public class RangeEnemy : DefaultEnemy
{
    // �s���p�^�[���؂�ւ��p
    [SerializeField] bool patern1 = true;



    // Start is called before the first frame update
    void Start()
    {
        InitializeEnemy();

    }

    // Update is called once per frame
    void Update()
    {
        
        if(!patern1)
        {
            // ��ڂ̃p�^�[���̍s��
            Patern2Move();
        }
        else
        {
            // ��ڂ̃p�^�[���̍s��

            // ���E����player�����Ȃ���΃}�[�J�[�ɉ����Ĉړ�
            if (!MoveWithinSight())
            {
                MoveRandom();

                agent.isStopped = false;
                isAttack = false;
            }
            else
            {
                // ���E�ɓ�������player�̈ʒu��ǂ������� ��ōU�����͍X�V���Ȃ��悤�ɏC��
                if (!isAttack) agent.SetDestination(playerObject.transform.position);

                // �U���\�ɂȂ����瓮�����~�߂�
                if(Vector3.Distance(playerObject.transform.position,transform.position) < attackStartDistance)
                {
                    agent.isStopped = true;
                    isAttack = true;
                }
            }

        }

        Dead();
    }
        
    void Patern2Move()
    {
        agent.speed = 0.0f;

        // ���E�Ƀv���C���[��������s��
        if(MoveWithinSight())
        {
            Vector3 lockVector = playerObject.transform.position - transform.position;
            lockVector.y = 0.0f;

            Quaternion quaternion = Quaternion.LookRotation(lockVector);

            transform.rotation = quaternion; 
        }
    }
}

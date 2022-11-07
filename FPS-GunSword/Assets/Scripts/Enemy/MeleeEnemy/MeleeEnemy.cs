using System;
using UnityEngine.AI;
using UnityEngine;

public class MeleeEnemy : DefaultEnemy
{
    // �����ɕς�鋗��
    [SerializeField] private float walkDistance = 100.0f;

    [SerializeField] private int attackFrame = 120;
    [SerializeField] private int attackIntervalFrame = 300;
    [SerializeField] private int attackActiveFrame = 1;

    private bool isWalk = false;

    private int countAttack = 0;
    private int countAttackInterval = 0;
    private int countAttackActive = 0;
    private bool isAttackInterval = false;

    private bool attackActive = false;

    // Start is called before the first frame update
    void Start()
    {
        // ����������
        InitializeEnemy();

        // ���g�̎��ӂɋߋ����G�l�~�[��3,4��������
        System.Random rand = new System.Random();
        int spawnRandNum = rand.Next(3, 5);

        // ���������͌�قǒǉ�
        for (int i = 0; i < spawnRandNum; i++)
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        // ���E����player�����Ȃ���΃}�[�J�[�ɉ����Ĉړ�
        if (!MoveWithinSight())
        {
            MoveRandom();

            agent.isStopped = false;
        }
        else
        {
            // ���E�ɓ�������player�̈ʒu��ǂ������� ��ōU�����͍X�V���Ȃ��悤�ɏC��
            if (!isAttack) 
            {
                agent.SetDestination(playerObject.transform.position);
            }

            // ��苗���ȏ�߂���Ε���
            var distanceToPlayer = Vector3.Distance(playerObject.transform.position, transform.position);
            if(distanceToPlayer <= walkDistance)
            {
                isWalk = true;

                // �U���ł���ʒu�܂ŋ߂Â�����U���s���J�n
                if(distanceToPlayer <= attackStartDistance && !isAttackInterval)
                {
                    isAttack = true; 
                }

            }
            else
            {
                isWalk = false;
            }

        }

        //
        if (isWalk)
        {
            agent.speed = walkSpeed;
        }
        else
        {
            agent.speed = runSpeed;
        }

        // 
        if(isAttack)
        {
            // ��b�s�����~�߂�
            agent.isStopped = true;
            agent.SetDestination(transform.position);


            countAttack++;
            if (countAttack > attackFrame)
            {
                isAttack = false;
                countAttack = 0;
                isAttackInterval = true;

                attackActive = true;
            }
        }

        // �U���̍d����
        if(isAttackInterval)
        {
            countAttackInterval++;

            if (countAttackInterval > attackIntervalFrame)
            {
                isAttackInterval = false;
                countAttackInterval = 0;

            }

        }

        // �U���̎���
        if (attackActive)
        {
            countAttackActive++;

            if(countAttackActive > attackActiveFrame)
            {
                countAttackActive = 0;

                attackActive = false;

                agent.isStopped = false;
            }
        }

        // ���S����
        Dead();
    }

    public bool AttackActive
    {
        get { return attackActive; }

        set { attackActive = value; }
    }
}

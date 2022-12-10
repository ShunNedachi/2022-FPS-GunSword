using UnityEngine;

public class RangeEnemy : DefaultEnemyScript
{
    // �s���p�^�[���؂�ւ��p
    [SerializeField] bool patern1 = true;

    [SerializeField] private float walkDistance = 100.0f;

    [SerializeField] private int attackFrame = 120;
    [SerializeField] private int attackIntervalFrame = 300;
    [SerializeField] private int attackActiveFrame = 1;

    // �e�p�̃I�u�W�F�N�g
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletFixedPosY = 1.0f;

    private bool isWalk = false;

    private int countAttack = 0;
    private int countAttackInterval = 0;
    private int countAttackActive = 0;
    private bool isAttackInterval = false;

    private bool attackActive = false;


    // Start is called before the first frame update
    void Start()
    {
        InitializeEnemy();

    }

    // Update is called once per frame
    void Update()
    {
        //if(!option.IsOption)
        {
            if (!IsStun)
            {
                if (!patern1)
                {
                    // ��ڂ̃p�^�[���̍s��
                    Patern2Move();
                }
                else
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
                        if (distanceToPlayer <= walkDistance)
                        {
                            isWalk = true;

                            // �U���ł���ʒu�܂ŋ߂Â�����U���s���J�n
                            if (distanceToPlayer <= attackStartDistance && !isAttackInterval)
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

                    // �U���O�̗\������
                    if (isAttack)
                    {
                        // ��b�s�����~�߂�
                        agent.isStopped = true;
                        agent.SetDestination(transform.position);


                        countAttack++;
                        // �U���̔���
                        if (countAttack > attackFrame)
                        {
                            isAttack = false;
                            countAttack = 0;
                            isAttackInterval = true;

                            attackActive = true;

                            // �e�̃I�u�W�F�N�g���� �����ʒu���v���C���[�̏����O�ɂ���
                            var fixedPos = new Vector3(transform.position.x,
                                    transform.position.y + bulletFixedPosY, transform.position.z);

                            fixedPos += transform.forward.normalized;
                            var tempObj = Instantiate(bullet, fixedPos, transform.rotation);
                            tempObj.GetComponent<RangeEnemyBulletScript>().targetV =
                                playerObject.transform.position - transform.position;
                        }
                    }

                    // �U���̍d����
                    if (isAttackInterval)
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

                        if (countAttackActive > attackActiveFrame)
                        {
                            countAttackActive = 0;

                            attackActive = false;

                            agent.isStopped = false;
                        }
                    }


                }

            }
            else
            {
                agent.isStopped = true;

                stunCount++;

                // �X�^�����ԕ��~�܂�����
                if (stunCount > stunFrame)
                {
                    agent.isStopped = false;

                    stunCount = 0;
                    IsStun = false;
                }

            }

            // ���S����
            Dead();

        }
    }


    void Patern2Move()
    {
        agent.speed = 0.0f;

        // ���E�Ƀv���C���[��������s��
        if(MoveWithinSight())
        {
            Vector3 lookVector = playerObject.transform.position - transform.position;
            lookVector.y = 0.0f;

            Quaternion quaternion = Quaternion.LookRotation(lookVector);

            transform.rotation = quaternion; 
        }
    }
}

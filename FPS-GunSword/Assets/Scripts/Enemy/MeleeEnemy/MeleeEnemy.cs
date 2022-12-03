using UnityEngine;

public class MeleeEnemy : DefaultEnemy
{
    // �����ɕς�鋗��
    [SerializeField] private float walkDistance = 100.0f;

    [SerializeField] private int attackFrame = 120;
    [SerializeField] private int attackIntervalFrame = 300;
    [SerializeField] private int attackActiveFrame = 1;

    [SerializeField] private float stepDistance = 20.0f;
    [SerializeField] private float stepSpeed = 2.0f;

    private bool isWalk = false;


    // �U�����쒆���ǂ�������p
    private bool attackMotion = false; 
    private int countAttack = 0;
    private int countAttackInterval = 0;
    private int countAttackActive = 0;
    private bool isAttackInterval = false;

    private bool attackActive = false;

    // �X�e�b�v�p�@�ϐ�
    private bool stepInitialize = false;
    private bool canStep = false;
    private Vector3 stepVector;

    private bool backstepInit = false;
    private bool canBackStep = false;
    // �X�e�b�v�����ǂ�������p
    private bool stepMotion = false;


    // Start is called before the first frame update
    void Start()
    {
        // ����������
        InitializeEnemy();

        //// ���g�̎��ӂɋߋ����G�l�~�[��3,4��������
        //System.Random rand = new System.Random();
        //int spawnRandNum = rand.Next(3, 5);

        //// ���������͌�قǒǉ�
        //for (int i = 0; i < spawnRandNum; i++)
        //{
            
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) isDead = true;

        if (!IsStun)
        {
            NormalMove();
        }
        else
        {
            StunMove();
        }

        // �ړ����x�̕ύX
        if(!stepMotion || !MoveWithinSight())MoveSpeedChange();

        // ���S����
        Dead();
    }

    public bool AttackActive
    {
        get { return attackActive; }

        set { attackActive = value; }
    }

    private void StunMove()
    {
        agent.isStopped = true;
        //agent.SetDestination(transform.position);

        stunCount++;

        // �X�^�����ԕ��~�܂�����
        if (stunCount > stunFrame)
        {
            agent.isStopped = false;

            stunCount = 0;
            IsStun = false;
        }
    }
    
    private void NormalMove()
    {
        // ���E����player�����Ȃ���΃}�[�J�[�ɉ����Ĉړ�
        if (!MoveWithinSight())
        {
            MoveRandom();

            agent.isStopped = false;
        }
        else // ���E����player������ꍇ
        {
            // ���E�ɓ�������player�̈ʒu��ǂ������� �U�����A�X�e�b�v������Ȃ���
            if (!attackMotion && !stepMotion)
            {
                agent.SetDestination(playerObject.transform.position);
            }

            // ��苗���ȏ�߂���Ε���
            var distanceToPlayer = Vector3.Distance(playerObject.transform.position, transform.position);
            if (distanceToPlayer <= walkDistance)
            {
                isWalk = true;

                // �U���ł���ʒu�܂ŋ߂Â�����U���s���J�n
                if (distanceToPlayer <= attackStartDistance && !isAttackInterval && !stepMotion)
                {
                    isAttack = true;
                    attackMotion = true;
                }

            }
            else
            {
                isWalk = false;
            }

            // �X�e�b�v�𓥂�
            StepAction();

        }


        // �U���O�̗\������
        if (isAttack)
        {
            // ��b�s�����~�߂�
            agent.isStopped = true;
            agent.SetDestination(transform.position);


            countAttack++;
            // �U������
            if (countAttack > attackFrame)
            {
                isAttack = false;
                countAttack = 0;
                //isAttackInterval = true;

                attackActive = true;
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
                isAttackInterval = true;

                agent.isStopped = false;
            }
        }
        // �U���̍d����
        if (isAttackInterval)
        {
            countAttackInterval++;

            if (countAttackInterval > attackIntervalFrame)
            {
                agent.isStopped = false;
                isAttackInterval = false;
                countAttackInterval = 0;

                // �U�������������
                attackMotion = false;

                // �X�e�b�v�̍s��������悤��
                backstepInit = false;
                canBackStep = true;
                stepMotion = true;
            }

        }

    }

    private void MoveSpeedChange()
    {
        //
        if (isWalk)
        {
            agent.speed = walkSpeed;
        }
        else
        {
            agent.speed = runSpeed;
        }

    }

    private void SetLookPlayer()
    {
        Vector3 lookVector = playerObject.transform.position - transform.position;
        lookVector.y = 0.0f;

        Quaternion quaternion = Quaternion.LookRotation(lookVector);

        transform.rotation = quaternion;

    }

    private void StepAction()
    {
        if (stepMotion && !attackMotion)
        {
            if (canBackStep && !canStep)
            {
                BackStep();

                SetLookPlayer();
            }

            if (canStep && !canBackStep)
            {
                StepLeftAndRight();
                SetLookPlayer();
            }

        }
    }

    private void BackStep()
    {
        if (!backstepInit)
        {
            // �������ɃX�e�b�v���鏈��
            StepInitialize(-transform.forward);

            backstepInit = true;
        }

        if(!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            canBackStep = false;
            canStep = true;


            backstepInit = false;
        }
    }

    private void StepLeftAndRight()
    {
        if (!stepInitialize)
        {
            // �����_���ňړ����������߂�
            // �����_���̐��x�p�ɔO�̂���
            int rand = Random.Range(0, 10);
            if (rand % 2 == 0)
            {
                // �ŏ��ɉE�ֈړ��ł��邩�m�F
                var result = StepInitialize(transform.right);
                // �ǂɓ��������ꍇ���Ɉړ��ł���̂��m�F����
                if (result) StepInitialize(-transform.right);
            }
            else
            {
                // �ŏ��ɍ��Ɉړ��ł���̂��m�F
                var result = StepInitialize(-transform.right);
                // �ړ��ł��Ȃ������ꍇ�ɉE�Ɉړ��ł���̂��m�F
                if (result) StepInitialize(transform.right);

            }


            stepInitialize = true;
        }

        // �ړ����I�������I�������� ��ŕύX�ł���悤�ɂ��邩���H(���̏ꍇremainingDaistance���g�p���Ă���ӏ��S��)
        if(!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            canStep = false;
            stepMotion = false;

            stepInitialize = false;
        }
    }

    // stepV = �ړ������̃x�N�g��
    // �Ԃ�l�͕ǂɓ����������ǂ����̐�������
    private bool StepInitialize(Vector3 stepV)
    {
        // �����l�ł͎��s��������Ă���
        bool moveResult = false;

        stepMotion = true;

        RaycastHit hit;
        Vector3 agentPoint;

        // �����̉E���Ƀ��C���΂�
        if (Physics.Raycast(transform.position, stepV.normalized, out hit, stepDistance))
        {
            // �ǂɃ��C�����������ʒu�������������G�l�~�[���Ɋ񂹂��ʒu��
            // �ړ��n�_��ݒu����

            // �ǂƂ̋������m�F���Ă���g�����ǂ����l����
            const float DISTANCE_WALL = 0.1f;
            agentPoint = hit.point + (-stepV * DISTANCE_WALL);

            moveResult = true;
        }
        else
        {
            // stepDistance�̋������̈ʒu�Ɉړ��n�_��ݒ肷��
            agentPoint = transform.position + (stepV.normalized * stepDistance);

            moveResult = false;
        }

        // �v�Z�����ʒu�Ɏ��̈ړ��ڕW��ݒ�
        agent.SetDestination(agentPoint);
        agent.speed = stepSpeed;

        return moveResult;
    }

}

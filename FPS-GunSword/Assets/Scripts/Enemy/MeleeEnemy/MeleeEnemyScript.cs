using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyScript : DefaultEnemyScript
{
    [SerializeField] private float walkDistance = 100.0f;

    // for Attack
    [SerializeField] private int attackFrame = 120;
    [SerializeField] private int attackIntervalFrame = 300;
    [SerializeField] private int attackActiveFrame = 1;
    [SerializeField] private GameObject attackObject;
    // for Step
    [SerializeField] private float stepDistance = 20.0f;
    [SerializeField] private float stepSpeed = 20.0f;


    private bool isWalk = false;

    // for Attack
    private bool attackInitialize = false;
    private int countAttack = 0;
    private int countAttackInterval = 0;
    private int countAttackActive = 0;
    private bool isAttackInterval = false;
    private bool attackActive = false;
    // for AttackHit
    private Collider attackCollider;

    // for Step
    private bool stepInitialize = false;
    private bool canStep = false;
    private bool backstepInit = false;
    private bool canBackStep = false;

    // Start is called before the first frame update
    void Start()
    {
        // Init State
        state = enemyState.patrol;
        // Initalize EnemyInfo
        InitializeEnemy();

        // attackHit
        attackCollider = attackObject.GetComponent<Collider>();
        attackCollider.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        // ���E����O�ꂽ���ɍs���p�^�[���؂�ւ�
        if (!MoveWithinSight()) ChangePatrol();

        // state���Q�Ƃ��čs���p�^�[���؂�ւ�
        switch (state)
        {
            case enemyState.patrol:
                
                PatrolMove();
                break;

            case enemyState.follow:

                FollowMove();
                
                break;

            case enemyState.attack:
                AttackMove();

                break;

            case enemyState.step:
                StepMove();
                
                break;

            case enemyState.stun:
                
                StunMove();
                break;
        }


        // �X�^���������ǂ����̃`�F�b�N
        StunCheck();
        // ���S���̏���
        Dead();
    }

    public bool AttackActive
    {
        get { return attackActive; }

        set { attackActive = value; }
    }

    private void MoveSpeedChange()
    {
        // for Follow
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

    private void PatrolMove()
    {
        // player�����E�ɓ������Ƃ��ɍs���p�^�[���؂�ւ�
        if(MoveWithinSight())
        {
            ChangeFollow();
        }
 

        MoveRandom();
    }

    private void FollowMove()
    {
        agent.SetDestination(playerObject.transform.position);

        var distance = Vector3.Distance(transform.position, playerObject.transform.position);
        if (distance <= walkDistance)
        {
            isWalk = true;

            // �͈͂ɓ����Ă�����
            if(distance <= attackStartDistance) { ChangeAttack(); }

        }
        else isWalk = false;
    }

    private void AttackMove()
    {
        if(attackInitialize)
        {
            // �����蔻��L����
            attackCollider.enabled = true;

            attackInitialize = false;
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

                // �U���̔��薳����
                attackCollider.enabled = false;
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

                // �X�e�b�v�̍s��������悤��

                ChangeStep();
            }

        }

    }

    private void StepMove()
    {
        if (canBackStep && !canStep)
        {
            BackStep();

            SetLookPlayer();
        }
        else if (canStep && !canBackStep)
        {
            StepLeftAndRight();
            SetLookPlayer();
        }
    }

    // for Stun
    private void StunMove()
    {
        // stopped
        agent.isStopped = true;

        stunCount++;
        // �X�^�����ԕ��~�܂�����
        if (stunCount > stunFrame)
        {
            agent.isStopped = false;

            stunCount = 0;
            IsStun = false;

            state = beforeStunState;
        }
    }

    private void ChangePatrol()
    {
        // ������E���n������ɕύX����悤�ɕύX
        // �p�g���[��������Ȃ������猩�n���悤�ɂ���
        if(state != enemyState.patrol) { }

        state = enemyState.patrol;
    }

    private void ChangeFollow()
    {
        state = enemyState.follow;
    }

    private void ChangeAttack()
    {
        state = enemyState.attack;

        isAttack = true;
        attackInitialize = true;
    }
    
    private void ChangeStep()
    {
        // �X�e�b�v�̏�����
        backstepInit = false;
        canBackStep = true;

        state = enemyState.step;
    }

    private void BackStep()
    {
        if (!backstepInit)
        {
            // �������ɃX�e�b�v���鏈��
            StepInitialize(-transform.forward);

            backstepInit = true;
        }

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
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
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            canStep = false;
            stepInitialize = false;

            ChangeFollow();
        }
    }

    // stepV = �ړ������̃x�N�g��
    // �Ԃ�l�͕ǂɓ����������ǂ����̐�������
    private bool StepInitialize(Vector3 stepV)
    {
        // �����l�ł͎��s��������Ă���
        bool moveResult = false;

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

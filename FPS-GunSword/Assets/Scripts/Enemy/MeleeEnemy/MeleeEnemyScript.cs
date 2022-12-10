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

    // for patrol
    private bool patrolEffect = false;
    private bool patrolInit = false;
    private int overLockCount = 0;
    private bool firstOverLocktoRight = false;
    private bool overLockSet = false;
    private float patrolTotalRot;
    [SerializeField]private float overLockStep = 2.0f;
    [SerializeField]private float overlockAngle = 90;
    [SerializeField] private int overLockNum = 2;

    [SerializeField] private AudioClip attackSE;



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

        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!option.IsOption)
        {
            // 視界から外れた時に行動パターン切り替え
            if (!MoveWithinSight()) ChangePatrol();

            // stateを参照して行動パターン切り替え
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

            MoveSpeedChange();

            // スタンしたかどうかのチェック
            StunCheck();
            // 死亡時の処理
            Dead();

        }
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
        // �ｽ�ｽ�ｽn�ｽ�ｽ�ｽG�ｽt�ｽF�ｽN�ｽg�ｽ�ｽ�ｽK�ｽv�ｽﾈとゑｿｽ
        if (patrolEffect)
        {
            if (patrolInit) PatrolInitalize();

            // 
            if (overLockCount < overLockNum)
            {
                // �ｽ�ｽ]�ｽ�ｽ�ｽ�ｽp�ｽx�ｽﾌ設抵ｿｽ
                if (!overLockSet)
                {
                    SetPatrolRotation();

                    overLockSet = true;
                }


                if(firstOverLocktoRight)
                {
                    var rotateAngle = overlockAngle;
                    if (overLockCount > 0) rotateAngle *= 2;

                    // y�ｽ�ｽ�ｽﾅ会ｿｽ]�ｽ�ｽ�ｽ�ｽ�ｽ�ｽ�ｽ�ｽ
                    transform.Rotate(new Vector3(0, overLockStep, 0));
                    patrolTotalRot += overLockStep;

                    // �ｽ�ｽ�ｽ�ｽx�ｽ�ｽ�ｽ�ｽ]�ｽ�ｽ�ｽﾄゑｿｽ�ｽ�ｽ�ｽ�ｽI�ｽ�ｽ
                    if(patrolTotalRot >= overlockAngle)
                    {
                        overLockSet = false;
                        firstOverLocktoRight = !firstOverLocktoRight;
                        overLockCount++;
                    }
                }
                else
                {
                    var rotateAngle = overlockAngle;
                    if (overLockCount > 0) rotateAngle *= 2;

                    // y�ｽ�ｽ�ｽﾅ会ｿｽ]�ｽ�ｽ�ｽ�ｽ�ｽ�ｽ�ｽ�ｽ
                    transform.Rotate(new Vector3(0, -overLockStep, 0));
                    patrolTotalRot += overLockStep;

                    // �ｽ�ｽ�ｽ�ｽx�ｽ�ｽ�ｽ�ｽ]�ｽ�ｽ�ｽﾄゑｿｽ�ｽ�ｽ�ｽ�ｽI�ｽ�ｽ
                    if (patrolTotalRot >= overlockAngle)
                    {
                        overLockSet = false;
                        firstOverLocktoRight = !firstOverLocktoRight;
                        overLockCount++;
                    }
                }


            }
            else
            {
                // �ｽ�ｽ�ｽn�ｽ�ｽ�ｽ�ｽ�ｽI�ｽ�ｽ
                patrolEffect = false;
                // �ｽs�ｽ�ｽ�ｽ�ｽ�ｽﾄ起�ｽ�ｽ
                agent.isStopped = false;
            }
        }


        // player�ｽ�ｽ�ｽ�ｽ�ｽE�ｽﾉ難ｿｽ�ｽ�ｽ�ｽ�ｽ�ｽﾆゑｿｽ�ｽﾉ行�ｽ�ｽ�ｽp�ｽ^�ｽ[�ｽ�ｽ�ｽﾘゑｿｽﾖゑｿｽ
        if (MoveWithinSight())
        {
            ChangeFollow();
        }

        if (!patrolEffect)
        {
            MoveRandom();
        }
    }

    private void FollowMove()
    {
        agent.SetDestination(playerObject.transform.position);

        var distance = Vector3.Distance(transform.position, playerObject.transform.position);
        if (distance <= walkDistance)
        {
            isWalk = true;

            // �ｽﾍ囲に難ｿｽ�ｽ�ｽ�ｽﾄゑｿｽ�ｽ�ｽ�ｽ�ｽ
            if (distance <= attackStartDistance) { ChangeAttack(); }

        }
        else isWalk = false;
    }

    private void AttackMove()
    {

        // �ｽU�ｽ�ｽ�ｽO�ｽﾌ予�ｽ�ｽ�ｽ�ｽ�ｽ�ｽ
        if (isAttack)
        {
            // �ｽ�ｽb�ｽs�ｽ�ｽ�ｽ�ｽ�ｽ~�ｽﾟゑｿｽ
            agent.isStopped = true;
            agent.SetDestination(transform.position);


            countAttack++;
            // �ｽU�ｽ�ｽ�ｽ�ｽ�ｽ�ｽ
            if (countAttack > attackFrame)
            {
                isAttack = false;
                countAttack = 0;
                //isAttackInterval = true;

                audio.PlayOneShot(attackSE);

                attackActive = true;
            }
        }
        // �ｽU�ｽ�ｽ�ｽﾌ趣ｿｽ�ｽ�ｽ
        if (attackActive)
        {
            if (attackInitialize)
            {
                // �ｽ�ｽ�ｽ�ｽ�ｽ阡ｻ�ｽ�ｽL�ｽ�ｽ�ｽ�ｽ
                attackCollider.enabled = true;

                attackInitialize = false;
            }

            countAttackActive++;

            if (countAttackActive > attackActiveFrame)
            {
                countAttackActive = 0;

                attackActive = false;
                isAttackInterval = true;

                // �ｽU�ｽ�ｽ�ｽﾌ費ｿｽ�ｽ阮ｳ�ｽ�ｽ�ｽ�ｽ
                attackCollider.enabled = false;
            }
        }
        // �ｽU�ｽ�ｽ�ｽﾌ硬�ｽ�ｽ�ｽ�ｽ
        if (isAttackInterval)
        {
            countAttackInterval++;

            if (countAttackInterval > attackIntervalFrame)
            {
                agent.isStopped = false;
                isAttackInterval = false;
                countAttackInterval = 0;

                // �ｽX�ｽe�ｽb�ｽv�ｽﾌ行�ｽ�ｽ�ｽ�ｽ�ｽ�ｽ�ｽ�ｽ謔､�ｽ�ｽ

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
        // �ｽX�ｽ^�ｽ�ｽ�ｽ�ｽ�ｽﾔ包ｿｽ�ｽ~�ｽﾜゑｿｽ�ｽ�ｽ�ｽ�ｽ
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
        // �ｽ�ｽ�ｽ�ｽ�ｽ�ｽE�ｽ�ｽ�ｽn�ｽ�ｽ�ｽ�ｽ�ｽ�ｽﾉ変更�ｽ�ｽ�ｽ�ｽ謔､�ｽﾉ変更
        // �ｽp�ｽg�ｽ�ｽ�ｽ[�ｽ�ｽ�ｽ�ｽ�ｽ�ｽ�ｽ�ｽﾈゑｿｽ�ｽ�ｽ�ｽ�ｽ�ｽ迪ｩ�ｽn�ｽ�ｽ�ｽ謔､�ｽﾉゑｿｽ�ｽ�ｽ
        if (state != enemyState.patrol)
        {
            patrolInit = true;
            patrolEffect = true;

            overLockSet = false;
        }

        state = enemyState.patrol;

        isWalk = false;
    }

    private void ChangeFollow()
    {
        agent.isStopped = false;
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
        // �ｽX�ｽe�ｽb�ｽv�ｽﾌ擾ｿｽ�ｽ�ｽ�ｽ�ｽ
        backstepInit = false;
        canBackStep = true;

        state = enemyState.step;
    }

    private void BackStep()
    {
        if (!backstepInit)
        {
            // �ｽ�ｽ�ｽ�ｽ�ｽ�ｽ�ｽﾉス�ｽe�ｽb�ｽv�ｽ�ｽ�ｽ髀茨ｿｽ�ｽ
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
            // �ｽ�ｽ�ｽ�ｽ�ｽ_�ｽ�ｽ�ｽﾅ移難ｿｽ�ｽ�ｽ�ｽ�ｽ�ｽ�ｽ�ｽ�ｽ�ｽﾟゑｿｽ
            // �ｽ�ｽ�ｽ�ｽ�ｽ_�ｽ�ｽ�ｽﾌ撰ｿｽ�ｽx�ｽp�ｽﾉ念�ｽﾌゑｿｽ�ｽ�ｽ
            int rand = Random.Range(0, 10);
            if (rand % 2 == 0)
            {
                // �ｽﾅ擾ｿｽ�ｽﾉ右�ｽﾖ移難ｿｽ�ｽﾅゑｿｽ�ｽ驍ｩ�ｽm�ｽF
                var result = StepInitialize(transform.right);
                // �ｽﾇに難ｿｽ�ｽ�ｽ�ｽ�ｽ�ｽ�ｽ�ｽ鼾�ｿｽ�ｽ�ｽﾉ移難ｿｽ�ｽﾅゑｿｽ�ｽ�ｽﾌゑｿｽ�ｽm�ｽF�ｽ�ｽ�ｽ�ｽ
                if (result) StepInitialize(-transform.right);
            }
            else
            {
                // �ｽﾅ擾ｿｽ�ｽﾉ搾ｿｽ�ｽﾉ移難ｿｽ�ｽﾅゑｿｽ�ｽ�ｽﾌゑｿｽ�ｽm�ｽF
                var result = StepInitialize(-transform.right);
                // �ｽﾚ難ｿｽ�ｽﾅゑｿｽ�ｽﾈゑｿｽ�ｽ�ｽ�ｽ�ｽ�ｽ鼾�ｿｽﾉ右�ｽﾉ移難ｿｽ�ｽﾅゑｿｽ�ｽ�ｽﾌゑｿｽ�ｽm�ｽF
                if (result) StepInitialize(transform.right);

            }


            stepInitialize = true;
        }

        // �ｽﾚ難ｿｽ�ｽ�ｽ�ｽI�ｽ�ｽ�ｽ�ｽ�ｽ�ｽ�ｽI�ｽ�ｽ�ｽ�ｽ�ｽ�ｽ�ｽ�ｽ �ｽ�ｽﾅ変更�ｽﾅゑｿｽ�ｽ�ｽ謔､�ｽﾉゑｿｽ�ｽ驍ｩ�ｽ�ｽ�ｽH(�ｽ�ｽ�ｽﾌ場合remainingDaistance�ｽ�ｽ�ｽg�ｽp�ｽ�ｽ�ｽﾄゑｿｽ�ｽ�ｽﾓ擾ｿｽ�ｽS�ｽ�ｽ)
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            canStep = false;
            stepInitialize = false;

            ChangeFollow();
        }
    }

    // stepV = �ｽﾚ難ｿｽ�ｽ�ｽ�ｽ�ｽ�ｽﾌベ�ｽN�ｽg�ｽ�ｽ
    // �ｽﾔゑｿｽl�ｽﾍ壁に難ｿｽ�ｽ�ｽ�ｽ�ｽ�ｽ�ｽ�ｽ�ｽ�ｽﾇゑｿｽ�ｽ�ｽ�ｽﾌ撰ｿｽ�ｽ�ｽ�ｽ�ｽ�ｽ�ｽ
    private bool StepInitialize(Vector3 stepV)
    {
        // �ｽ�ｽ�ｽ�ｽ�ｽl�ｽﾅは趣ｿｽ�ｽs�ｽ�ｽ�ｽ�ｽ�ｽ�ｽ�ｽ�ｽﾄゑｿｽ�ｽ�ｽ
        bool moveResult = false;

        RaycastHit hit;
        Vector3 agentPoint;

        // �ｽ�ｽ�ｽ�ｽ�ｽﾌ右�ｽ�ｽ�ｽﾉ�ｿｽ�ｽC�ｽ�ｽ�ｽﾎゑｿｽ
        if (Physics.Raycast(transform.position, stepV.normalized, out hit, stepDistance))
        {
            // �ｽﾇに�ｿｽ�ｽC�ｽ�ｽ�ｽ�ｽ�ｽ�ｽ�ｽ�ｽ�ｽ�ｽ�ｽﾊ置�ｽ�ｽ�ｽ�ｽ�ｽ�ｽ�ｽ�ｽ�ｽ�ｽ�ｽ�ｽ�ｽG�ｽl�ｽ~�ｽ[�ｽ�ｽ�ｽﾉ寄せゑｿｽ�ｽﾊ置�ｽ�ｽ
            // �ｽﾚ難ｿｽ�ｽn�ｽ_�ｽ�ｽﾝ置�ｽ�ｽ�ｽ�ｽ

            // �ｽﾇとの具ｿｽ�ｽ�ｽ�ｽ�ｽ�ｽm�ｽF�ｽ�ｽ�ｽﾄゑｿｽ�ｽ�ｽg�ｽ�ｽ�ｽ�ｽ�ｽﾇゑｿｽ�ｽ�ｽ�ｽl�ｽ�ｽ�ｽ�ｽ
            const float DISTANCE_WALL = 0.1f;
            agentPoint = hit.point + (-stepV * DISTANCE_WALL);

            moveResult = true;
        }
        else
        {
            // stepDistance�ｽﾌ具ｿｽ�ｽ�ｽ�ｽ�ｽ�ｽﾌ位置�ｽﾉ移難ｿｽ�ｽn�ｽ_�ｽ�ｽﾝ定す�ｽ�ｽ
            agentPoint = transform.position + (stepV.normalized * stepDistance);

            moveResult = false;
        }

        // �ｽv�ｽZ�ｽ�ｽ�ｽ�ｽ�ｽﾊ置�ｽﾉ趣ｿｽ�ｽﾌ移難ｿｽ�ｽﾚ標�ｽ�ｽﾝ抵ｿｽ
        agent.SetDestination(agentPoint);
        agent.speed = stepSpeed;

        return moveResult;
    }

    private void PatrolInitalize()
    {
        // �ｽ�ｽU�ｽ~�ｽﾟてゑｿｽ�ｽ�ｽ
        agent.isStopped = true;

        overLockCount = 0;

        var temp = Random.Range(0, 10);
        temp %= 2;

        if (temp == 0) firstOverLocktoRight = false;
        else firstOverLocktoRight = true;

        overLockSet = false;

        patrolInit = false;
    }

    private void SetPatrolRotation()
    {
        patrolTotalRot = 0;
        //if (firstOverLocktoRight)
        //{

        //    patrolEffectEndRot =
        //        Quaternion.AngleAxis(overlockAngle, Vector3.up) * transform.rotation;
        //}
        //else
        //{
        //    patrolEffectEndRot =
        //        Quaternion.AngleAxis(-overlockAngle, Vector3.up) * transform.rotation;
        //}

        //// �ｽ�ｽ�ｽｽ対ゑｿｽ�ｽ�ｽ�ｽ�ｽ�ｽ謔､�ｽﾉ費ｿｽ�ｽ]�ｽ�ｽ�ｽ�ｽ�ｽ�ｽ
        //firstOverLocktoRight = !firstOverLocktoRight;
    }
}

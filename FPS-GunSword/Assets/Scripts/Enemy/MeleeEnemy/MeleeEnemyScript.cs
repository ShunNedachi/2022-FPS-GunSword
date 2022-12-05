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


        // スタンしたかどうかのチェック
        StunCheck();
        // 死亡時の処理
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
        // playerが視界に入ったときに行動パターン切り替え
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

            // 範囲に入っていたら
            if(distance <= attackStartDistance) { ChangeAttack(); }

        }
        else isWalk = false;
    }

    private void AttackMove()
    {
        if(attackInitialize)
        {
            // 当たり判定有効化
            attackCollider.enabled = true;

            attackInitialize = false;
        }

        // 攻撃前の予備動作
        if (isAttack)
        {
            // 基礎行動を止める
            agent.isStopped = true;
            agent.SetDestination(transform.position);


            countAttack++;
            // 攻撃発生
            if (countAttack > attackFrame)
            {
                isAttack = false;
                countAttack = 0;
                //isAttackInterval = true;

                attackActive = true;
            }
        }
        // 攻撃の持続
        if (attackActive)
        {
            countAttackActive++;

            if (countAttackActive > attackActiveFrame)
            {
                countAttackActive = 0;

                attackActive = false;
                isAttackInterval = true;

                // 攻撃の判定無効化
                attackCollider.enabled = false;
            }
        }
        // 攻撃の硬直中
        if (isAttackInterval)
        {
            countAttackInterval++;

            if (countAttackInterval > attackIntervalFrame)
            {
                agent.isStopped = false;
                isAttackInterval = false;
                countAttackInterval = 0;

                // ステップの行動を取れるように

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
        // スタン時間分止まったら
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
        // 後程左右見渡した後に変更するように変更
        // パトロール中じゃなかったら見渡すようにする
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
        // ステップの初期化
        backstepInit = false;
        canBackStep = true;

        state = enemyState.step;
    }

    private void BackStep()
    {
        if (!backstepInit)
        {
            // 後ろ方向にステップする処理
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
            // ランダムで移動方向を決める
            // ランダムの精度用に念のため
            int rand = Random.Range(0, 10);
            if (rand % 2 == 0)
            {
                // 最初に右へ移動できるか確認
                var result = StepInitialize(transform.right);
                // 壁に当たった場合左に移動できるのか確認する
                if (result) StepInitialize(-transform.right);
            }
            else
            {
                // 最初に左に移動できるのか確認
                var result = StepInitialize(-transform.right);
                // 移動できなかった場合に右に移動できるのか確認
                if (result) StepInitialize(transform.right);

            }


            stepInitialize = true;
        }

        // 移動が終わったら終了時処理 後で変更できるようにするかも？(その場合remainingDaistanceを使用している箇所全て)
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            canStep = false;
            stepInitialize = false;

            ChangeFollow();
        }
    }

    // stepV = 移動方向のベクトル
    // 返り値は壁に当たったかどうかの成功判定
    private bool StepInitialize(Vector3 stepV)
    {
        // 初期値では失敗判定を入れておく
        bool moveResult = false;

        RaycastHit hit;
        Vector3 agentPoint;

        // 自分の右側にレイを飛ばす
        if (Physics.Raycast(transform.position, stepV.normalized, out hit, stepDistance))
        {
            // 壁にレイが当たった位置よりも少しだけエネミー側に寄せた位置に
            // 移動地点を設置する

            // 壁との距離を確認してから使うかどうか考える
            const float DISTANCE_WALL = 0.1f;
            agentPoint = hit.point + (-stepV * DISTANCE_WALL);

            moveResult = true;
        }
        else
        {
            // stepDistanceの距離分の位置に移動地点を設定する
            agentPoint = transform.position + (stepV.normalized * stepDistance);

            moveResult = false;
        }

        // 計算した位置に次の移動目標を設定
        agent.SetDestination(agentPoint);
        agent.speed = stepSpeed;

        return moveResult;
    }
}

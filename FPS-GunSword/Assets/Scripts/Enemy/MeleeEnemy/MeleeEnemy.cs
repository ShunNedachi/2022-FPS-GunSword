using UnityEngine;

public class MeleeEnemy : DefaultEnemy
{
    // 歩きに変わる距離
    [SerializeField] private float walkDistance = 100.0f;

    [SerializeField] private int attackFrame = 120;
    [SerializeField] private int attackIntervalFrame = 300;
    [SerializeField] private int attackActiveFrame = 1;

    [SerializeField] private float stepDistance = 20.0f;
    [SerializeField] private float stepSpeed = 2.0f;

    private bool isWalk = false;


    // 攻撃動作中かどうか判定用
    private bool attackMotion = false; 
    private int countAttack = 0;
    private int countAttackInterval = 0;
    private int countAttackActive = 0;
    private bool isAttackInterval = false;

    private bool attackActive = false;

    // ステップ用　変数
    private bool stepInitialize = false;
    private bool canStep = false;
    private Vector3 stepVector;

    private bool backstepInit = false;
    private bool canBackStep = false;
    // ステップ中かどうか判定用
    private bool stepMotion = false;


    // Start is called before the first frame update
    void Start()
    {
        // 初期化処理
        InitializeEnemy();

        //// 自身の周辺に近距離エネミーを3,4沸かせる
        //System.Random rand = new System.Random();
        //int spawnRandNum = rand.Next(3, 5);

        //// 生成処理は後ほど追加
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

        // 移動速度の変更
        if(!stepMotion || !MoveWithinSight())MoveSpeedChange();

        // 死亡処理
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

        // スタン時間分止まったら
        if (stunCount > stunFrame)
        {
            agent.isStopped = false;

            stunCount = 0;
            IsStun = false;
        }
    }
    
    private void NormalMove()
    {
        // 視界内にplayerがいなければマーカーに沿って移動
        if (!MoveWithinSight())
        {
            MoveRandom();

            agent.isStopped = false;
        }
        else // 視界内にplayerがいる場合
        {
            // 視界に入ったらplayerの位置を追いかける 攻撃時、ステップ中じゃない時
            if (!attackMotion && !stepMotion)
            {
                agent.SetDestination(playerObject.transform.position);
            }

            // 一定距離以上近ければ歩く
            var distanceToPlayer = Vector3.Distance(playerObject.transform.position, transform.position);
            if (distanceToPlayer <= walkDistance)
            {
                isWalk = true;

                // 攻撃できる位置まで近づいたら攻撃行動開始
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

            // ステップを踏む
            StepAction();

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

                agent.isStopped = false;
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

                // 攻撃中判定を除去
                attackMotion = false;

                // ステップの行動を取れるように
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
            // 後ろ方向にステップする処理
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
        if(!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            canStep = false;
            stepMotion = false;

            stepInitialize = false;
        }
    }

    // stepV = 移動方向のベクトル
    // 返り値は壁に当たったかどうかの成功判定
    private bool StepInitialize(Vector3 stepV)
    {
        // 初期値では失敗判定を入れておく
        bool moveResult = false;

        stepMotion = true;

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

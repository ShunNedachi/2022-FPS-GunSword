using System;
using UnityEngine.AI;
using UnityEngine;

public class MeleeEnemy : DefaultEnemy
{
    // 歩きに変わる距離
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
        // 初期化処理
        InitializeEnemy();

        // 自身の周辺に近距離エネミーを3,4沸かせる
        System.Random rand = new System.Random();
        int spawnRandNum = rand.Next(3, 5);

        // 生成処理は後ほど追加
        for (int i = 0; i < spawnRandNum; i++)
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) isDead = true;

        if (!IsStun)
        {
            // 視界内にplayerがいなければマーカーに沿って移動
            if (!MoveWithinSight())
            {
                MoveRandom();

                agent.isStopped = false;
            }
            else
            {
                // 視界に入ったらplayerの位置を追いかける 後で攻撃時は更新しないように修正
                if (!isAttack)
                {
                    agent.SetDestination(playerObject.transform.position);
                }

                // 一定距離以上近ければ歩く
                var distanceToPlayer = Vector3.Distance(playerObject.transform.position, transform.position);
                if (distanceToPlayer <= walkDistance)
                {
                    isWalk = true;

                    // 攻撃できる位置まで近づいたら攻撃行動開始
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
                    isAttackInterval = true;

                    attackActive = true;
                }
            }

            // 攻撃の硬直中
            if (isAttackInterval)
            {
                countAttackInterval++;

                if (countAttackInterval > attackIntervalFrame)
                {
                    isAttackInterval = false;
                    countAttackInterval = 0;

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

                    agent.isStopped = false;
                }
            }


        }
        else
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


        // 死亡処理
        Dead();
    }

    public bool AttackActive
    {
        get { return attackActive; }

        set { attackActive = value; }
    }
}

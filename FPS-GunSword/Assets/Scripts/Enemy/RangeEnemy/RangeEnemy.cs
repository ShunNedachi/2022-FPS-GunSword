using UnityEngine;

public class RangeEnemy : DefaultEnemyScript
{
    // 行動パターン切り替え用
    [SerializeField] bool patern1 = true;

    [SerializeField] private float walkDistance = 100.0f;

    [SerializeField] private int attackFrame = 120;
    [SerializeField] private int attackIntervalFrame = 300;
    [SerializeField] private int attackActiveFrame = 1;

    // 弾用のオブジェクト
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
        if(!IsStun)
        {
            if (!patern1)
            {
                // 二つ目のパターンの行動
                Patern2Move();
            }
            else
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
                    // 攻撃の発射
                    if (countAttack > attackFrame)
                    {
                        isAttack = false;
                        countAttack = 0;
                        isAttackInterval = true;

                        attackActive = true;

                        // 弾のオブジェクト生成 生成位置をプレイヤーの少し前にする
                        var fixedPos = new Vector3(transform.position.x,
                                transform.position.y + bulletFixedPosY, transform.position.z);

                        fixedPos += transform.forward.normalized;
                        var tempObj = Instantiate(bullet, fixedPos, transform.rotation);
                        tempObj.GetComponent<RangeEnemyBulletScript>().targetV =
                            playerObject.transform.position - transform.position;
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

        }
        else
        {
            agent.isStopped = true;

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


    void Patern2Move()
    {
        agent.speed = 0.0f;

        // 視界にプレイヤーがいたら行動
        if(MoveWithinSight())
        {
            Vector3 lookVector = playerObject.transform.position - transform.position;
            lookVector.y = 0.0f;

            Quaternion quaternion = Quaternion.LookRotation(lookVector);

            transform.rotation = quaternion; 
        }
    }
}

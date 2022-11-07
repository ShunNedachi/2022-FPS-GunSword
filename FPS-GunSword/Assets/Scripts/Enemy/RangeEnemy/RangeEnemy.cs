using UnityEngine;

public class RangeEnemy : DefaultEnemy
{
    // 行動パターン切り替え用
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
            // 二つ目のパターンの行動
            Patern2Move();
        }
        else
        {
            // 一つ目のパターンの行動

            // 視界内にplayerがいなければマーカーに沿って移動
            if (!MoveWithinSight())
            {
                MoveRandom();

                agent.isStopped = false;
                isAttack = false;
            }
            else
            {
                // 視界に入ったらplayerの位置を追いかける 後で攻撃時は更新しないように修正
                if (!isAttack) agent.SetDestination(playerObject.transform.position);

                // 攻撃可能になったら動きを止める
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

        // 視界にプレイヤーがいたら行動
        if(MoveWithinSight())
        {
            Vector3 lockVector = playerObject.transform.position - transform.position;
            lockVector.y = 0.0f;

            Quaternion quaternion = Quaternion.LookRotation(lockVector);

            transform.rotation = quaternion; 
        }
    }
}

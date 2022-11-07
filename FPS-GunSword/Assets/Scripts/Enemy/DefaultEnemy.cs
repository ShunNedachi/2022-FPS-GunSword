using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 敵のベースになる親クラス
public class DefaultEnemy : MonoBehaviour
{
    // HP
    [SerializeField] protected float hp = 100.0f;
    // ダメージ量
    [SerializeField] protected float damageValue = 30.0f;
    // 視認距離
    [SerializeField] protected float sightDistance = 100.0f;
    // 歩きのスピード
    [SerializeField] protected float walkSpeed = 10.0f;
    // 走りのスピード
    [SerializeField] protected float runSpeed = 30.0f;
    // 怯みの時間
    [SerializeField] protected float stunTime = 3.0f;
    // とりあえず持っておく 消滅にかかる時間
    [SerializeField] protected float deadTime = 3.0f;
    // どの範囲にプレイヤーが入ったら攻撃を開始するか
    [SerializeField] protected float attackStartDistance = 5.0f;

    // 視界に入ったときに追いかける対象
    [SerializeField] protected GameObject playerObject;
    // 敵が集団として反応する距離
    [SerializeField] protected float groupDistance = 30.0f;
    // 敵の視角
    [SerializeField] protected float visualAngle = 70.0f;



    // 基本行動用
    protected int destinationIndex = 0;
    protected GameObject[] navPointsObj;
    protected Rigidbody rigidBody;
    protected NavMeshAgent agent;

    // 視界に入っているのか　集団が反応させるために主に使用
    protected bool isInSight = false;
    // 攻撃中かどうか
    protected bool isAttack = false;
    protected bool isDead = false;


    // Start is called before the first frame update
    void Start()
    {
        InitializeEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        MoveRandom();
    }

    public void MoveRandom()
    {
        // エージェントが現目標地点に近づいてきたら、
        // 次の目標地点を選択
        if (!agent.pathPending && agent.remainingDistance < 0.5f)GetNextPoint();
    }

    public void GetNextPoint()
    {
        if (navPointsObj.Length == 0)
        {
            return;
        }

        agent.SetDestination(navPointsObj[destinationIndex].transform.position);

        //配列のインデックスを+1して、最後の地点だった場合は0に戻す
        destinationIndex = (destinationIndex + 1) % navPointsObj.Length;
    }


    // 視界にプレイヤーが入っているかどうか
    public bool MoveWithinSight()
    {

        // 視界にオブジェクトがあるか判定
        var diff = playerObject.transform.position - transform.position;
        var axis = Vector3.Cross(transform.forward, diff);
        var angle = Vector3.Angle(transform.forward, diff) * (axis.y < 0 ? -1 : 1); 
        if (angle <= visualAngle && angle >= -visualAngle)
        {
            // 視界にオブジェクトがあればレイを飛ばす
            RaycastHit hit;
            Vector3 temp = playerObject.transform.position - transform.position;
            Vector3 normal = temp.normalized;

            if (Physics.Raycast(transform.position, normal, out hit, sightDistance))
            {
                // playerが視界内にいたとき
                if(hit.transform.gameObject == playerObject)
                {
                    return true;
                }
            }

        }

        // 視界内にplayerがいないときfalse
        return false;
    }

    // 親クラスの初期化処理を子クラスで利用するために作成
    public void InitializeEnemy()
    {
        // エラーをなくすためのもの
        stunTime = 3.0f;
        deadTime = 3.0f;

        // もし何も入っていなかったときにplayerタグから代入
        if (playerObject == null)
        {
            playerObject = GameObject.FindGameObjectWithTag("Player");
        }

        // 基本行動用
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = 0.1f;

        navPointsObj = GameObject.FindGameObjectsWithTag("EnemyMoveMaker");

        agent.speed = runSpeed;

        GetNextPoint();

    }

    public void GetDamage(float damage)
    {
        hp -= damage;

        if (hp <= 0) isDead = true;
    }

    public float TakeDamage() { return damageValue; }

    protected void Dead()
    {
        if (isDead) Destroy(this);
    }

    public bool IsInSight
    {
        get { return isInSight; }

        protected set { isInSight = value; }

    }

    public bool IsAttack
    {
        get { return isAttack; }
        protected set { isAttack = value; }
    }
}

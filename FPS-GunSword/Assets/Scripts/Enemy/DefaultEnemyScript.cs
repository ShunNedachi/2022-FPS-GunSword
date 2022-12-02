using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DefaultEnemyScript : MonoBehaviour
{
    // HP
    [SerializeField] protected float hp = 100.0f;
    // damage to Player
    [SerializeField] protected float damageValue = 30.0f;
    // 
    [SerializeField] protected float sightDistance = 100.0f;
    // 
    [SerializeField] protected float walkSpeed = 10.0f;
    // 
    [SerializeField] protected float runSpeed = 30.0f;
    // 
    [SerializeField] protected int stunFrame = 30;
    // 
    [SerializeField] protected float deadTime = 3.0f;
    // 
    [SerializeField] protected float attackStartDistance = 5.0f;
    // for Follow
    [SerializeField] protected GameObject playerObject;
    // angle
    [SerializeField] protected float visualAngle = 70.0f;
    [SerializeField] protected float heightOfVision = 1.0f;
    // 
    [SerializeField] protected GameObject[] makerObj;
    // for Destroy
    [SerializeField] private HealItem heal;

    // state info
    protected enum enemyState
    {
        patrol,
        follow,
        attack,
        step,
        stun,

    }
    protected enemyState state;
    protected bool isDead = false;


    // for Patrol
    protected int destinationIndex = 0;
    protected Rigidbody rigidBody;
    protected NavMeshAgent agent;

    // for attack
    protected bool isAttack = false;
    // for stun
    public bool IsStun { get;set;} = false;
    protected int stunCount = 0;
    protected enemyState beforeStunState;

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
        // movePoint change
        if (!agent.pathPending && agent.remainingDistance < 0.5f) GetNextPoint();
    }

    public void GetNextPoint()
    {
        // maker NULL
        if (makerObj.Length == 0)
        {
            return;
        }

        agent.SetDestination(makerObj[destinationIndex].transform.position);

        // 
        destinationIndex = (destinationIndex + 1) % makerObj.Length;
    }


    // Patrol Move
    public bool MoveWithinSight()
    {

        // sight fixed
        var fixedPosition = transform.position;
        fixedPosition.y += heightOfVision;

        var diff = playerObject.transform.position - fixedPosition;
        var axis = Vector3.Cross(transform.forward, diff);
        var angle = Vector3.Angle(transform.forward, diff) * (axis.y < 0 ? -1 : 1);
        if (angle <= visualAngle && angle >= -visualAngle)
        {
            // ray generate
            RaycastHit hit;
            Vector3 temp = playerObject.transform.position - fixedPosition;
            Vector3 normal = temp.normalized;

            var ray = new Ray(fixedPosition, normal);


            Debug.DrawRay(ray.origin, ray.direction * sightDistance, Color.red);
            if (Physics.Raycast(fixedPosition, normal, out hit, sightDistance))
            {
                // player Hit
                if (hit.transform.gameObject == playerObject)
                {
                    return true;
                }
            }

        }

        // not Player Hit
        return false;
    }


    protected void InitializeEnemy()
    {
        // playerObj
        if (playerObject == null)
        {
            playerObject = GameObject.FindGameObjectWithTag("Player");
        }

        // navMesh agent
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = 0.1f;
        agent.speed = runSpeed;

    }

    public void GetDamage(float damage)
    {
        hp -= damage;

        // dead Flag
        if (hp <= 0) isDead = true;

    }
    // for EnemyAttack
    public float TakeDamage() { return damageValue; }

    protected void Dead()
    {
        if (isDead)
        {   
            // HealObject  Instantiate
            var tempObj = Instantiate(heal, transform.position, transform.rotation);
            tempObj.Awake();

            // deastroy Gameobject
            Destroy(transform.gameObject);
        }
    }

    public bool IsAttack
    {
        get { return isAttack; }
        protected set { isAttack = value; }
    }

    protected void StunCheck()
    {
        if (IsStun)
        {
            // stun前の情報を記録　スタン状態終了時に移行
            beforeStunState = state;
            state = enemyState.stun;
        }
    }
}

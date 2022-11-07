using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// �G�̃x�[�X�ɂȂ�e�N���X
public class DefaultEnemy : MonoBehaviour
{
    // HP
    [SerializeField] protected float hp = 100.0f;
    // �_���[�W��
    [SerializeField] protected float damageValue = 30.0f;
    // ���F����
    [SerializeField] protected float sightDistance = 100.0f;
    // �����̃X�s�[�h
    [SerializeField] protected float walkSpeed = 10.0f;
    // ����̃X�s�[�h
    [SerializeField] protected float runSpeed = 30.0f;
    // ���݂̎���
    [SerializeField] protected float stunTime = 3.0f;
    // �Ƃ肠���������Ă��� ���łɂ����鎞��
    [SerializeField] protected float deadTime = 3.0f;
    // �ǂ͈̔͂Ƀv���C���[����������U�����J�n���邩
    [SerializeField] protected float attackStartDistance = 5.0f;

    // ���E�ɓ������Ƃ��ɒǂ�������Ώ�
    [SerializeField] protected GameObject playerObject;
    // �G���W�c�Ƃ��Ĕ������鋗��
    [SerializeField] protected float groupDistance = 30.0f;
    // �G�̎��p
    [SerializeField] protected float visualAngle = 70.0f;



    // ��{�s���p
    protected int destinationIndex = 0;
    protected GameObject[] navPointsObj;
    protected Rigidbody rigidBody;
    protected NavMeshAgent agent;

    // ���E�ɓ����Ă���̂��@�W�c�����������邽�߂Ɏ�Ɏg�p
    protected bool isInSight = false;
    // �U�������ǂ���
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
        // �G�[�W�F���g�����ڕW�n�_�ɋ߂Â��Ă�����A
        // ���̖ڕW�n�_��I��
        if (!agent.pathPending && agent.remainingDistance < 0.5f)GetNextPoint();
    }

    public void GetNextPoint()
    {
        if (navPointsObj.Length == 0)
        {
            return;
        }

        agent.SetDestination(navPointsObj[destinationIndex].transform.position);

        //�z��̃C���f�b�N�X��+1���āA�Ō�̒n�_�������ꍇ��0�ɖ߂�
        destinationIndex = (destinationIndex + 1) % navPointsObj.Length;
    }


    // ���E�Ƀv���C���[�������Ă��邩�ǂ���
    public bool MoveWithinSight()
    {

        // ���E�ɃI�u�W�F�N�g�����邩����
        var diff = playerObject.transform.position - transform.position;
        var axis = Vector3.Cross(transform.forward, diff);
        var angle = Vector3.Angle(transform.forward, diff) * (axis.y < 0 ? -1 : 1); 
        if (angle <= visualAngle && angle >= -visualAngle)
        {
            // ���E�ɃI�u�W�F�N�g������΃��C���΂�
            RaycastHit hit;
            Vector3 temp = playerObject.transform.position - transform.position;
            Vector3 normal = temp.normalized;

            if (Physics.Raycast(transform.position, normal, out hit, sightDistance))
            {
                // player�����E���ɂ����Ƃ�
                if(hit.transform.gameObject == playerObject)
                {
                    return true;
                }
            }

        }

        // ���E����player�����Ȃ��Ƃ�false
        return false;
    }

    // �e�N���X�̏������������q�N���X�ŗ��p���邽�߂ɍ쐬
    public void InitializeEnemy()
    {
        // �G���[���Ȃ������߂̂���
        stunTime = 3.0f;
        deadTime = 3.0f;

        // �������������Ă��Ȃ������Ƃ���player�^�O������
        if (playerObject == null)
        {
            playerObject = GameObject.FindGameObjectWithTag("Player");
        }

        // ��{�s���p
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

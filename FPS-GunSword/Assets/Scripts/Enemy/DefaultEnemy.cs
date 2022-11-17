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
    [SerializeField] protected int stunFrame = 30;
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

    [SerializeField] protected float heightOfVision = 1.0f;

    // �g�p����}�[�J�[
    [SerializeField] protected GameObject[] makerObj;

    [SerializeField] private HealItem heal;

    // ��{�s���p
    protected int destinationIndex = 0;
    protected Rigidbody rigidBody;
    protected NavMeshAgent agent;

    // ���E�ɓ����Ă���̂��@�W�c�����������邽�߂Ɏ�Ɏg�p
    protected bool isInSight = false;
    // �U�������ǂ���
    protected bool isAttack = false;
    protected bool isDead = false;

    public bool IsStun {get;set;} = false;
    protected int stunCount = 0;
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
        if (makerObj.Length == 0)
        {
            return;
        }

        agent.SetDestination(makerObj[destinationIndex].transform.position);

        //�z��̃C���f�b�N�X��+1���āA�Ō�̒n�_�������ꍇ��0�ɖ߂�
        destinationIndex = (destinationIndex + 1) % makerObj.Length;
    }


    // ���E�Ƀv���C���[�������Ă��邩�ǂ���
    public bool MoveWithinSight()
    {

        // ���E�ɃI�u�W�F�N�g�����邩���� �I�u�W�F�N�g�̖ڂ̈ʒu���l�����邽�߂�1����y���Ƀv���X���Ă���
        var fixedPosition = transform.position;
        fixedPosition.y += heightOfVision;

        var diff = playerObject.transform.position - fixedPosition;
        var axis = Vector3.Cross(transform.forward, diff);
        var angle = Vector3.Angle(transform.forward, diff) * (axis.y < 0 ? -1 : 1); 
        if (angle <= visualAngle && angle >= -visualAngle)
        {
            // ���E�ɃI�u�W�F�N�g������΃��C���΂�
            RaycastHit hit;
            Vector3 temp = playerObject.transform.position - fixedPosition;
            Vector3 normal = temp.normalized;

            var ray = new Ray(fixedPosition, normal);


            Debug.DrawRay(ray.origin, ray.direction * sightDistance, Color.red);
            if (Physics.Raycast(fixedPosition, normal, out hit, sightDistance))
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
        deadTime = 3.0f;

        // �������������Ă��Ȃ������Ƃ���player�^�O������
        if (playerObject == null)
        {
            playerObject = GameObject.FindGameObjectWithTag("Player");
        }

        // ��{�s���p
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = 0.1f;


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
        if (isDead)
        {
            var tempObj = Instantiate(heal, transform.position, transform.rotation);
            tempObj.Awake();
            //heal.SetPosition(transform.position);
            Destroy(transform.gameObject); 
        }
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

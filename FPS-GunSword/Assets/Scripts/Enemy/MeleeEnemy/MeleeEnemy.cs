using System;
using UnityEngine.AI;
using UnityEngine;

public class MeleeEnemy : DefaultEnemy
{
    // •à‚«‚É•Ï‚í‚é‹——£
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
        // ‰Šú‰»ˆ—
        InitializeEnemy();

        // ©g‚Ìü•Ó‚É‹ß‹——£ƒGƒlƒ~[‚ğ3,4•¦‚©‚¹‚é
        System.Random rand = new System.Random();
        int spawnRandNum = rand.Next(3, 5);

        // ¶¬ˆ—‚ÍŒã‚Ù‚Ç’Ç‰Á
        for (int i = 0; i < spawnRandNum; i++)
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        // ‹ŠE“à‚Éplayer‚ª‚¢‚È‚¯‚ê‚Îƒ}[ƒJ[‚É‰ˆ‚Á‚ÄˆÚ“®
        if (!MoveWithinSight())
        {
            MoveRandom();

            agent.isStopped = false;
        }
        else
        {
            // ‹ŠE‚É“ü‚Á‚½‚çplayer‚ÌˆÊ’u‚ğ’Ç‚¢‚©‚¯‚é Œã‚ÅUŒ‚‚ÍXV‚µ‚È‚¢‚æ‚¤‚ÉC³
            if (!isAttack) 
            {
                agent.SetDestination(playerObject.transform.position);
            }

            // ˆê’è‹——£ˆÈã‹ß‚¯‚ê‚Î•à‚­
            var distanceToPlayer = Vector3.Distance(playerObject.transform.position, transform.position);
            if(distanceToPlayer <= walkDistance)
            {
                isWalk = true;

                // UŒ‚‚Å‚«‚éˆÊ’u‚Ü‚Å‹ß‚Ã‚¢‚½‚çUŒ‚s“®ŠJn
                if(distanceToPlayer <= attackStartDistance && !isAttackInterval)
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

        // 
        if(isAttack)
        {
            // Šî‘bs“®‚ğ~‚ß‚é
            agent.isStopped = true;
            agent.SetDestination(transform.position);


            countAttack++;
            if (countAttack > attackFrame)
            {
                isAttack = false;
                countAttack = 0;
                isAttackInterval = true;

                attackActive = true;
            }
        }

        // UŒ‚‚Ìd’¼’†
        if(isAttackInterval)
        {
            countAttackInterval++;

            if (countAttackInterval > attackIntervalFrame)
            {
                isAttackInterval = false;
                countAttackInterval = 0;

            }

        }

        // UŒ‚‚Ì‘±
        if (attackActive)
        {
            countAttackActive++;

            if(countAttackActive > attackActiveFrame)
            {
                countAttackActive = 0;

                attackActive = false;

                agent.isStopped = false;
            }
        }

        // €–Sˆ—
        Dead();
    }

    public bool AttackActive
    {
        get { return attackActive; }

        set { attackActive = value; }
    }
}

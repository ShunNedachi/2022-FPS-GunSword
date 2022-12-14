using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefaultMove : MonoBehaviour
{
    public static PlayerDefaultMove instance;

    [SerializeField] public float moveSpeed = 0.05f;
    [SerializeField] public float dashSpeed = 0.5f;
    [SerializeField] public int recastInterval = 180;
    [SerializeField] public int dashInterval = 60;
    [SerializeField] public AudioClip sound;
    [SerializeField] public new GameObject camera;
    
    private int recastTimer = 0;
    private int dashTimer = 0;
    private int dashEnergy = 50;
    private bool dashMode = false;
    float defaultY;
    float moveX;
    float moveZ;
    bool isMove = true;
    //Rigidbody rb;
    CharacterController controller;

    AudioSource audioSource;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    // Start is called before the first frame update
    public void Start()
    {
        PlayerStaminaScript.instance.Start();
        //rb = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
        dashTimer = dashInterval;
        defaultY = this.gameObject.transform.position.y;
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    public void Update()
    {
        if(isMove)
        {
            if(dashMode == false && recastTimer > recastInterval)
            {
                if(Input.GetKeyDown(KeyCode.LeftShift) && PlayerStaminaScript.instance.GetStamina() > dashEnergy)
                {
                    dashMode = true;
                    PlayerStaminaScript.instance.Dash();
                    audioSource.PlayOneShot(sound);

                }
            }
            Transform trans = transform;
            transform.position = trans.position;

            if(dashMode)
            {
                moveX = Input.GetAxisRaw("Vertical") * dashSpeed;
                moveZ = Input.GetAxisRaw("Horizontal") * dashSpeed;
                Vector3 direction = camera.transform.forward * moveX + camera.transform.right * moveZ;                
                controller.SimpleMove (direction);
                dashTimer++;

                if(dashTimer>dashInterval)
                {
                    dashMode = false;
                    dashTimer = 0;
                }
            }
            else
            {
                moveX = Input.GetAxisRaw("Vertical") * moveSpeed;
                moveZ = Input.GetAxisRaw("Horizontal") * moveSpeed;
                Vector3 direction = camera.transform.forward * moveX + camera.transform.right * moveZ;                
                controller.SimpleMove(direction);
                recastTimer++;
                if(recastTimer>recastInterval)
                {
                    PlayerStaminaScript.instance.Recharge();
                }
            }
            TPSControl.instance.Move(moveX,moveZ);
        }
    }
    public bool GetDashMode()
    {
        return dashMode;
    }
    public float GetSpeed()
    {
        if(dashMode)
        {
            return dashSpeed;
        }
        return moveSpeed;
    }

    public void SetMove(bool chack)
    {
        isMove = chack;
    }
    public bool GetIsMove()
    {
        return isMove;
    }
}

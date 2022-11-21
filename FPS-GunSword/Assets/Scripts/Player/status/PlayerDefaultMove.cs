using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefaultMove : MonoBehaviour
{
    public static PlayerDefaultMove instance;

    [SerializeField] public float moveSpeed = 0.05f;
    [SerializeField] public float dashSpeed = 0.5f;
    [SerializeField] public int recastInterval = 180;
    [SerializeField] public int dashInterval = 20;
    
    private int recastTimer = 0;
    private int dashTimer = 0;
    private int dashEnergy = 50;
    private bool dashMode = false;
    float vertical;
    float horizontal;
    float defaultY;
    float moveX;
    float moveZ;
    //Rigidbody rb;
    CharacterController controller;
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
    }

    // Update is called once per frame
    public void Update()
    {
        if(dashMode == false && recastTimer > recastInterval)
        {
            if(Input.GetKeyDown(KeyCode.LeftShift) && PlayerStaminaScript.instance.GetStamina() > dashEnergy)
            {
                vertical = Input.GetAxis("Vertical");
                horizontal = Input.GetAxis("Horizontal");
                dashMode = true;
                PlayerStaminaScript.instance.Dash();
            }
        }
        Transform trans = transform;
        transform.position = trans.position;

        if(dashMode)
        {
            moveX = vertical * dashSpeed;
            moveZ = horizontal * dashSpeed;
            Vector3 direction = new Vector3(moveX,0,moveZ);
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
            moveX = Input.GetAxis("Vertical") * moveSpeed;
            moveZ = Input.GetAxis("Horizontal") * moveSpeed;
            Vector3 direction = new Vector3(moveX,0,moveZ);
            transform.position = new Vector3(moveX,3,moveZ);
            controller.SimpleMove(direction);

            recastTimer++;
            if(recastTimer>recastInterval)
            {
                PlayerStaminaScript.instance.Recharge();
            }
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
}

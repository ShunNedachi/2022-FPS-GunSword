using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefaultMove : MonoBehaviour
{
    public static PlayerDefaultMove instance;

    [SerializeField] public float moveSpeed = 0.5f;
    [SerializeField] public float dashSpeed = 1.0f;
    [SerializeField] public int recastInterval = 180;
    [SerializeField] public int dashInterval = 20;
    
    private int recastTimer = 0;
    private int dashTimer = 0;
    private int dashEnergy = 50;
    private bool dashMode = false;
    float vertical;
    float horizontal;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
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
            trans.position += trans.TransformDirection(Vector3.forward) * vertical * moveSpeed;
            trans.position += trans.TransformDirection(Vector3.right) * horizontal * moveSpeed;

            dashTimer++;
            
            if(dashTimer>dashInterval)
            {
                dashMode = false;
                dashTimer = 0;
            }
        }
        else
        {
            trans.position += trans.TransformDirection(Vector3.forward) * Input.GetAxis("Vertical") * moveSpeed;
            trans.position += trans.TransformDirection(Vector3.right) * Input.GetAxis("Horizontal") * moveSpeed;

            recastTimer++;
            if(recastTimer>recastInterval)
            {
                PlayerStaminaScript.instance.Recharge();
            }
        }
    }
}

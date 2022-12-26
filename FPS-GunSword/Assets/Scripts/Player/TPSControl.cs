using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSControl : MonoBehaviour
{
    public static TPSControl instance;
    Animator animator;

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
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetActivity(bool activity)
    {
        this.gameObject.SetActive(activity);
    }
    public void TPS_Taiki()
    {
        animator.SetInteger("pram",0);
    }
    public void Reload()
    {
        animator.SetInteger("pram",1);
    }
    public void Trans()
    {
        animator.SetBool("ULT",true);
    }
    public void Move(float moveX,float moveZ)
    {
        animator.SetFloat("MoveX",moveX);
        animator.SetFloat("MoveZ",moveZ);
    }
}

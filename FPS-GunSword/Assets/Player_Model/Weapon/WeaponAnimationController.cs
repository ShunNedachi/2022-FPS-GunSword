using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimationController : MonoBehaviour
{
    public enum State
    {
       Idle,
       Reroad_Open,
       Reroad_Close,
       Trans_Open,
       TransClose
    }

    [SerializeField] State animationstate;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        this.animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        this.animator.SetInteger("Param", (int)animationstate);

        if (Input.GetMouseButton(0))
        {
            animationstate++;

            if((int)animationstate > 4)
            {
                animationstate = 0;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public enum State
    {
        Idle,
        ForwordRun,
        BackRun,
        RightRun,
        leftRun,
        DownSlash,
        UpSlash
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
        this.animator.SetInteger("AnimParam", (int)animationstate);
    }
}

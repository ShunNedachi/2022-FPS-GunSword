using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 敵のベースになる親クラス
public class DefaultEnemy : MonoBehaviour
{
    // 視認距離
    [SerializeField] private float sightDistance = 100.0f;
    // 歩きのスピード
    [SerializeField] private float walkSpeed = 10.0f;
    // 走りのスピード
    [SerializeField] private float runSpeed = 30.0f;
    // 怯みの時間
    [SerializeField] private float stunTime = 3.0f;
    // とりあえず持っておく 消滅にかかる時間
    [SerializeField] private float deadTime = 3.0f; 

    // Start is called before the first frame update
    void Start()
    {
        // エラーをなくすためのもの
        sightDistance = 100.0f;
        walkSpeed = 10.0f;
        runSpeed = 30.0f;
        stunTime = 3.0f;
        deadTime = 3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

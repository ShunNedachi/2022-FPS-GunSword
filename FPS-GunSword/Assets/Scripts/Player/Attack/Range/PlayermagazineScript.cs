using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagazineScript : MonoBehaviour
{
    public static PlayerMagazineScript instance;

    [SerializeField] private int magazineMax;

    private int remainingBullets;
    
    public void Awake()
    {
        if(instance ==null)
        {
            instance = this;
        }
    }

    public void Start()
    {
        remainingBullets = magazineMax;
    }
    //残弾数を取得
    public int GetRemainingBullets()
    {
        return remainingBullets;
    }
    //撃った時に残弾を減らす
    public void Shot()
    {
        remainingBullets--;
    }
    //リロード
    public void Reload()
    {
        remainingBullets = magazineMax;
    }
}

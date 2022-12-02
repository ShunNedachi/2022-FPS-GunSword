using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreScript : MonoBehaviour
{
    public int hp=100;
    public int meleeDamage=100;
    public int rangeDagame=10;
    public static int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        count++;
    }

    // Update is called once per frame
    void Update()
    {
        if(hp<0)
        {
            Break();
        }
    }
    public void Break()
    {
        count--;
        Destroy(this.gameObject);
    }

    public void MeleeHit()
    {
        hp-=meleeDamage;
    }
    public void RangeHit()
    {
        hp -= rangeDagame;
    }

}

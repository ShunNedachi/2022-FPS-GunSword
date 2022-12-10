using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreScript : MonoBehaviour
{
    public static CoreScript instance;

    public int hp=100;
    public int meleeDamage=100;
    public int rangeDagame=10;
    public int keyNum = 0;
    public static int coreCount0 = 0;
    public static int coreCount1 = 0;
    public static int coreCount2 = 0;

    public void Awake()
    {
        if(instance ==null)
        {
            instance = this;
        }
    }


    // Start is called before the first frame update
    void Start()
    { 
        if(keyNum==0)
        {
            coreCount0++; 
        }
        if(keyNum==1)
        {
            coreCount1++;
        }
        if(keyNum==2)
        {
            coreCount2++;
        }
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
        if (keyNum == 0)
        {
            coreCount0--;
        }
        if (keyNum == 1)
        {
            coreCount1--;
        }
        if (keyNum == 2)
        {
            coreCount2--;
        }
        Destroy(this.gameObject);
    }

    public void MeleeHit()
    {
        hp-=meleeDamage;
    }
    public void RangeHit()
    {
        hp -= rangeDagame;
        Debug.Log(hp);
    }

}

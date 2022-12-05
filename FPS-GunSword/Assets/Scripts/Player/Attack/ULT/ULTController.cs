using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ULTController : MonoBehaviour
{
    public static ULTController instance;
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
        PlayerULTScript.instance.Start();
        PlayerExcaliburScript.instance.Start();
        PlayerEnergyScript.instance.Start();
    }

    // Update is called once per frame
    public void Update()
    {

        PlayerULTScript.instance.Update();
        PlayerExcaliburScript.instance.Update();

        PlayerEnergyScript.instance.EnemyConsumption();
        if(PlayerEnergyScript.instance.GetEnergy()<0)
        {
            //ULT終了処理
            PlayerEnergyScript.instance.SetULTchack(false);
            PlayerScript.instance.SetULTchack(false);

        }

    }
}

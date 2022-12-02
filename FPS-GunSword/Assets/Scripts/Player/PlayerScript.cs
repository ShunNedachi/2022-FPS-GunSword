using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public static PlayerScript instance;

    [SerializeField] public int deformationInterval = 30;

    private bool ULT = false;
    private int deformationTimer = 0;
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
        PlayerShotScript.instance.Start();
        PlayerSlashScript.instance.Start();
        PlayerULTScript.instance.Start();
        PlayerDefaultMove.instance.Start();
        PlayerHPScript.instance.Start();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerEnergyScript.instance.Update();
        PlayerStaminaScript.instance.Update();
        PlayerMagazineScript.instance.Update();

        if(Input.GetKeyDown(KeyCode.F) && PlayerEnergyScript.instance.GetULTchack())
        {
            ULT = true;
            CameraController.instance.ChangeThirdViewCamera();

        }
        PlayerDefaultMove.instance.Update();

        if(ULT)
        {
            deformationTimer++;
            if(deformationTimer>deformationInterval)
            {
                //ULT中処理
                PlayerULTScript.instance.Update();

            }
        }
        else
        {
            CameraController.instance.ChangeMainCamera();
            PlayerShotScript.instance.Update();
            PlayerSlashScript.instance.Update();
        } 
    }
    public void SetULTchack(bool chack)
    {
        ULT = chack;
    }

}

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
    public Image gaugeWeaponImage;
    public Sprite[] gaugeWeaponSprite;
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
        PlayerDefaultMove.instance.Start();
        PlayerHPScript.instance.Start();
        ULTController.instance.Start();
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
            gaugeWeaponImage.sprite = gaugeWeaponSprite[0];
            deformationTimer++;
            if(deformationTimer>deformationInterval)
            {
                //ULT中処理
                ULTController.instance.Update();
            }
        }
        else
        {
            gaugeWeaponImage.sprite = gaugeWeaponSprite[1];
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

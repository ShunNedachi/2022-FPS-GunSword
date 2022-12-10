using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public static PlayerScript instance;

    [SerializeField] private int deformationInterval = 30;
    [SerializeField] private GameObject canvas;
    [SerializeField] public AudioClip ULTStart;


    private bool ULT = false;
    private int deformationTimer = 0;
    public Image gaugeWeaponImage;
    public Sprite[] gaugeWeaponSprite;
    AudioSource audioSource;

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
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        PlayerEnergyScript.instance.Update();
        PlayerStaminaScript.instance.Update();
        PlayerMagazineScript.instance.Update();

        if(Input.GetKeyDown(KeyCode.F) && PlayerEnergyScript.instance.GetULTchack() && ULT == false)
        {
            ULT = true;
            CameraController.instance.ChangeThirdViewCamera();
            canvas.SetActive(false);
            audioSource.PlayOneShot(ULTStart);
            BayonetMove.instance.SetActivity(false);
            TPSControl.instance.SetActivity(true);

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
            canvas.SetActive(true);
            BayonetMove.instance.SetActivity(true);
            TPSControl.instance.SetActivity(false);
        } 
    }
    public void SetULTchack(bool chack)
    {
        ULT = chack;
    }

}

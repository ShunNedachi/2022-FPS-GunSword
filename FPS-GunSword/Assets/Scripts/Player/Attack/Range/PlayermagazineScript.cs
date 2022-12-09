using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMagazineScript : MonoBehaviour
{
    public static PlayerMagazineScript instance;

    [SerializeField] private int magazineMax = 6;

    private int remainingBullets;

    public Image MagazineImage;
    public Sprite[] Magazine;
    public Text TextFrame;
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
    public void Update()
    {
        Draw();
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
    public void Draw()
    {
        TextFrame.text = string.Format("{0}/6", remainingBullets);
        switch (remainingBullets)
        {
            default:
               MagazineImage.sprite = Magazine[0];
                //Debug.Log("0");
                break;
            case 1:
                MagazineImage.sprite = Magazine[1];
                //Debug.Log("1");
                break;
            case 2:
                MagazineImage.sprite = Magazine[2];
                //Debug.Log("2");
                break;
            case 3:
                MagazineImage.sprite = Magazine[3];
                //Debug.Log("3");
                break;
            case 4:
                MagazineImage.sprite = Magazine[4];
                //Debug.Log("4");
                break;
            case 5:
                MagazineImage.sprite = Magazine[5];
                //Debug.Log("5");
                break;
            case 6:
                MagazineImage.sprite = Magazine[6];
                //Debug.Log("6");
                break;

        }

    }

}

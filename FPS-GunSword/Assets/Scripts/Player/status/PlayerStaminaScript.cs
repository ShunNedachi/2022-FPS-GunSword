using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerStaminaScript : MonoBehaviour
{
    public static PlayerStaminaScript instance;

    [SerializeField] private int staminaMax = 100;
    public Image staminaImage;
    public Sprite[] staminaSprite;
    private int stamina = 0;
    public int dashStamina = 50;

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
        stamina = staminaMax;
    }
    public void Update()
    {
        Draw();
    }
    public void Recharge()
    {
        if(stamina<staminaMax)
        {
            stamina++;
        }
    }
    public void Dash()
    {
        stamina -= dashStamina;
    }

    public int GetStamina()
    {
        return stamina;
    }
    public void Draw()
    {
        switch (stamina/dashStamina)
        {
            default:
                staminaImage.sprite = staminaSprite[0];
                break;
            case 1:
                staminaImage.sprite = staminaSprite[1];
                break;
            case 2:
                staminaImage.sprite = staminaSprite[2];
                break;
        }

    }
}

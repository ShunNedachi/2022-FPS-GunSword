using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStaminaScript : MonoBehaviour
{
    public static PlayerStaminaScript instance;

    [SerializeField] private int staminaMax = 100;

    private int stamina = 0;

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

    public void Recharge()
    {
        if(stamina<staminaMax)
        {
            stamina++;
        }
    }
    public void Dash()
    {
        stamina -= 50;
    }

    public int GetStamina()
    {
        return stamina;
    }
}

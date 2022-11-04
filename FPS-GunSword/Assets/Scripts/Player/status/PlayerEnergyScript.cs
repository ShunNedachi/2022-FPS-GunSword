using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergyScript : MonoBehaviour
{
    public static PlayerEnergyScript instance;

    [SerializeField] private int energyMax;
    private float energy = 0;
    private int energyItem = 60;
    private int baceEnergyAbsorption = 25;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetEnergyItem()
    {
        if(energy<energyMax)
        {
            energy += energyItem;
        }
    }

    public void SlashChargeEnergy()
    {
        if(PlayerSlashScript.instance.GetComboCount() == 0)
        {
            energy += baceEnergyAbsorption;
        }
        else
        {
            energy = energy + PlayerSlashScript.instance.GetComboCount() * baceEnergyAbsorption * 1.1f;
        }
    }
}

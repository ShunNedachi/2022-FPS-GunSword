using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergyScript : MonoBehaviour
{
    public static PlayerEnergyScript instance;
    public Image gaugeImage;
    public Sprite[] gauge;
    [SerializeField] private float energyMax = 1000;
    [SerializeField] private int energySlash = 60;
    [SerializeField] private int energyDecrease = 1;

    private float energy = 0;
    private float energyItem = 100;
    private int baceEnergyAbsorption = 25;
    private bool ULTchack = false;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
       
        gaugeImage.enabled = true;
    }

    // Update is called once per frame
    public void Update()
    {
        if(energy>energyMax)
        {
            ULTchack = true;
        }
        energyGaugeUpdate();
    }

    public float GetEnergy()
    {
        return energy;
    }
    public bool GetULTchack()
    {
        return ULTchack;
    }
    public void SetULTchack(bool chack)
    {
        ULTchack = chack;
    }
    public void SetEnergyItem()
    {
        
        if (energy<energyMax)
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
        if(energy>=energyMax)
        {
            energy = energyMax;
        }
    }
    public void EnemyConsumptionSlash()
    {
        energy -= energySlash;
    }
    public void EnemyConsumption()
    {
        energy -= energyDecrease;
    }
    public void energyGaugeUpdate()
    {
        switch (energy / energyItem)
        {
            default:

                gaugeImage.sprite = gauge[0];
                break;
            case 10:
                gaugeImage.sprite = gauge[10];
                break;
            case 9:
                gaugeImage.sprite = gauge[9];
                break;
            case 8:
                gaugeImage.sprite = gauge[8];
                break;
            case 7:
                gaugeImage.sprite = gauge[7];
                break;
            case 6:
                gaugeImage.sprite = gauge[6];
                break;
            case 5:
                gaugeImage.sprite = gauge[5];
                break;
            case 4:
                gaugeImage.sprite = gauge[4];
                break;
            case 3:
                gaugeImage.sprite = gauge[3];
                break;
            case 2:
                gaugeImage.sprite = gauge[2];
                break;
            case 1:
                gaugeImage.sprite = gauge[1];

                break;
  

        }

    }
}

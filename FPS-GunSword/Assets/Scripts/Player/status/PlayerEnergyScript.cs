using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergyScript : MonoBehaviour
{
    public static PlayerEnergyScript instance;

    [SerializeField] private int energyMax = 1000;
    [SerializeField] private int energySlash = 60;
    [SerializeField] private int energyDecrease = 1;

    private float energy = 0;
    private int energyItem = 60;
    private int baceEnergyAbsorption = 25;
    private bool ULTchack = false;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    public void Update()
    {
        if(energy>energyMax)
        {
            ULTchack = true;
        }
        
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
}

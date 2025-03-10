using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Diagnostics;

public class HealthButton : MonoBehaviour
{
    public GameObject HealthPotion;
    public TextMeshProUGUI PotionAmount;
    public int healthPotionAmount;
    [Space]
    public GameObject EnergyPotion;
    public TextMeshProUGUI PotionAmount2;
    public int energyPotionAmount; 
    
    void Start()
    {
        PotionAmount.text = "X" + healthPotionAmount.ToString();
        PotionAmount2.text = "X" + energyPotionAmount.ToString();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Switch();
        }
    }
    
    public void UseHealthPotion()
    {
        if (healthPotionAmount > 0)
        {
            playermovement.playerstats.Heal(50);
            healthPotionAmount --; 
            PotionAmount.text = "X" + healthPotionAmount.ToString();
        }
    }
    public void UseEnergyPotion()
    {
        if (energyPotionAmount > 0)
        {
            if (playermovement.playerstats.Energy != playermovement.playerstats.MaxEnergy)
            {
                playermovement.playerstats.Energy += 10;
                energyPotionAmount --;
                PotionAmount2.text = "X" + energyPotionAmount.ToString();
            }
        }
        
    }

    void Switch()
    {
        if (HealthPotion.activeInHierarchy)
        {
            EnergyPotion.SetActive(true);
            HealthPotion.SetActive(false);
        }
        else{
            HealthPotion.SetActive(true);
            EnergyPotion.SetActive(false);
        }
    }
}

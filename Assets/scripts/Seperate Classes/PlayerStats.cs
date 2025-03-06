using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStats
{
    public static PlayerStats instance;

    //Classes
    bool Knight;
    bool Archer;
    bool Mage;
    bool Warrior;
    
    //Health
    public float MaxHealth;
    public int MaxHealthPoints;
    public float Health;

    //Speed
    public float Speed;
    public float SpeedPoints;

    //Damage
    public float AttackPower;
    public float AttackPowerPoints;
    public float Defense;
    public float DefensePoints;

    //Heavy Attack
    public int MaxEnergy;
    public float Energy;

    //Upgrading
    public int SkillPoints;

    private void Awake() {
        instance = this;
    }
    public void GetHurt(float Damage)
    {
        Defense = Defense + DefensePoints*1.1f;
        float FinalDamage = (float)Math.Round(Mathf.Max(10*(Damage*Damage)/10*(Damage+Defense), 0),2);
        Health -= FinalDamage;
    }

    public void Heal(float Amount)
    {
        Health += Amount;
        if (Health > MaxHealth)
        {
            Health = MaxHealth;
        }
    }
    public void SetStats () {

        if (Knight)
        {
            AttackPower = AttackPower + AttackPowerPoints * 1.1F;
            Speed = Speed + SpeedPoints * 1.1F;
            Defense = Defense + DefensePoints * 1.1F;
            MaxHealth = MaxHealth + MaxHealthPoints * 1.1F;
        }
        else if (Mage)
        {
            AttackPower = AttackPower + AttackPowerPoints * 1.1F;
            Speed = Speed + SpeedPoints * 1.1F;
            Defense = Defense + DefensePoints * 1.1F;
            MaxHealth = MaxHealth + MaxHealthPoints * 1.1F;
        }
        else if (Warrior)
        {
            AttackPower = AttackPower + AttackPowerPoints * 1.1F;
            Speed = Speed + SpeedPoints * 1.1F;
            Defense = Defense + DefensePoints * 1.1F;
            MaxHealth = MaxHealth + MaxHealthPoints * 1.1F;
        }
        else if (Archer)
        {
            AttackPower = AttackPower + AttackPowerPoints * 1.1F;
            Speed = Speed + SpeedPoints * 1.1F;
            Defense = Defense + DefensePoints * 1.1F;
            MaxHealth = MaxHealth + MaxHealthPoints * 1.1F;
        }
        else{
            AttackPower = AttackPower + AttackPowerPoints * 1.1F;
            Speed = Speed + SpeedPoints * 1.1F;
            Defense = Defense + DefensePoints * 1.1F;
            MaxHealth = MaxHealth + MaxHealthPoints * 1.1F;
        }
        
    }

    public void Upgrade(int Type)
    {
        if (SkillPoints > 0)
        {
            if (Type == 1)
            {
                AttackPower += 1;
                Debug.Log("Upgrade Point assigned to:" + AttackPower);
            }
            else if (Type == 2)
            {
                SpeedPoints += 1;
                Debug.Log("Upgrade Point assigned to:" + Speed);
            }
            else if(Type == 3) 
            { 
                DefensePoints += 1;
                Debug.Log("Upgrade Point assigned to:" + Defense);
            } 
            else if(Type == 4) 
            {
                MaxHealthPoints += 1; 
                Debug.Log("Upgrade Point assigned to:" + MaxHealth);
            } 
            SkillPoints--;
            SetStats();
        }
        else
        {
            Debug.Log("No Skill Points");
        }
    }

    public void SaveStats()
    {
        PlayerPrefs.SetInt("PlayerSkillPoints", SkillPoints);
        PlayerPrefs.SetFloat("PlayerHealth", Health);
        PlayerPrefs.SetFloat("PlayerSpeed", SpeedPoints);
        PlayerPrefs.SetInt("PlayerMaxHealthPoints", MaxHealthPoints);
        PlayerPrefs.SetFloat("PlayerAttackPowerPoints", AttackPowerPoints);
        PlayerPrefs.SetFloat("PlayerDefensePoints", DefensePoints);
        PlayerPrefs.Save();
        Debug.Log("Stats Saved");
    }

    public void LoadStats()
    {
        SkillPoints = PlayerPrefs.GetInt("PlayerSkillPoints");
        Health = PlayerPrefs.GetFloat("PlayerHealth");
        MaxHealthPoints = PlayerPrefs.GetInt("PlayerMaxHealthPoints");
        AttackPowerPoints = PlayerPrefs.GetFloat("PlayerAttackPowerPoints");
        DefensePoints = PlayerPrefs.GetFloat("PlayerDefensePoints");
        Debug.Log("Stats Loaded");
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStats 
{
    public static PlayerStats instance;

    //Classes
    readonly bool Knight;
    readonly bool Archer;
    readonly bool Mage;
    readonly bool Warrior;
    
    //Health
    public float MaxHealth ;
    public float MaxHealthPoints;
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
    public int MaxEnergy = 10;
    public float Energy;

    //Upgrading
    public int SkillPoints;

    //Score
    public int Score;
    public int HighScore;

    private void Awake() {
        instance = this;
    }

    public void Heal(float Amount)
    {
        Health += Amount;
        if (Health > MaxHealth)
        {
            Health = MaxHealth;
        }
    }
    public void SetPoints(float AttackPoints, float DefensePoints, float SpeedPoints,float MaxHealthPoints)
    {
        this.AttackPowerPoints = AttackPoints;
        this.DefensePoints = DefensePoints;
        this.SpeedPoints = SpeedPoints;
        this.MaxHealthPoints = MaxHealthPoints;
        SetStats();
        

    }

    public float CalculateDamage(float damage, float defense)
    {
        return Mathf.Max((10 * damage * damage) / (10 * (damage + defense)), 1);
    }

    public void SetStats () {

        if (Knight)
        {
            AttackPower = AttackPower + AttackPowerPoints * 5F;
            Speed = Speed + SpeedPoints * 1;
            Defense = Defense + DefensePoints * 4F;
            MaxHealth = MaxHealth + MaxHealthPoints * 200F;
        }
        else if (Mage)
        {
            AttackPower = AttackPower + AttackPowerPoints * 7F;
            Speed = Speed + SpeedPoints * .5F;
            Defense = DefensePoints * 1.34F;
            MaxHealth = MaxHealth + MaxHealthPoints * 120F;     
        }
        else if (Warrior)
        {
            AttackPower = AttackPower + AttackPowerPoints * 4F;
            Speed = Speed + SpeedPoints * 0.3F;
            Defense = Defense + DefensePoints * 6F;
            MaxHealth = MaxHealth + MaxHealthPoints * 250F;
        }
        else if (Archer)
        {
            AttackPower = AttackPower + AttackPowerPoints * 6F;
            Speed = Speed + SpeedPoints * 1.2F;
            Defense = Defense + DefensePoints * 1.2F;
            MaxHealth = MaxHealth + MaxHealthPoints * 100F;
        }
        else
        {
            AttackPower = AttackPower + (AttackPowerPoints * 2F);
            Speed = Speed + (SpeedPoints * 0.6F);
            Defense = Defense + (DefensePoints * 3F);
            MaxHealth = MaxHealth + (MaxHealthPoints * 150F);
            Health = MaxHealth;
            Debug.Log(this.AttackPower);
            Debug.Log(this.Health);
            Debug.Log(this.MaxHealth);
            Debug.Log(this.Defense);
            Debug.Log(this.Speed);
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
        PlayerPrefs.SetFloat("PlayerMaxHealthPoints", MaxHealthPoints);
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
    public void HighScoreCheck()
    {
        if (Score > HighScore)
        {
            this.HighScore = Score;
            Debug.Log("New HoghScore");
        }
        else
        {
            Debug.Log("So Close!Better Luck Next Time.");
        }
    }
    
}

using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


public class EnemyStats
{
    //EnemyHealth
    public int EnemyMaxHealth = 200;
    public float EnemyHealth = 200;

    //Movement
    public float EnemySpeed = 3;

    //Damage
    public float EnemyAttackPower;
    public float EnemyDefense = 5;

    //Type
    public string[] Type;
    
    public float CalculateDamage(float damage, float defense)
    {
        return Mathf.Max((10 * damage * damage) / (10 * (damage + defense)), 1);
    }
    
    public void EnemyType(int SeletedType,int MaxHealthValue, float SpeedValue,float AttackPowerValue,float DefenseValue){

        Type = new string[] {"Speed", "Heavy", "Tank", "Ranged", "MagicWielder"};

        if (Type[SeletedType] == "Speed")
        {
            EnemyMaxHealth = MaxHealthValue;
            EnemySpeed = SpeedValue;
            EnemyAttackPower = AttackPowerValue;
            EnemyDefense = DefenseValue;
        }
        else if(Type[SeletedType] == "Heavy") 
        {
            EnemyMaxHealth = MaxHealthValue;
            EnemySpeed = SpeedValue;
            EnemyAttackPower = AttackPowerValue;
            EnemyDefense = DefenseValue;
        } 
        else if(Type[SeletedType] == "Tank") 
        {
            EnemyMaxHealth = MaxHealthValue;
            EnemySpeed = SpeedValue;
            EnemyAttackPower = AttackPowerValue;
            EnemyDefense = DefenseValue;
        } 
        else if(Type[SeletedType] == "Ranged")
        {
            EnemyMaxHealth = MaxHealthValue;
            EnemySpeed = SpeedValue;
            EnemyAttackPower = AttackPowerValue;
            EnemyDefense = DefenseValue;
        }
        else if (Type[SeletedType] == "MagicWielder")
        {
            EnemyMaxHealth = MaxHealthValue;
            EnemySpeed = SpeedValue;
            EnemyAttackPower = AttackPowerValue;
            EnemyDefense = DefenseValue;
        }
        else
        {
            EnemyMaxHealth = MaxHealthValue;
            EnemySpeed = SpeedValue;
            EnemyAttackPower = AttackPowerValue;
            EnemyDefense = DefenseValue;
        }
    }

    public void Heal(float Amount)
    {
        EnemyHealth += Amount;
        if (EnemyHealth > EnemyMaxHealth)
        {
            EnemyHealth = EnemyMaxHealth;
        }
    }
}

using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


public class EnemyStats : MonoBehaviour
{
    //EnemyHealth
    public int EnemyMaxHealth;
    public float EnemyHealth;

    //Movement
    public float EnemySpeed;

    //Damage
    public float EnemyAttackPower;
    public float EnemyDefense;
    
    public void TakeDamage(float Damage)
    {
        float FinalDamage = (float)Math.Round(Mathf.Max(10*(Damage*Damage)/10*(Damage+EnemyDefense), 0),2);
        EnemyHealth -= FinalDamage;
        if (EnemyHealth <= 0) 
        {
            EnemyHealth = 0;
            Debug.Log("Player is dead");

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
    public void SetStats () {
        EnemyAttackPower = EnemyAttackPower * 1.1F;
        EnemySpeed = EnemySpeed * 1.1F;
        EnemyDefense = EnemyDefense * 1.1F;
        EnemyHealth = EnemyHealth * 1.1F;
    }
}

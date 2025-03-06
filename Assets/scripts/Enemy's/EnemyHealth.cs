using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public HealthBar Hb;
    public playermovement pm;
    Animator anim;
    EnemyStats Enemystats = new EnemyStats();

    public float DeadDelay;
    bool Dead;

    //particle system
    [SerializeField] ParticleSystem damageParticles;
    ParticleSystem damageParticlesInstance;

    void Start()
    {
        anim = GetComponent<Animator>();

        Hb.setmaxHealth(Enemystats.EnemyMaxHealth);
        Enemystats.EnemyHealth = Enemystats.EnemyMaxHealth;

    }

    public void TakeDamage(float Damage,Vector2 AttackDirection)
    {
        float FinalDamage = (float)Math.Round(Mathf.Max(10*(Damage*Damage)/10*(Damage+Enemystats.EnemyDefense), 0),2);
        Enemystats.EnemyHealth -= FinalDamage;
        Hb.sethealth(Enemystats.EnemyHealth);

        if (Enemystats.EnemyHealth > 0)
        {
            DamageParticles(AttackDirection);
        }
        else
        {
            if (!Dead)
            {

                if (GetComponent<EnemyPatrol>() != null)

                {
                    anim.SetTrigger("dead");
                    GetComponent<EnemyPatrol>().enabled = false;
                    Dead = true;

                }
                else
                {
                    anim.SetTrigger("dead");
                    Invoke("DeadEnemy",DeadDelay);
                    Dead = true;
                    GetComponent<EnemyPatrol>().enabled = false;
                }
            }
        }

    }
    public void DeadEnemy()
    {
        if (this.gameObject.CompareTag("Boss1"))
        {
            pm.KeyCollected = true;
            Destroy(this.gameObject);
        }
        else{
            Destroy(this.gameObject);
        }
    }

    public void DamageParticles(Vector2 AttackDirection)
    {
        Quaternion rot = Quaternion.FromToRotation(Vector2.right,AttackDirection);
        damageParticlesInstance = Instantiate(damageParticles, transform.position, Quaternion.identity);
    }
}

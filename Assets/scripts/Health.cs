using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public HealthBar Hb;
    Animator anim;

    public int MaxHealth = 100;
    public float CurrentHealth;
    public float damage;
    bool Dead;

    //particle system
    [SerializeField] private ParticleSystem damageParticles;
    private ParticleSystem damageParticlesInstance;

    void Start()
    {
        anim = GetComponent<Animator>();

        Hb.setmaxHealth(MaxHealth);
        CurrentHealth = MaxHealth;

    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        Hb.sethealth(CurrentHealth);

        if (CurrentHealth > 0)
        {
            anim.SetTrigger("hurt");
            DamageParticles();
        }
        else
        {
            if (!Dead)
            {

                if (GetComponent<playermovement>() != null)

                {
                    anim.SetTrigger("dead");
                    GetComponent<playermovement>().enabled = false;
                    Dead = true;

                }
                else
                {
                    anim.SetTrigger("dead");
                    Invoke("DeadEnemy",2);
                    Dead = true;
                }
            }
        }

    }
    public void DeadEnemy()
    {
        Destroy(this.gameObject);
    }

    public void DamageParticles()
    {
        damageParticlesInstance = Instantiate(damageParticles, transform.position, Quaternion.identity);
    }
}

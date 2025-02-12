using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public HealthBar Hb;
    public GameObject Ds;
    Animator anim;

    public int MaxHealth = 100;
    public float CurrentHealth;

    bool Dead;

    void Start()
    {
        anim = GetComponent<Animator>();

        Hb.setmaxHealth(MaxHealth);
        CurrentHealth = MaxHealth;

    }
    void Update()
    {
        Hb.sethealth(CurrentHealth);
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        Hb.sethealth(CurrentHealth);

        if (CurrentHealth > 0)
        {
            anim.SetTrigger("hurt");
        }
        else
        {
            if (!Dead)
            {

                if (GetComponent<playermovement>() != null)

                {
                    anim.SetTrigger("dead");
                    Ds.SetActive(true);
                    GetComponent<playermovement>().enabled = false;
                    Dead = true;
                }
            }
        }

    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Death"))
        {
            TakeDamage(MaxHealth);
        }
    }
        
}

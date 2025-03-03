using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using TMPro;
using Cainos.PixelArtPlatformer_VillageProps;

public class playermovement : MonoBehaviour,ISaveable
{
    
    public TextMeshProUGUI scoretext;
    public Rigidbody2D mybody;
    public Animator anim;
    public GameObject LevelText;
    public GameObject next;
    public Chest chest;

    [SerializeField] float moveSpeed;
    [SerializeField] float JumpPower;
    
    float scorecount;
    public bool KeyCollected = false;
    int HighScore;
    
    //Script References 
    EnemyHealth EnemyHealth;
    BossHealth bossHealth;

    //ground check
    bool grounded;

    //Health
    public float damage;
    bool Dead;
    
    //Heavy Attack
    public HeavyAttack Ha;
    public float heavyAttackDamage;
    public float heavyAttackEnergy;
    public float heavyAttackMaxEnergy;

    //particle system
    [SerializeField] ParticleSystem coinParticles;
    ParticleSystem coinParticlesInstance;

    [Header("Attack Parameters")]

    [SerializeField] float attackCooldown1;
    [SerializeField] float attackCooldown2;

    float cooldownTimer1 = Mathf.Infinity;
    float cooldownTimer2 = Mathf.Infinity;

    [SerializeField] float range;

    [Header("Collider Parameters")]
    
    [SerializeField] float colliderDistance;
    [SerializeField] BoxCollider2D boxCollider;

    [Header("Player Layer")]

    [SerializeField] LayerMask EnemyLayer;


    void Update()
    {
        Ha.setEnergyBar(heavyAttackMaxEnergy);
        Ha.setEnergy(heavyAttackEnergy);

        cooldownTimer1 += Time.deltaTime;
        cooldownTimer2 += Time.deltaTime;

        mybody.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, mybody.velocity.y);

        float Horizontalinput = Input.GetAxis("Horizontal");

        //Flip
        if (Horizontalinput > 0.01f)
        {
            transform.localScale = new Vector3(5.1f, 5.1f, 5.1f);
        }
        else if (Horizontalinput < -0.01f)
        {
            transform.localScale = new Vector3(-5.1f, 5.1f, 5.1f);
        }

        //Jump
        if (Input.GetKey(KeyCode.Space) && grounded == true)
        {
            jump();
        }

        //Attacking
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (PlayerRange())
            {
                if (cooldownTimer1 >= attackCooldown1)
                {
                    cooldownTimer1 = 0;
                    anim.SetTrigger("attack");
                    damageEnemy(damage);
                    heavyAttackEnergy += 1;
                    Ha.setEnergy(heavyAttackEnergy);
                }
            }
            else if (cooldownTimer1 >= attackCooldown1)
            {
                cooldownTimer1 = 0;
                anim.SetTrigger("attack");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1) && heavyAttackEnergy >= heavyAttackMaxEnergy)
        {
            if (PlayerRange())
            {
                if (cooldownTimer2 >= attackCooldown2)
                {
                    cooldownTimer2 = 0;
                    anim.SetTrigger("attack2");
                    damageEnemy(heavyAttackDamage);
                    heavyAttackEnergy = 0;
                    Ha.setEnergy(heavyAttackEnergy);
                }

            }
            else if (cooldownTimer2 >= attackCooldown2)
            {
                cooldownTimer2 = 0;
                anim.SetTrigger("attack2");
            }

        }

        anim.SetBool("isgrouned ", grounded);
        anim.SetBool("isrunning ", Horizontalinput != 0);


    }
    private void jump()
    {
        mybody.velocity = new Vector2(mybody.velocity.x, JumpPower);
        grounded = false;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("ground "))
        {
            grounded = true;
        }
        if (other.gameObject.CompareTag("Stage2"))
        {
            if (KeyCollected == true)
            {
                Destroy(other.gameObject);
            }
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {


        if (other.gameObject.CompareTag("waystone"))
        {
            LevelText.SetActive(true);
        }
        if (other.gameObject.CompareTag("Finish"))
        {
            next.SetActive(true);
        }
        if (other.gameObject.CompareTag("Chest"))
        {
            other.GetComponent<Chest>().IsOpened = KeyCollected;
        }

        if (other.gameObject.tag == "coins")
        {
            Destroy(other.gameObject);
            scorecount++;
            Debug.Log(scorecount);
            scoretext.text = "coins:" + scorecount.ToString();
            CoinParticles(other);
        }
        
    }

    void OnTriggerExit2D(Collider2D other)
    {


        if (other.gameObject.CompareTag("waystone"))
        {
            LevelText.SetActive(false);
        }


    }
    bool PlayerRange()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, EnemyLayer);

        if (hit.collider != null)
        {
            EnemyHealth = hit.transform.GetComponent<EnemyHealth>();
            bossHealth = hit.transform.GetComponent<BossHealth>();
        }
            
            

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
    public void damageEnemy(float damage)
    {
        if (EnemyHealth != null)
        {
            EnemyHealth.TakeDamage(damage,transform.right);
        }
        else
        {
            bossHealth.TakeDamage(damage,transform.right);
        }
        
    }

    public void CoinParticles(Collider2D other)
    {
        coinParticlesInstance = Instantiate(coinParticles, other.transform.position, Quaternion.identity);

        
    }

    public void Save()
    {
        SaveData WorldData = new SaveData(HighScore);

        SaveLoad.instance.SaveInfo(WorldData);
    }

    public void Load () {
        
        
    }
}

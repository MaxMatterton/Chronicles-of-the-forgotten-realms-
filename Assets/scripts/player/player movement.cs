using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using TMPro;
using Cainos.PixelArtPlatformer_VillageProps;
using Cainos.LucidEditor;
using System;
using UnityEngine.SceneManagement;

public class playermovement : MonoBehaviour,ISaveable
{
    
    public TextMeshProUGUI Cointext;
    public Rigidbody2D mybody;
    public Animator anim;
    public GameObject LevelText;
    public GameObject next;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI HighScoreText;

    [SerializeField] float JumpPower;
    
    float scorecount;
    public bool KeyCollected = false;
    int HighScore;
    int CurrentLevel;
    
    //Script References 
    EnemyHealth EnemyHealth;
    BossHealth bossHealth;
    public static PlayerStats playerstats;
    public HeavyAttack Ha;
    PlayerHealth PlayerHealth;

    //Health
    public float damage;
    bool Dead;

    //Audio
    AudioSource audioSource;
    public AudioClip DeathAudio;
    public float num1;
    public float num2;
    Unity.Mathematics.Random random;

    //Heavy Attack
    public float heavyAttackDamage;

    //particle system
    [SerializeField] ParticleSystem coinParticles;
    ParticleSystem coinParticlesInstance;

    
    [Header("Ground Check")]
    
    [SerializeField] bool grounded;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckRadius;
    [SerializeField] GameObject groundCheck;

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

    [Header("Player Stats")]
    [SerializeField] float playerHealth;
    [SerializeField] float playerSpeed;
    [SerializeField] float PlayerAttack;
    [SerializeField] float playerDefence;
    
    [Header("BG Switch")]
    [SerializeField] GameObject NormalBG;
    [SerializeField] GameObject CaveBG;

    private void Awake() {

        // Initialize the player stats script
        playerstats = new PlayerStats();
        playerstats.SetPoints(PlayerAttack,playerDefence,playerSpeed,playerHealth);

        // Initialize the audio source
        audioSource = this.GetComponent<AudioSource>();
        
        PlayerHealth = GetComponent<PlayerHealth>();
        if (audioSource != null )
        {
            Debug.Log("Audio Connected");
        }
            
        CurrentLevel = SceneManager.GetActiveScene().buildIndex;
        playerstats.Score = 0;
    }
    
    void Update()
    {
        Ha.setEnergyBar(playerstats.MaxEnergy);
        Ha.setEnergy(playerstats.Energy);

        cooldownTimer1 += Time.deltaTime;
        cooldownTimer2 += Time.deltaTime;

        mybody.velocity = new Vector2(Input.GetAxis("Horizontal") * playerstats.Speed, mybody.velocity.y);

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

        GroundCheck();
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
                    playerstats.Energy += 1;
                    Ha.setEnergy(playerstats.Energy);
                    
                }
            }
            else if (cooldownTimer1 >= attackCooldown1)
            {
                cooldownTimer1 = 0;
                anim.SetTrigger("attack");
            }
                    
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1) && playerstats.Energy >= playerstats.MaxEnergy)
        {
            if (PlayerRange())
            {
                if (cooldownTimer2 >= attackCooldown2)
                {
                    cooldownTimer2 = 0;
                    anim.SetTrigger("attack2");
                    damageEnemy(heavyAttackDamage);
                    playerstats.Energy = 0;
                    Ha.setEnergy(playerstats.Energy);
                    
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

    void GroundCheck()
    {
        // Raycast downward to check for ground
        grounded = Physics2D.Raycast(groundCheck.transform.position, Vector2.down, groundCheckRadius, groundLayer);
        
        // Optional Debugging
        Debug.DrawRay(groundCheck.transform.position, Vector2.down * groundCheckRadius, Color.red);
    }

    private void jump()
    {
        mybody.velocity = new Vector2(mybody.velocity.x, JumpPower);
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        // if (other.gameObject.CompareTag("ground "))
        // {
        //     grounded = true;
        // }
        if (other.gameObject.CompareTag("Stage2"))
        {
            if (KeyCollected == true)
            {
                Destroy(other.gameObject);
            }
        }
        if (other.gameObject.CompareTag("Trap"))
        {
            PlayerHealth.TakeDamage(50,false);

            // Determine the direction based on player's facing
            float knockbackDirection = transform.localScale.x > 0 ? -1 : 1;

            // Apply knockback force
            mybody.AddForce(new Vector2(knockbackDirection * 10, 10), ForceMode2D.Impulse);
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
            playerstats.HighScoreCheck();
            ScoreText.text = playerstats.Score.ToString();
            HighScoreText.text = playerstats.HighScore.ToString();
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
            Cointext.text = "coins:" + scorecount.ToString();
            CoinParticles(other);
        }
        if (other.gameObject.CompareTag("switchBG"))
        {
            if (NormalBG.activeInHierarchy)
            {
                NormalBG.SetActive(false);
                CaveBG.SetActive(true);
            }
            else if (CaveBG.activeInHierarchy)
            {
                NormalBG.SetActive(true);
                CaveBG.SetActive(false);
            }
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
        SaveData WorldData = new SaveData(CurrentLevel,HighScore);

        SaveLoad.instance.SaveInfo(WorldData);
    }

    public void Load () {
        
        
    }
}

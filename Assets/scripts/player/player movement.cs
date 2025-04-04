using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using TMPro;
using Cainos.PixelArtPlatformer_VillageProps;
using Cainos.LucidEditor;
using System;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class playermovement : MonoBehaviour,ISaveable
{
    public TextMeshProUGUI Cointext;
    public Rigidbody2D mybody;
    public Animator anim;
    public GameObject LevelText;
    public GameObject next;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI HighScoreText;

    [Header("Score")]
    float scorecount;
    public bool KeyCollected = false;
    int HighScore;
    int CurrentLevel; 
    
    //Script References
    [Header("Scripts")] 
    List<LevelHighScore> scores = new List<LevelHighScore>();
    EnemyHealth EnemyHealth;
    BossHealth bossHealth;
    public static PlayerStats playerstats;
    public HeavyAttack Ha;
    PlayerHealth PlayerHealth;

    //Health
    [Header("Health")]
    public float damage;
    bool Dead;

    //Audio
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip coinAudio;
    public AudioClip lightattackAudio;
    public AudioClip heavyattackAudio;
    public float num1;
    public float num2;
    

    //Heavy Attack
    [Header("Heavy Attack")]
    public float heavyAttackDamage;

    //particle system
    [Header("Particle System")]
    [SerializeField] ParticleSystem coinParticles;
    ParticleSystem coinParticlesInstance;

    //Chests
    [Space]
    bool FreeChest = true;

    
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
    [SerializeField] float JumpPower;
    
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
        //Jump

        GroundCheck();
        //Attacking
        anim.SetBool("isgrouned ", grounded);
    }
    public void BasicAttack () {
        if (PlayerRange())
            {
                if (cooldownTimer1 >= attackCooldown1)
                {
                    cooldownTimer1 = 0;
                    anim.SetTrigger("attack");
                    damageEnemy(damage);
                    playerstats.Energy += 1;
                    Ha.setEnergy(playerstats.Energy);
                    PlayWithRandomPitch(lightattackAudio);
                }
            }
            else if (cooldownTimer1 >= attackCooldown1)
            {
                cooldownTimer1 = 0;
                anim.SetTrigger("attack");
                PlayWithRandomPitch(lightattackAudio);
            }
    }
    public void HeavyAttack () {
        if (playerstats.Energy >= playerstats.MaxEnergy)
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
                    PlayWithRandomPitch(heavyattackAudio);
                }

            }
            else if (cooldownTimer2 >= attackCooldown2)
            {
                cooldownTimer2 = 0;
                anim.SetTrigger("attack2");
                PlayWithRandomPitch(heavyattackAudio);
            }

        }
    }
    public void PlayWithRandomPitch( AudioClip clip)
    {
        float randomPitch = Random.Range(num1, num2);
        audioSource.pitch = randomPitch;

        audioSource.PlayOneShot(clip);
    
        
    }

    public void Move(float move, bool jump)
    {
        if (grounded || true) // Assumes air control is allowed
        {
            Vector2 targetVelocity = new Vector2(move * playerstats.Speed, mybody.velocity.y);
            mybody.velocity = targetVelocity;

            if (move > 0)
            {
                transform.localScale = new Vector3(5.1f, 5.1f, 5.1f);
            }
            else if (move < 0)
            {
                transform.localScale = new Vector3(-5.1f, 5.1f, 5.1f);
            }
        }
        if (grounded == true && jump == true)
        {
            mybody.velocity = new Vector2(mybody.velocity.x, JumpPower);
        }
        anim.SetBool("isrunning ",move != 0);
    }

    void GroundCheck()
    {
        // Raycast downward to check for ground
        grounded = Physics2D.Raycast(groundCheck.transform.position, Vector2.down, groundCheckRadius, groundLayer);
        
        // Optional Debugging
        Debug.DrawRay(groundCheck.transform.position, Vector2.down * groundCheckRadius, Color.red);
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        
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
            mybody.AddForce(new Vector2(knockbackDirection * 20, 20), ForceMode2D.Impulse);
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
            int index = scores.FindIndex(x => x.LevelNumber == CurrentLevel);

            if (index != -1)
            {
                scores[index] = new LevelHighScore { LevelNumber = CurrentLevel, HighScore = playerstats.HighScore };
            }
            else
            {
                scores.Add(new LevelHighScore { LevelNumber = CurrentLevel, HighScore = playerstats.HighScore });
            }
        }

        if (other.gameObject.CompareTag("BossChest"))
        {
            other.GetComponent<Chest>().IsOpened = KeyCollected;
        }

        if (other.gameObject.CompareTag("Chest"))
        {
            other.GetComponent<Chest>().IsOpened = FreeChest;
            playerstats.Score += 100;
        }

        if (other.gameObject.tag == "coins")
        {
            audioSource.pitch = 1;
            audioSource.PlayOneShot(coinAudio);
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
        
        SaveData WorldData = new SaveData(scores);

        SaveLoad.instance.SaveInfo(WorldData);
    }

    public void Load () {
        
        
    }
}

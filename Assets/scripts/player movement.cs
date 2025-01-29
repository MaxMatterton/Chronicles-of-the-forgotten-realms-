using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using TMPro;

public class playermovement : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D mybody;
    public Animator anim;
    public GameObject LevelText;
    [SerializeField] float moveSpeed;
    [SerializeField] float JumpPower;
    [SerializeField] TextMeshProUGUI scoretext;
    float scorecount;
    public GameObject next;
    Health EnemyHealth;


    //ground check
    bool grounded;

    //Health
    public float damage;
    bool Dead;

    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown1;
    [SerializeField] private float attackCooldown2;
    private float cooldownTimer1 = Mathf.Infinity;
    private float cooldownTimer2 = Mathf.Infinity;
    [SerializeField] private float range;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask EnemyLayer;


    void Update()
    {
        cooldownTimer1 += Time.deltaTime;
        cooldownTimer2 += Time.deltaTime;

        mybody.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, mybody.velocity.y);

        float Horizontalinput = Input.GetAxis("Horizontal");


        if (Horizontalinput > 0.01f)
        {
            transform.localScale = new Vector3(5.1f, 5.1f, 5.1f);
        }
        else if (Horizontalinput < -0.01f)
        {
            transform.localScale = new Vector3(-5.1f, 5.1f, 5.1f);
        }

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

                }

            }
            else if (cooldownTimer1 >= attackCooldown1)
            {
                cooldownTimer1 = 0;
                anim.SetTrigger("attack");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (PlayerRange())
            {
                if (cooldownTimer2 >= attackCooldown2)
                {
                    cooldownTimer2 = 0;
                    anim.SetTrigger("attack2");

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

        if (other.gameObject.tag == "coins")
        {
            Destroy(other.gameObject);
            scorecount++;
            Debug.Log(scorecount);
            scoretext.text = "coins:" + scorecount.ToString();
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
            EnemyHealth = hit.transform.GetComponent<Health>();

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
    public void damageEnemy()
    {
        EnemyHealth.TakeDamage(damage);
    }
}

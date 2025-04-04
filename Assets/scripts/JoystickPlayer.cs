using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickPlayer : MonoBehaviour
{
     Rigidbody2D rb;
    public Joystick mj;
    public float jumpspeed;
    public playermovement controller;
    bool jump = false;
    float horizontalmove;
    float x;
    [Range(1, 10)]
    public float jumpvelocity;
    public float Sensitivity;
    Animator anim;
    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        horizontalmove = x = mj.Horizontal * playermovement.playerstats.Speed;
        rb.velocity = new Vector2(x * playermovement.playerstats.Speed, rb.velocity.y);
        float verticalmove = mj.Vertical * jumpspeed;
        Debug.Log(verticalmove);
        if (verticalmove >= Sensitivity)
        {
            jump = true;
            //GetComponent<Rigidbody2D>().velocity = Vector2.up * jumpvelocity;   
        }
        //anim.SetBool("isrunning")
    }
    private void FixedUpdate()
    { 
        controller.Move(horizontalmove * Time.fixedDeltaTime, jump);
        jump = false;
    }
    
}

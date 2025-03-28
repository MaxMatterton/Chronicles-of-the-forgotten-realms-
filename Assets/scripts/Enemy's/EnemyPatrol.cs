using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] Transform leftEdge;
    [SerializeField] Transform rightEdge;

    [Header("Enemy")]
    [SerializeField] Transform enemy;

    [Header("Movement parameters")]
    EnemyStats Em = new EnemyStats();
    public Vector3 initScale;
    public bool movingLeft;

    [Header("Idle Behaviour")]
    [SerializeField] float idleDuration;
    float idleTimer;

    [Header("Enemy Animator")]
    [SerializeField] Animator anim;

    private void Awake()
    {
        initScale = enemy.localScale;
    }

    private void OnDisable()
    {
        anim.SetBool("isrunning", false);
    }

    private void Update()
    {
        if (movingLeft)
        {
            if (enemy.position.x >= leftEdge.position.x)
                MoveInDirection(-1);
            else
                DirectionChange();
        }
        else
        {
            if (enemy.position.x <= rightEdge.position.x)
            {
                MoveInDirection(1);
            }
            else
            {
                DirectionChange();
            }
        }
    }

    public void DirectionChange()
    {

        anim.SetBool("isrunning", false);
        idleTimer += Time.deltaTime;

        if (idleTimer > idleDuration)
        {
            movingLeft = !movingLeft;
        }
    }

    private void MoveInDirection(int _direction)
    {

        idleTimer = 0;
        anim.SetBool("isrunning", true);

        //Make enemy face direction
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction ,
            initScale.y, initScale.z);

        //Move in that direction
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * Em.EnemySpeed,
            enemy.position.y, enemy.position.z);

    }

    public void lookatplayer()
    {
        anim.SetBool("isrunning", false);
        movingLeft = !movingLeft;
    }

}

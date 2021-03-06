using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMovement : MonoBehaviour, iHealth
{
    public Manager ManagerReference { get; set; }
    public Queue<(Vector2 pos, float time)> MoveHistory { protected get; set; }
    private (Vector2 pos, float time) currentMove;
    private Rigidbody2D rb;
    private float t = 0;
    private int health;

    private Animator animator;
    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
            if (health <= 0)
            {
                die();
            }
        }
    }

    private bool isAtEndOfPath;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentMove = MoveHistory.Dequeue();

        Health = 1;

        animator = GetComponentInChildren<Animator>();
        animator.SetBool("Idle", true);
    }

    // Update is called once per frame
    void Update()
    {

        if (MoveHistory != null && !isAtEndOfPath)
        {
            t += Time.deltaTime;
            rb.velocity = currentMove.pos;
            if(t > currentMove.time)
            {
                t = 0;
                if (MoveHistory.Count != 0)
                {
                    currentMove = MoveHistory.Dequeue();
                    if(currentMove.pos == Vector2.zero)
                    {
                        animator.SetBool("Idle", true);
                    }
                    else
                    {
                        animator.SetFloat("x", currentMove.pos.x);
                        animator.SetFloat("y", currentMove.pos.y);
                        animator.SetBool("Idle", false);
                    }
                }
                else
                {
                    isAtEndOfPath = true;
                    ManagerReference.Victory();
                }
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Clover")
        {
            Destroy(collision.gameObject, 0);
        }
    }
    void die()
    {
        Destroy(gameObject, 0);
        ManagerReference.GameOver();
    }
}

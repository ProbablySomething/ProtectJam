using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMovement : MonoBehaviour, iHealth
{
    public Queue<(Vector2 pos, float time)> MoveHistory { protected get; set; }
    private (Vector2 pos, float time) currentMove;
    private Rigidbody2D rb;
    private float t = 0;
    private int health;

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
                }
                else
                {
                    isAtEndOfPath = true;
                }
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    void die()
    {
        Debug.Log("Passive Lamb Dieing");
        rb.velocity = new Vector2(0, 0);
        Destroy(this, 0);
        FindObjectOfType<Manager>().GameOver();
    }
}

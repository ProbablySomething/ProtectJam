using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMovement : MonoBehaviour
{
    public Queue<(Vector2 pos, float time)> MoveHistory { protected get; set; }
    private (Vector2 pos, float time) currentMove;
    private Rigidbody2D rb;
    private float t = 0;

    private bool isAtEndOfPath;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentMove = MoveHistory.Dequeue();
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
}

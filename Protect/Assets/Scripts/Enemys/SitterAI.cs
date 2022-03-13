using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SitterAI : MonoBehaviour, iHealth
{

    [SerializeField] private float attackRange;
    [SerializeField] private float attackDelay;
    [SerializeField] private float lungeSpeed;

    Coroutine readying;
    MovementController movement;
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
    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<CircleCollider2D>().radius = attackRange;
        movement = GetComponent<MovementController>();
        movement.Speed = lungeSpeed;

        animator = GetComponentInChildren<Animator>();
        animator.SetBool("InRange", false);
        Health = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.name == "LambPassive(Clone)")
        {
            Debug.Log("Stopping");
            StopCoroutine(readying);
            animator.SetBool("InRange", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "LambPassive(Clone)")
        {
            if(readying != null)
                StopCoroutine(readying);
            Vector2 dir = (collision.transform.position - transform.position).normalized;
            animator.SetFloat("x", dir.x);
            animator.SetFloat("y", dir.y);
            animator.SetBool("InRange", true);
            readying = StartCoroutine(AttackDelay(collision.gameObject));
        }
    }

    void attack(GameObject target)
    {
        if (target.tag == "lambPassive")
        {
            animator.SetTrigger("Attack");
            Vector2 dir = target.transform.position - transform.position;
            movement.HandleMovement(dir.normalized);
            StartCoroutine(lungeStop(dir.magnitude));
            target.GetComponent<iHealth>().Health = 0;
        }
    }

    void die()
    {
        Destroy(gameObject, 0);
    }

    IEnumerator AttackDelay(GameObject target)
    {
        yield return new WaitForSeconds(attackDelay);
        if(target != null)
            attack(target);
    }

    IEnumerator lungeStop(float distance)
    {
        yield return new WaitForSeconds(distance / lungeSpeed);
        movement.HandleMovement(new Vector2(0, 0));
    }
}

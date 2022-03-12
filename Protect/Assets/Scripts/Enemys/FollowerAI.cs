using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerAI : MonoBehaviour, iHealth
{
    [SerializeField] private float speed;
    [SerializeField] private float aggroRange;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCooldown;

    GameObject target;

    bool awake = false;
    bool onCooldown = false;

    MovementController movement;
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

    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<CircleCollider2D>().radius = aggroRange;
        movement = GetComponent<MovementController>();
        movement.Speed = speed;

        Health = 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!awake)
        {
            if (collision.gameObject.name == "LambPassive(Clone)")
            {
                awake = true;
                target = collision.gameObject;
                GetComponentInChildren<CircleCollider2D>().radius = attackRange;
            }
        }
        else
        {
            if(collision.gameObject.name == "LambPassive(Clone)")
            {
                attack(target);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(awake)
        {
            if(target != null)
                movement.HandleMovement((target.transform.position - transform.position).normalized);
        }
    }

    void attack(GameObject target)
    {
        if(!onCooldown)
        {
            if (target.tag == "lambPassive")
            {
                target.GetComponent<iHealth>().Health -= 1;
                onCooldown = true;
                StartCoroutine(cooldown());
            }
        }
    }

    IEnumerator cooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        onCooldown = false;
    }

    void die()
    {
        Destroy(gameObject, 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerAI : MonoBehaviour, iHealth
{
    [SerializeField] private float speed;
    [SerializeField] private float aggroRange;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCooldown;
    [SerializeField] private int health;

    GameObject target;

    bool awake = false;
    bool onCooldown = false;

    MovementController movement;

    public int Health {
        get
        {
            return health;
        }
        set
        {
            health = value;
            if(health <= 0)
            {
                die();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<CircleCollider2D>().radius = aggroRange;
        movement = GetComponent<MovementController>();
        movement.Speed = speed;
        Debug.Log(name + " has a speed of " + speed + ' ' + movement.Speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!awake)
        {
            Debug.Log(collision.gameObject.name);
            if (collision.gameObject.name == "LambPassive(Clone)")
            {
                awake = true;
                target = collision.gameObject;
                GetComponent<CircleCollider2D>().radius = attackRange;
            }
        }
        else
        {
            if(collision.gameObject.name == target.name)
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
            movement.HandleMovement((target.transform.position - transform.position).normalized);
        }
    }

    void attack(GameObject target)
    {
        if(!onCooldown)
        {
            onCooldown = true;
            //add health n stuff
            Debug.Log(name + " Just hit you");
            StartCoroutine(cooldown());
        }
    }

    IEnumerator cooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        onCooldown = false;
    }

    void die()
    {
        Destroy(this);
    }
}

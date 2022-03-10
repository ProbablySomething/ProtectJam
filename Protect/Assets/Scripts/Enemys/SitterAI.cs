using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SitterAI : MonoBehaviour
{

    [SerializeField] private float attackRange;
    [SerializeField] private float attackDelay;
    [SerializeField] private float lungeSpeed;

    Coroutine readying;
    MovementController movement;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<CircleCollider2D>().radius = attackRange;
        movement = GetComponent<MovementController>();
        movement.Speed = lungeSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.name == "LambPassive(Clone)")
        {
            StopCoroutine(readying);
            Debug.Log("Stopped Coroutine");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "LambPassive(Clone)")
        {
            readying = StartCoroutine(AttackDelay(collision.gameObject));
            Debug.Log("Started Coroutine");
        }
    }

    void attack(GameObject target)
    {
        Vector2 dir = target.transform.position - transform.position;
        movement.HandleMovement(dir.normalized);
        StartCoroutine(lungeStop(dir.magnitude));
        Debug.Log("Hit you");
    }


    IEnumerator AttackDelay(GameObject target)
    {
        yield return new WaitForSeconds(attackDelay);
        attack(target);
    }

    IEnumerator lungeStop(float distance)
    {
        yield return new WaitForSeconds(distance / lungeSpeed);
        movement.HandleMovement(new Vector2(0, 0));
    }
}

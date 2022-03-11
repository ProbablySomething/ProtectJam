using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class ShepardController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float attackRange;

    [SerializeField] InputActionReference moveAction;
    [SerializeField] InputActionReference attackAction;

    MovementController movement;

    Vector2 currDir;

    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<MovementController>();
        movement.Speed = speed;

        moveAction.action.Enable();
        moveAction.action.performed += HandleMovement;
        moveAction.action.canceled += HandleMovement;



        attackAction.action.Enable();
        attackAction.action.performed += HandleAttack;
    }

    // Update is called once per frame
    void Update()
    {
        
    
    }

    private void OnDisable()
    {
        moveAction.action.performed -= HandleMovement;
        moveAction.action.canceled -= HandleMovement;
        moveAction.action.Disable();

        attackAction.action.performed -= HandleAttack;
        attackAction.action.Disable();
    }

    private void HandleMovement(InputAction.CallbackContext context)
    {
        Vector2 move = context.ReadValue<Vector2>();
        if (move != new Vector2(0, 0))
        {
            currDir = move.normalized;
        }
        movement.HandleMovement(move);
    } 

    private void HandleAttack(InputAction.CallbackContext context)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, currDir, attackRange);
        Debug.DrawRay(transform.position, currDir * attackRange, Color.black, 100);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.tag == "enemy")
            {
                hit.collider.GetComponent<iHealth>().Health -= 1;
            }
        }
        
    }
}

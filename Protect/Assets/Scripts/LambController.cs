using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using System;

public class LambController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private InputActionReference moveAction;

    public Manager ManagerInstance { get; set; }

    private Vector2 prevMove;
    private Vector2 prevPos;
    private float timeBetweenMove = 0;

    private Queue<ValueTuple<Vector2, float>> moveHistory;

    private MovementController movement;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        moveHistory = new Queue<(Vector2 pos, float time)>();
        movement = GetComponent<MovementController>();
        movement.Speed = speed;

        moveAction.action.Enable();
        moveAction.action.performed += HandleMovement;
        moveAction.action.canceled += HandleMovement;

        prevPos = transform.position;

        animator = GetComponentInChildren<Animator>();
        animator.SetBool("Idle", true);
    }

    private void OnDisable()
    {
        moveAction.action.performed -= HandleMovement;
        moveAction.action.canceled -= HandleMovement;
        moveAction.action.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        timeBetweenMove += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Flag")
        {
            moveHistory.Enqueue((prevMove * speed, timeBetweenMove));
            ManagerInstance.MoveHistory = moveHistory;
            
            ManagerInstance.SetPhase((int)Manager.LevelPhase.Ghost);
        }else if(collision.gameObject.tag == "Clover")
        {

            ManagerInstance.numOfClovers += 1;
            Destroy(collision.gameObject, 0);
        }
    }

    private void HandleMovement(InputAction.CallbackContext context)
    {
        Vector2 move = context.ReadValue<Vector2>();
        movement.HandleMovement(move);

        if(move != prevMove)
        {
            moveHistory.Enqueue((prevMove * speed, timeBetweenMove));
            timeBetweenMove = 0f;
        }
        if(move != new Vector2(0, 0))
        {
            animator.SetBool("Idle", false);
            animator.SetFloat("x", move.x);
            animator.SetFloat("y", move.y);
        }
        else
        {
            animator.SetBool("Idle", true);
        }
        prevMove = move;
    }

    public void handleMoveHistory()
    {
        moveHistory.Enqueue((prevMove * speed, timeBetweenMove));
        ManagerInstance.MoveHistory = moveHistory;
    }
}

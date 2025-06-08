using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private Rigidbody2D rb;
    // Start is called before the first frame 
    private IState currentState;

    void Update()
    {
        if (currentState != null)
        {
            currentState.OnExecute(this);
        }
    }


    public override void OnInit()
    {
        base.OnInit();
        ChangeState(new IdleState());
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        Debug.Log("Enemy has died.");
    }



    public void ChangeState(IState newState)
    {
        // Logic to change the state of the enemy
        // This could involve calling OnExit on the current state and OnEnter on the new state
        if (currentState != null)
        {
            currentState.OnExit(this);
        }

        currentState = newState;

        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
    }

    public void Moving()
    {
        ChangeAnim("run");

        rb.velocity = new Vector2(speed, rb.velocity.y);
    }

    public void StopMoving()
    {
        rb.velocity = Vector2.zero;
        ChangeAnim("idle");
    }

    public void Attack()
    {

    }

    public bool IsTargetInRange()
    {
        return false;
    }
}

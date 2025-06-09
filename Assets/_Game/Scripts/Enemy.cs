using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject attackArea;
    private bool isRight = true;
    private Character target;
    public Character Target => target;
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
        DeActiveAttack();
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        Destroy(gameObject);
    }

    protected override void OnDeath()
    {
        ChangeState(null);
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
    internal void SetTarget(Character character)
    {
        this.target = character;

        if (IsTargetInRange())
        {
            ChangeState(new AttackState());
        }
        else if (Target != null)
        {
            ChangeState(new PatrolState());
        }
        else
        {
            ChangeState(new IdleState());
        }
    }
    public void Moving()
    {
        ChangeAnim("run");

        rb.velocity = transform.right * speed;
    }

    public void StopMoving()
    {
        rb.velocity = Vector2.zero;
        ChangeAnim("idle");
    }

    public void Attack()
    {
        ChangeAnim("attack");
        ActiveAttack();

        Invoke(nameof(DeActiveAttack), 0.66f);
    }

    public bool IsTargetInRange()
    {
        if (Target != null)
        {
            return Vector2.Distance(target.transform.position, transform.position) < attackRange;
        }
        else
            return false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyWall"))
        {
            ChangDirection(!isRight);
        }
    }

    public void ChangDirection(bool isRight)
    {
        this.isRight = isRight;

        transform.rotation = isRight ? Quaternion.Euler(Vector3.zero) : Quaternion.Euler(Vector3.up * 180);
    }

    private void ActiveAttack()
    {
        attackArea.SetActive(true);
    }

    private void DeActiveAttack()
    {
        attackArea.SetActive(false);
    }
}

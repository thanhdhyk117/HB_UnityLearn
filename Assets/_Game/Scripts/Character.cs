using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    [SerializeField] private Animator animator;
    [SerializeField] protected HealthBar healthBar;
    [SerializeField] private CombatText combatTextPrefab;
    private float hp;

    [SerializeField] private string currentAnimName;

    public bool IsDead => hp <= 0;

    void Start()
    {
        OnInit();
    }


    public virtual void OnInit()
    {
        hp = 100;
        healthBar.OnInit(100, transform);
    }
    public virtual void OnDespawn()
    {

    }
    protected void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            animator.ResetTrigger(animName);
            currentAnimName = animName;
            animator.SetTrigger(currentAnimName);
        }
    }


    public void OnHit(float damage)
    {
        Debug.Log("hit");
        if (!IsDead)
        {
            hp -= damage;


            if (IsDead)
            {
                hp = 0;
                OnDeath();
            }
            healthBar.SetNewHp(hp);
            Instantiate(combatTextPrefab, transform.position + Vector3.up, Quaternion.identity).OnInit(damage);
        }
    }

    protected virtual void OnDeath()
    {
        ChangeAnim("die");
        Invoke(nameof(OnDespawn), 2);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    [SerializeField] private Animator animator;
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
        if (!IsDead)
        {
            hp -= damage;

            if (IsDead)
            {
                OnDeath();
            }
        }
    }

    protected virtual void OnDeath()
    {

    }
}

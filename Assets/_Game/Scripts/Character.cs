using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected Animator anim;
    [SerializeField] protected HealthBar healthBar;
    [SerializeField] private CombatText combatTextPrefab;

    private float hp;
    private string currentAnimName;


    public bool IsDead => hp <= 0;

    protected virtual void Start()
    {
        OnInit();
    }
    protected virtual void OnInit()
    {
        hp = 100;
        healthBar.OnInit(100,transform);
    }
    protected virtual void OnDespawn()
    {

    }
    
    protected virtual void OnDeath()
    {
        ChangeAnim("die");
        Invoke(nameof(OnDespawn), 0.5f);
    }

    protected void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            anim.ResetTrigger(animName);
            currentAnimName = animName;
            anim.SetTrigger(currentAnimName);
        }
    }
    public virtual void OnHit(float damege)
    {
        Debug.Log("hitl");
        if (!IsDead)
        {
            hp -= damege;
            if (IsDead)
            {
                hp = 0;
                OnDeath();
            }

            healthBar.SetNewHp(hp);
            Instantiate(combatTextPrefab, transform.position + Vector3.up, Quaternion.identity).OnInit(damege);
        }
    }
}

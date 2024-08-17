using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHP;
    public int currentHP;
    private Character _cc;

    private void Awake()
    {
        currentHP = maxHP;
        _cc = GetComponent<Character>();
    }
    public float CurrentHealthProgress
    {
        get
        {
            return (float)currentHP / (float)maxHP;
        }
    }
    public void ApplyDamage(int damage)
    { 
        currentHP -= damage;
        Debug.Log(gameObject.name+" damege :"+damage);
        CheckHealth();
    }

    private void CheckHealth()
    {
        if (currentHP<=0)
        {
            _cc.SwitchStateTo( Character.CharacterState.Dead);
        }
    }

    internal void Addhealth(int heal)
    {
        currentHP += heal;
        if (currentHP>maxHP)
        {
            currentHP = maxHP;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int health = 100;
    [SerializeField] private int healthMax = 100;
    public event EventHandler OnDead;
    public event EventHandler OnDamaged;

    private void Awake()
    {
        health = healthMax;
    }
    public void Damgae(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            health = 0;
        }


        OnDamaged?.Invoke(this, EventArgs.Empty);
        if (health == 0)
        {
            Die();
        }
        
    }

    private void Die()
    {
        OnDead?.Invoke(this,EventArgs.Empty);
    }

    public float GetHealthNormalized()
    {
        return  ((float)health / (float)healthMax);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public int initialHealth;
    int currentHealth;

    private void Start()
    {
        currentHealth = initialHealth;
    }

    public void RemoveHealth(int health)
    {
        currentHealth -= health;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void AddHealth(int health)
    {
        if (health > initialHealth - currentHealth)
        {
            currentHealth = initialHealth;
        }
        currentHealth = health;
    }

    public void SetHealth(int health)
    {
        currentHealth = health;
    }
}

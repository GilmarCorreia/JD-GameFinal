using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JonasVida : MonoBehaviour
{
    public HealthbarScript JonasHealth;
    public int maxHealth = 60;
    public int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        JonasHealth.SetMaxHealth(currentHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        JonasHealth.SetHealth(currentHealth);
    }

}

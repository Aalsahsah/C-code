using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyController : MonoBehaviour
{
    public int EnemyDamage;

    [SerializeField] private int enemyMaxHealth;
    private int enemyCurrentHealth;

    public bool isDead { get; set; }

    public event Action OnEnemyDeath;
    public event Action<int, int> OnEnemyHealthChange;

    //This the games current audio source
    AudioSource audioSource;
    //This stores the enemies's sound effects
    [SerializeField] AudioClip enemyDamageSound, enemyDeathSound;

    //Initialize player data
    private void Awake()
    {
        isDead = false;
        audioSource = FindObjectOfType<AudioSource>();
        enemyCurrentHealth = enemyMaxHealth;
    }

    public void TakeDamage(int damageToTake)
    {
        enemyCurrentHealth -= damageToTake;
        OnEnemyHealthChange?.Invoke(enemyCurrentHealth, enemyMaxHealth);

        if (enemyCurrentHealth <= 0)
        {
            Die();
        }
        else
        {
            audioSource.PlayOneShot(enemyDamageSound);
        }
    }
    private void Die()
    {
        isDead = true;
        OnEnemyDeath?.Invoke();

        audioSource.PlayOneShot(enemyDeathSound);

        Destroy(gameObject);
    }

    private float HealthPercentage(float currentHealth, float maxHealth)
    {
        return currentHealth / maxHealth * 100;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    //THIS SCRIPT IS USED TO KEEP THE PLAYER'S HEALTH AND OTHER DATA

    //Unity makes you use public fields instead of properties to set values in inspector
    public int PlayerMaxHealth;
    private int playerCurrentHealth;

    //Event called when player takes damage, takes an int parameter
    public event Action<int> OnPlayerHealthChange;

    //This the games current audio source
    AudioSource audioSource;
    //This stores the player's sound effects
    [SerializeField] AudioClip playerDamageSound, playerDeathSound;


    private void Awake()
    {
        audioSource = FindObjectOfType<AudioSource>();
    }

    void Start()
    {
        playerCurrentHealth = PlayerMaxHealth;
    }

    //These are all about taking damage
    public void PlayerTakeDamage(int damageTaken)
    {
        playerCurrentHealth -= damageTaken;

        //using current health as a parameter to keep current health private, may change if needed
        OnPlayerHealthChange?.Invoke(playerCurrentHealth);

        CheckForDeath();
    }
    private void CheckForDeath()
    {
        if(playerCurrentHealth <= 0)
        {
            PlayerDie();
        }
        else
        {
            audioSource.PlayOneShot(playerDamageSound);
        }
    }
    private void PlayerDie()
    {
        audioSource.PlayOneShot(playerDeathSound);
        print("Oh dear you are dead!");
        gameObject.SetActive(false);
    }

    public void HealPlayer(int healthHealed)
    {
        //This check prevents overheal
        int possibleNewHealthValue = playerCurrentHealth + healthHealed;

        if (possibleNewHealthValue < PlayerMaxHealth)
        {
            playerCurrentHealth += healthHealed;
        }
        else
        {
            playerCurrentHealth = PlayerMaxHealth;
        }

        OnPlayerHealthChange?.Invoke(playerCurrentHealth);
    }

}
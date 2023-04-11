using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealObject : MonoBehaviour
{
    [SerializeField] private int healAmount;
    
    //This will store the player object
    private PlayerData playerData;

    //This the games current audio source
    AudioSource audioSource;
    //These are the sound clips
    [SerializeField] AudioClip playerHealSound;

    //Initialize player data
    private void Awake()
    {
        playerData = FindObjectOfType<PlayerData>();
        audioSource = FindObjectOfType<AudioSource>();
    }

    //When the enemy collides with the player, make the player take specified damage
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerData.HealPlayer(healAmount);
            audioSource.PlayOneShot(playerHealSound);
            gameObject.SetActive(false);
        }
    }
}

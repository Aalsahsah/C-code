using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropController : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] private AudioClip soundEffect;

    private void Awake()
    {
        audioSource = FindObjectOfType<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            audioSource.PlayOneShot(soundEffect);
            Destroy(gameObject);
        }
    }
}

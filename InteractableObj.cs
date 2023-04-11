using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObj : MonoBehaviour
{
    bool playerIsInRange = false;
    GameObject pickupActive;

    //AUDIO
    [SerializeField] private AudioClip soundEffect;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Awake()
    {
        audioSource = FindObjectOfType<AudioSource>();

        //This grabs the first child object of this object
        pickupActive = gameObject.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsInRange && Input.GetKeyDown(KeyCode.E))
        {
            audioSource.PlayOneShot(soundEffect);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerData>())
        {
            playerIsInRange = true;
            pickupActive.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerData>())
        {
            playerIsInRange = false;
            pickupActive.SetActive(false);
        }
    }
}

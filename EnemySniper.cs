using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySniper : MonoBehaviour
{
    //References this enemy's enemy controller script
    private EnemyController enemyController;

    private AttackRange attackRange;

    [SerializeField] private float timeBetweenShots;

    //Reference to enemy rigidbody
    private Rigidbody2D enemyRb;

    //Player info
    private GameObject player;
    private Vector2 playerPos;

    //References to raycast for line of sight
    Ray2D ray;
    RaycastHit2D rayHit;
    [SerializeField] private float sightRange;

    //Note that this may get taken out and made in a "AwareRange" script later
    public bool isAwareOfPlayer { get; set; }


    [Header("Bullet References")]
    [SerializeField] private GameObject bullet, sniperBarrel;
    [SerializeField] private float bulletSpeed;

    // flag for when enemy is waiting to attack again
    private bool isAbleToFire;

    //This the games current audio source
    AudioSource audioSource;
    //This stores the player's sound effects
    [SerializeField] AudioClip sniperShotSound;


    void Awake()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        enemyController = GetComponent<EnemyController>();
        attackRange = GetComponentInChildren<AttackRange>();
        audioSource = FindObjectOfType<AudioSource>();

        player = GameObject.FindGameObjectWithTag("Player");

        isAwareOfPlayer = false;
        isAbleToFire = true;
    }

    private void Update()
    {
        CastRaycasts();

        //THIS IS TEMPORARY FOR TESTING ONLY
        if (Input.GetKeyDown(KeyCode.Space) && !isAwareOfPlayer)
        {
            isAwareOfPlayer = true;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && isAwareOfPlayer)
        {
            isAwareOfPlayer = false;
        }
    }

    //Note that if the player object happens to get destroyed it will crash the program
    //as this method will have no player to calculate its position.
    //This may become a problem and maybe not.
    private void SetPlayerPosTarget()
    {
        playerPos.x = player.transform.position.x;
        playerPos.y = player.transform.position.y;
    }

    void FixedUpdate()
    {
        CastRaycasts();

        if (isAwareOfPlayer)
        {
            SetPlayerPosTarget();
            AimAtPlayer();

            if (attackRange.isInRange && isAbleToFire)
            {
                AttackPlayer();
            }
        }
    }

    private void AimAtPlayer()
    {
        Vector2 lookDirection = playerPos - enemyRb.position;
        float aimAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 0f;
        enemyRb.rotation = aimAngle;
    }

    private void CastRaycasts()
    {
        rayHit = Physics2D.Raycast(sniperBarrel.transform.position, transform.right, sightRange);
        if(rayHit.collider != null)
        {
            Debug.DrawRay(sniperBarrel.transform.position, rayHit.point, Color.red, 100f, false);
        }
        else
        {
            Debug.DrawRay(sniperBarrel.transform.position, sniperBarrel.transform.position + sniperBarrel.transform.right * sightRange, Color.red, 100f, false);
        }

    }

    private void AttackPlayer()
    {
        audioSource.PlayOneShot(sniperShotSound);

        GameObject bulletClone = Instantiate(bullet, sniperBarrel.transform.position, sniperBarrel.transform.rotation);

        Rigidbody2D bulletCloneRb = bulletClone.GetComponent<Rigidbody2D>();
        bulletCloneRb.AddForce(sniperBarrel.transform.up * bulletSpeed, ForceMode2D.Impulse);

        //Tells the projectile script of the projectile object instantiated that an enemy shot it
        //and its damage
        Projectile bulletCloneScript = bulletClone.GetComponent<Projectile>();
        bulletCloneScript.whoShotMeOutID = (int)WhoShotMeOut.Enemy;

        bulletCloneScript.damage = enemyController.EnemyDamage;

        isAbleToFire = false;
        Coroutine reload = StartCoroutine(BoltBackSniper());
    }

    IEnumerator BoltBackSniper()
    {
        yield return new WaitForSecondsRealtime(timeBetweenShots);
        isAbleToFire = true;
    }

}

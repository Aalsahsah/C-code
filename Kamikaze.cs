using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Kamikaze : MonoBehaviour
{
    //References this enemy's enemy controller script
    private EnemyController enemyController;

    // agent
    private NavMeshAgent enemyAI;

    private AttackRange attackRange;

    [SerializeField] private float fuseTime;

    //Reference to enemy rigidbody
    public Rigidbody2D enemyRb;

    //Player info
    private GameObject player;
    private Vector2 playerPos;

    //Note that this may get taken out and made in a "AwareRange" script later
    public bool isAwareOfPlayer { get; set; }

    SpriteRenderer spriteRenderer;

    [SerializeField] GameObject explosion;

    private bool isDetonating, hasDetonated;


    void Awake()
    {
        enemyAI = GetComponent<NavMeshAgent>();
        //enemyRb = GetComponent<Rigidbody2D>();
        enemyController = GetComponent<EnemyController>();
        attackRange = GetComponentInChildren<AttackRange>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        player = GameObject.FindGameObjectWithTag("Player");

        isAwareOfPlayer = false;
    }

    private void Start()
    {
        //enemyAI.updatePosition = false;
        enemyAI.updateRotation = false;
        enemyAI.updateUpAxis = false;

        isDetonating = false;
    }

    private void Update()
    {
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
        if (attackRange.isInRange && isAwareOfPlayer)
        {
            AttackPlayer();
        }
        else
        {
            if (!isDetonating && isAwareOfPlayer)
            {
                SetPlayerPosTarget();
                MoveTowardsPlayer();
            }
        }
    }

    private void MoveTowardsPlayer()
    {
        enemyAI.SetDestination(playerPos);
    }

    void AttackPlayer()
    {
        isDetonating = true;
        enemyAI.updatePosition = false;
        spriteRenderer.color = Color.red;

        Coroutine countdownFuse = StartCoroutine(CountdownFuse());
    }


    IEnumerator CountdownFuse()
    {
        yield return new WaitForSeconds(fuseTime);
        Explode();
    }

    void Explode()
    {
        explosion.SetActive(true);

        if (attackRange.isInRange && !hasDetonated)
        {
            PlayerData playerData = player.GetComponent<PlayerData>();
            playerData.PlayerTakeDamage(enemyController.EnemyDamage);
        }

        hasDetonated = true;

        Invoke(nameof(DestroyInstance), 1.5f);
    }

    void DestroyInstance()
    {
        Destroy(gameObject);
    }
}

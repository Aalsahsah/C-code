using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTankAI : MonoBehaviour
{
    //References this enemy's enemy controller script
    private EnemyController enemyController;

    // agent
    private NavMeshAgent enemyAI;

    private AttackRange attackRange;

    [SerializeField] private float timeBetweenBombs, speed;

    // flag for when enemy is waiting to attack again
    private bool isAbleToThrow;

    //Reference to enemy rigidbody
    public Rigidbody2D enemyRb;

    //Player info
    private GameObject player;
    private Vector2 playerPos;

    //Note that this may get taken out and made in a "AwareRange" script later
    public bool isAwareOfPlayer { get; set; }


    [Header("Bomb References")]
    [SerializeField] private GameObject bomb, throwTarget;
    [SerializeField] private float throwSpeed;

    public SpriteRenderer enemySprite;

    void Awake()
    {
        enemyAI = GetComponent<NavMeshAgent>();
        //enemyRb = GetComponent<Rigidbody2D>();
        enemySprite = GetComponent<SpriteRenderer>();
        enemyController = GetComponent<EnemyController>();
        attackRange = GetComponentInChildren<AttackRange>();

        player = GameObject.FindGameObjectWithTag("Player");

        isAwareOfPlayer = false;
        isAbleToThrow = true;
    }

    private void Start()
    {
        //enemyAI.updatePosition = false;
        enemyAI.updateRotation = false;
        enemyAI.updateUpAxis = false;
    }

    private void Update()
    {
        //THIS IS TEMPORARY FOR TESTING ONLY
        if (Input.GetKeyDown(KeyCode.Space) && !isAwareOfPlayer)
        {
            isAwareOfPlayer = true;
            print("I'm coming for ya big boy");
        }
        else if (Input.GetKeyDown(KeyCode.Space) && isAwareOfPlayer)
        {
            isAwareOfPlayer = false;
            print("Must have been the wind");
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
        if (isAwareOfPlayer)
        {
            SetPlayerPosTarget();
            AimAtPlayer();

            if (!attackRange.isInRange)
            {
                MoveTowardsPlayer();
            }
            else if (attackRange.isInRange && isAbleToThrow)
            {
                AttackPlayer();
            }
        }
    }

    private void MoveTowardsPlayer()
    {
        enemyAI.SetDestination(playerPos);
    }
    private void AimAtPlayer()
    {
        Vector2 lookDirection = playerPos - enemyRb.position;
        float aimAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 0f;
        enemyRb.rotation = aimAngle;
    }

    private void AttackPlayer()
    {
        GameObject bombClone = Instantiate(bomb, this.transform.position, transform.rotation);
        GameObject throwTargetClone = Instantiate(throwTarget, player.transform.position, transform.rotation);

        //Tells the bomb script of the bomb object instantiated that an enemy threw it,
        //its damage, its target, and its speed to move at
        Bomb bombThrownScript = bombClone.GetComponent<Bomb>();
        bombThrownScript.whoThrewMeID = (int)WhoThrewMe.Enemy;
        bombThrownScript.bombDamage = enemyController.EnemyDamage;
        bombThrownScript.target = throwTargetClone;
        bombThrownScript.bombSpeed = throwSpeed;

        isAbleToThrow = false;
        Coroutine reload = StartCoroutine(ReloadNextThrow());
    }

    IEnumerator ReloadNextThrow()
    {
        yield return new WaitForSecondsRealtime(timeBetweenBombs);
        isAbleToThrow = true;
    }
}
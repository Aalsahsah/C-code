using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    Rigidbody2D playerRb;

    //Stores the data of movement Vectors for movement and mouse position
    private Vector2 moveDirection, mousePos;

    public float playerSpeed;

    [SerializeField] private MainController mainController;


    //Player States
    bool isWalking, isSprinting, isCrouching;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializePlayerWalkState();
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Inputs()
    {
        MovementInput();
        MouseAim();
    }

    private void MovementInput()
    {
        if (mainController.usingController)
        {
            moveDirection.x = Input.GetAxis("Horizontal");
            moveDirection.y = Input.GetAxis("Vertical");
        }
        else
        {
            moveDirection.x = Input.GetAxisRaw("Horizontal");
            moveDirection.y = Input.GetAxisRaw("Vertical");
        }

        Sprint();
        Crouch();

    }

    private void Sprint()
    {
        float sprintMultiplier = 1.6f;

        if (Input.GetKeyDown(KeyCode.LeftShift) && isWalking && !isSprinting)
        {
            isSprinting = true;
            isWalking = false;

            playerSpeed *= sprintMultiplier;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && !isWalking && isSprinting)
        {
            isSprinting = false;
            isWalking = true;

            playerSpeed /= sprintMultiplier;
        }
    }
    private void Crouch()
    {
        float crouchMultiplier = 0.4f;

        if (Input.GetKeyDown(KeyCode.C) && isWalking && !isCrouching)
        {
            isCrouching = true;
            isWalking = false;

            playerSpeed *= crouchMultiplier;
        }
        else if (Input.GetKeyDown(KeyCode.C) && !isWalking && isCrouching)
        {
            isCrouching = false;
            isWalking = true;

            playerSpeed /= crouchMultiplier;
        }
    }

    private void Move()
    {
        playerRb.MovePosition(playerRb.position + moveDirection * playerSpeed * Time.fixedDeltaTime);
    }

    private void MouseAim()
    {
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        Vector2 lookDirection = mousePos - playerRb.position;
        float aimAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 0f;
        playerRb.rotation = aimAngle;
    }


    private void InitializePlayerWalkState()
    {
        isWalking = true;
        isSprinting = false;
        isCrouching = false;
    }
}

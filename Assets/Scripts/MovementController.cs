using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{

    public CharacterController controller;
    public Camera playerFOV;

    public float walkFOV = 80;
    public float speed = 5f;
    public float sprintSpeed = 8.5f;
    public float gravity = -9.81f;
    public float jumpHeight = 5f;
    public bool isScoped;
    float gameSpeedX;
    float gameSpeedY;

    public Transform groundCheck1;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public static MovementController instance;
    public GameManager gameManager;


    Vector3 velocity;
    bool isGrounded;


    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;
    }

    private void Start()
    {
        gameManager = GameManager.instance;
        playerFOV.fieldOfView = walkFOV;
        gameSpeedX = 0;
        gameSpeedY = 0;

    }

    void Update()
    {
        if (gameManager.gameIsPaused)
        {
            return;
        }

        isGrounded = Physics.CheckSphere(groundCheck1.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0) 
        {
            velocity.y = -10f;
        }

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        if (x != 0f) 
        {
            gameSpeedX = Mathf.Lerp(gameSpeedX, speed, 3f * Time.deltaTime);
        } else
        {
            gameSpeedX = Mathf.Lerp(gameSpeedX, 0, 3f * Time.deltaTime);
        }

        if (z != 0f)
        {
            gameSpeedY = Mathf.Lerp(gameSpeedY, speed, 3f * Time.deltaTime);
        }
        else
        {
            gameSpeedY = Mathf.Lerp(gameSpeedY, 0, 3f * Time.deltaTime);
        }

        Vector3 moveX = Vector3.Normalize(transform.right * x);
        Vector3 moveY = Vector3.Normalize(transform.forward * z);

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetAxisRaw("Vertical") != 0 && !isScoped)
        {
            gameSpeedY = sprintSpeed;
            playerFOV.fieldOfView = Mathf.Lerp(playerFOV.fieldOfView, walkFOV + (walkFOV * 0.15f), 15f * Time.deltaTime); 
        } else if (!isScoped)
        {
            playerFOV.fieldOfView = Mathf.Lerp(playerFOV.fieldOfView, walkFOV, 15f * Time.deltaTime);
        }

        controller.Move(moveX * gameSpeedX * Time.deltaTime);
        controller.Move(moveY * gameSpeedY * Time.deltaTime);

        if(Input.GetButton("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}

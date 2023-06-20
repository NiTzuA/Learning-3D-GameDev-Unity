using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{

    public CharacterController controller;
    public Camera playerFOV;

    public float walkFOV = 80;
    public float speed = 10f;
    public float sprintSpeed = 8.5f;
    public float gravity = -9.81f;
    public float jumpHeight = 5f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    private void Start()
    {
        playerFOV.fieldOfView = walkFOV;
    }

    void Update()
    {

        float gameSpeed = speed;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0) 
        {
            velocity.y = -1f;
        }

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 move = Vector3.Normalize(transform.right * x + transform.forward * z);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            gameSpeed = sprintSpeed;
            playerFOV.fieldOfView = Mathf.Lerp(playerFOV.fieldOfView, walkFOV + (walkFOV * 0.15f), 15f * Time.deltaTime); //walkFOV + (walkFOV * 0.15f);
        } else
        {
            playerFOV.fieldOfView = Mathf.Lerp(playerFOV.fieldOfView, walkFOV, 15f * Time.deltaTime);
        }

        controller.Move(move * gameSpeed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}

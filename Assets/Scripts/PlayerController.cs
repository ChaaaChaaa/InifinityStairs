using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody playerRigidBody;

    public float jumpSpeed;
    public float moveSpeed;
    //private float activeMoveSpeed;

    public bool canMove;
    
    public bool isGround;
    
    // public Transform groundCheck;
    // public float groundCheckRadius;
    // public LayerMask whatIsGround;

    public Animator animator;

    //public AudioSource jumpSound;

    private bool onPlatform;
    public float onPlatformSpeedModifier;
    
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        animator.SetBool("Die",false);
        //activeMoveSpeed = moveSpeed;
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKey(KeyCode.LeftArrow))
        // {
        //     playerRigidBody.AddForce(-speed,0,0);
        // }
        //
        // else if (Input.GetKey(KeyCode.RightArrow))
        // {
        //     playerRigidBody.AddForce(speed,0,0);
        // }
        //
        // else if (Input.GetKey(KeyCode.UpArrow))
        // {
        //     playerRigidBody.AddForce(0,0,speed);
        // }
        //
        // else if (Input.GetKey(KeyCode.DownArrow))
        // {
        //     playerRigidBody.AddForce(0,0,-speed);
        // }

        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        float fallSpeed = playerRigidBody.velocity.y;

        Vector3 velocity = new Vector3(inputX, 0, inputZ);
        velocity *= speed;
        velocity.y = fallSpeed;
        playerRigidBody.velocity = velocity;

        //isGround = Physics.OverlapCapsule(groundCheck.position, groundCheck.position, groundCheckRadius, whatIsGround);

        if (canMove)
        {
            if (onPlatform)
            {
                moveSpeed = moveSpeed*onPlatformSpeedModifier;

            }
            
            if (Input.GetAxis("Horizontal") > 0f)
            {
                Debug.Log("테스트");
                playerRigidBody.velocity = new Vector3(moveSpeed, playerRigidBody.velocity.y, 0f);
                animator.Play("Move_L",-1,0);
                //transform.localScale = new Vector3(1f, 1f, 1f);
            }
            
            else if (Input.GetAxis("Horizontal") < 0f)
            {
                playerRigidBody.velocity = new Vector3(-moveSpeed, playerRigidBody.velocity.y, 0f);
                animator.Play("Move_R",-1,0);
                //transform.localScale = new Vector3(-1f, 1f, 1f);
            }

            else
            {
                playerRigidBody.velocity = new Vector3(0f, playerRigidBody.velocity.y, 0f);
            }

            if (Input.GetButtonDown("Jump") && isGround)
            {
                playerRigidBody.velocity = new Vector3(playerRigidBody.velocity.x, jumpSpeed, 0f);
                animator.Play("Jump",-1,0);
            }
        }
        // else
        // {
        //     animator.Play("Die2",-1,0);
        // }
        
        animator.SetFloat("Speed",Mathf.Abs(playerRigidBody.velocity.x));
        animator.SetBool("Grounded", isGround);
    }

    public void Die()
    {
        //Debug.Log("Game Over3"+animator.GetBool("Die"));
        animator.SetBool("Die",true);
        //animator.Play("Die2",-1,0);
        canMove = false;
    }
}

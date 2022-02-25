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

    public Animator animator;

    //public AudioSource jumpSound;

    private bool onPlatform;
    public float onPlatformSpeedModifier;

    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        //activeMoveSpeed = moveSpeed;
        animator.SetBool("isDie", false);
        //animator.Play("Idle",-1,0);
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
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
            //animator.Play("Walk",0,0);

            if (onPlatform)
            {
                moveSpeed = moveSpeed * onPlatformSpeedModifier;
            }

            if (Input.GetAxis("Horizontal") > 0f)
            {
                playerRigidBody.velocity = new Vector3(moveSpeed, playerRigidBody.velocity.y, 0f);
                animator.Play("Move_L", -1, 0f);
                //transform.localScale = new Vector3(1f, 1f, 1f);
            }

            else if (Input.GetAxis("Horizontal") < 0f)
            {
                playerRigidBody.velocity = new Vector3(-moveSpeed, playerRigidBody.velocity.y, 0f);
                animator.Play("Move_R", -1, 0f);
                //transform.localScale = new Vector3(-1f, 1f, 1f);
            }

            else
            {
                playerRigidBody.velocity = new Vector3(0f, playerRigidBody.velocity.y, 0f);
            }
        }
    }

    public void InitPlayer()
    {
        animator = GetComponent<Animator>();
        animator.Play("Idle", -1, 0);
        animator.SetBool("isDie", false);
        canMove = true;
    }

    public void Die()
    {
        //Debug.Log("테스트");
        //Debug.Log("Game Over3"+animator.GetBool("Die"));
        animator.SetBool("isDie", true);
        animator.Play("Jump", -1, 100);
        canMove = false;
    }
}
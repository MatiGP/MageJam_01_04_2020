﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] float jumpTime;
    [SerializeField] Transform legsPosition;
    [SerializeField] LayerMask groundMask;
    [SerializeField] int bounceAddForce = 2;
    [SerializeField] int bounceMaxForce = 14;
    [SerializeField] AudioSource jumpSource;
    [SerializeField] ParticleSystem runningParticle;
    [SerializeField] ParticleSystem jumpParticle;
    int bounceForce = 3;

    float horizontal;
    float jumpTimeCounter;


    public bool canJump;
    bool dialogueOpen = false;
    bool canMove = true;
    bool isJumping = false;
    bool isDucking;
    bool isSpellcrafting;
    bool isCarrying = false;
    bool isSwinging = false;

    Rigidbody2D rb2d;
    Animator animator;
    CapsuleCollider2D capsuleCollider;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove && !isDucking)
        {
            horizontal = Input.GetAxisRaw("Horizontal");       
        }
        else
        {
            horizontal = 0;
        }

        if(horizontal == 0)
        {
            runningParticle.gameObject.SetActive(false);
        }
        else
        {
            runningParticle.gameObject.SetActive(true);
        }


        ChangeScale();


        if (canJump) bounceForce = 3;

        if (Input.GetKeyDown(KeyCode.Space) && canJump && !dialogueOpen && !isSpellcrafting)
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            Jump();
            animator.SetTrigger("jump");
            jumpSource.Play();
        }

        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                Jump();
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }


        if (Input.GetKeyDown(KeyCode.S) && !isSpellcrafting && !isDucking)
        {
            isDucking = true;
            Duck();
        }

        if (Input.GetKeyUp(KeyCode.S) && isDucking)
        {
            isDucking = false;
            StopDuck();
        }


        UpdateAnimator();
    }

    void Jump()
    {
        rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
        jumpParticle.Play();
    }

    private void FixedUpdate()
    {
        canJump = Physics2D.OverlapCircle(legsPosition.position, 0.4f, groundMask);

        rb2d.velocity = new Vector2(horizontal * speed, rb2d.velocity.y);
    }

    void UpdateAnimator()
    {
        bool isRunning = horizontal != 0 ? true : false;
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isInAir", !canJump);
        animator.SetBool("isDucking", isDucking);
        animator.SetBool("isSpellCrafting", isSpellcrafting);
        animator.SetBool("isSwinging", isSwinging);
    }

    void ChangeScale()
    {

        if (horizontal > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (horizontal < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }


    }

    public void Bounce()
    {
        if (bounceForce < bounceMaxForce) bounceForce += bounceAddForce;

        rb2d.velocity = new Vector2(rb2d.velocity.x, bounceForce);        
    }

    public void Bounce(float bounceForce)
    {
        rb2d.velocity = new Vector2(rb2d.velocity.x, bounceForce);
    }

    public void DisableMovement()
    {
        canMove = false;
    }

    public void EnableMovement()
    {
        canMove = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Platform")
        {
            transform.parent = collision.gameObject.transform;
            if (horizontal > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (horizontal < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Platform")
        {
            transform.parent = null;
        }
    }

    public void SpellCraft(bool value)
    {
        isSpellcrafting = value;
    }

    void Duck()
    {
        capsuleCollider.offset = new Vector2(-0.03f, -0.35f);
        capsuleCollider.size = new Vector2(0.36f, 0.48f);
    }

    void StopDuck()
    {
        capsuleCollider.offset = new Vector2(-0.03f, -0.06f);
        capsuleCollider.size = new Vector2(0.36f, 1.18f);
    }

    public void StopSwinging()
    {
        transform.parent = null;
        rb2d.isKinematic = false;
        EnableMovement();
        transform.rotation = Quaternion.Euler(0, 0, 0);
        GetComponent<CapsuleCollider2D>().isTrigger = false;
        isSwinging = false;
        Jump();
    }

    public void DisableJumpingDialogOpen()
    {
        dialogueOpen = true;
    }

    public void EnableJumpingDialogClose()
    {
        dialogueOpen = false;
    }

    public void SetSwing()
    {
        GetComponent<CapsuleCollider2D>().isTrigger = true;
        isSwinging = true;
        rb2d.velocity = Vector2.zero;
        rb2d.isKinematic = true;
        DisableMovement();
    }

    public bool GetDialogOpen()
    {
        return dialogueOpen;
    }

    private void OnDisable()
    {             
        isJumping = false;
        isDucking = false;
        isSpellcrafting = false;
        StopSwinging();
        StopDuck();
    }





}

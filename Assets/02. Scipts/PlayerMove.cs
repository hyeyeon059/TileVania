using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float jumpPower = 25f;
    [SerializeField] private float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(20f, 20f);
    [SerializeField] GameObject bulletPre;
    [SerializeField] Transform gun;

    Vector2 moveInput;
    float gravityScaleAtStart;
    bool isAlive = true;

    Rigidbody2D rb;
    Animator animator;
    CapsuleCollider2D capsuleCollider;
    BoxCollider2D boxCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = rb.gravityScale;
    }

    void Update()
    {
        if (!isAlive) return;

        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    private void OnMove(InputValue value)
    {
        if (!isAlive) return;
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) return;

        if (!boxCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) return;

        if (value.isPressed)
        {
            rb.velocity += new Vector2(0f, jumpPower);
        }

    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, rb.velocity.y);
        rb.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
        animator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
        // Epsilon은 0에 가장 가까운 숫자

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
            // Sign 함수가 0이거나 양수이면 1로 반환
        }
    }

    void ClimbLadder()
    {
        if (!boxCollider.IsTouchingLayers(LayerMask.GetMask("Climb")))
        {
            rb.gravityScale = gravityScaleAtStart;
            animator.SetBool("isClimbing", false);
            return;
        }

        Vector2 climbVelocity = new Vector2(rb.velocity.x, moveInput.y * climbSpeed);
        rb.velocity = climbVelocity;
        rb.gravityScale = 0;
        
        bool playerHasVerticalSpeed = Mathf.Abs(rb.velocity.y) > Mathf.Epsilon;
        animator.SetBool("isClimbing", playerHasVerticalSpeed);
    }

    void Die()
    {
        if (capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")))
        {
            isAlive = false;
            animator.SetTrigger("Dying");
            rb.velocity = deathKick;
        }

        if (capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Hazard")))
        {
            isAlive = false;
            animator.SetTrigger("Dying");
        }
    }

    void OnFire(InputValue value)
    {
        if (!isAlive) return;
        Instantiate(bulletPre, gun.position, transform.rotation);
    }
}
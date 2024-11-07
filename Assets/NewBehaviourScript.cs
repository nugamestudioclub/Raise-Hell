using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jump;
    private Rigidbody2D body;
    private TrailRenderer trail;
    private BoxCollider2D collider;
    [SerializeField] private LayerMask groundlayer;

    [SerializeField] private LayerMask walllayer;
    private float wallJumpCooldown;
    private float airjump;
    private bool isSliding;
    [SerializeField] private float wallSlidingSpeed;
    [SerializeField] private float wallJumpDuration;
    [SerializeField] private Vector2 wallJumpForce;
    private bool wallJumping;

    [SerializeField] private float dashVelocity = 14f;
    [SerializeField] private float dashTime = 0.5f;
    [SerializeField] private float dashCoolDown = 0.5f;
    private Vector2 dashDir;
    private bool isDashing;
    private bool canDash = true;


    private void Awake() //Runs when you load the Script
    {

        body = GetComponent<Rigidbody2D>(); //Accesses rigid body component
        collider = GetComponent<BoxCollider2D>();
        trail = GetComponent<TrailRenderer>();

    }

    private void Update() //Runs every frame
    {

        float inputX = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(inputX * speed, body.velocity.y);
        bool dashInput = Input.GetButtonDown("Dash");

        flipMethod();
        jumpMethod();
        wallJumpMethod();

        if (dashInput)
        {
            dashAbility();
        }

        Debug.Log(canDash);
    }



    private void flipMethod()
    {
        float inputX = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(inputX * speed, body.velocity.y);

        if (inputX > 0.01f)
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        else if (inputX < -0.0f)
        {
            transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
        }
    }

    private void jumpMethod()
    {
        float inputX = Input.GetAxis("Horizontal");
        bool jumpInput = Input.GetButtonDown("Jump");
        bool jumpRelease = Input.GetButtonUp("Jump");


        body.velocity = new Vector2(inputX * speed, body.velocity.y);


        if (jumpInput && isGrounded())
        {
            airjump = 1;
            body.velocity = new Vector2(body.velocity.x, jump);
        }
        if (jumpInput && isSliding)
        {
            wallJumping = true;
            Invoke("StopWallJump", wallJumpDuration);
        }

        if (jumpRelease && body.velocity.y > 0)
        {
            body.velocity = new Vector2(body.velocity.x, 0);
        }
    }

    private void wallJumpMethod()
    {
        float inputX = Input.GetAxis("Horizontal");
        if (onWall() && !isGrounded())
        {
            isSliding = true;
        }
        else
        {
            isSliding = false;
        }

        if (isSliding)
        {
            body.velocity = new Vector2(body.velocity.x, -wallSlidingSpeed);
        }

        if (wallJumping)
        {
            body.velocity = new Vector2(-inputX * wallJumpForce.x, wallJumpForce.y);
        }
        else
        {
            body.velocity = new Vector2(inputX * speed, body.velocity.y);
        }
    }

    private void dashMethod()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        bool dashInput = Input.GetButtonDown("Dash");

        if (canDash)
        {
            isDashing = true;
            canDash = false;
            trail.emitting = true;
            dashDir = new Vector2(inputX, inputY);

            if (dashDir == Vector2.zero)
            {
                dashDir = new Vector2(transform.localScale.x, y: 0);
            }
        }

        if (isDashing)
        {
            body.velocity = dashDir.normalized * dashVelocity;
        }

        if (isGrounded())
        {
            canDash = true;
        }

        StartCoroutine(routine: stopDashing());
    }

    private void dashAbility()
    {
        if (canDash && isGrounded())
        {
            StartCoroutine(routine: groundDash());
        }

        if (canDash && !isGrounded())
        {
            StartCoroutine(routine: airDash());
        }
    }



    private IEnumerator groundDash()
    {
        canDash = false;
        float s = speed;
        speed = dashVelocity;
        trail.emitting = true;

        yield return new WaitForSeconds(dashTime);

        speed = s;
        trail.emitting = false;

        yield return new WaitForSeconds(dashCoolDown);

        canDash = true;
    }

    private IEnumerator airDash()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        canDash = false;
        Vector2 bv = body.velocity;
        dashDir = new Vector2(inputX, inputY);
        body.velocity = dashDir.normalized * dashVelocity;

        yield return new WaitForSeconds(dashTime);

        body.velocity = bv; ;
        yield return new WaitUntil(() => isGrounded());
        canDash = true;
    }

    private void StopWallJump()
    {
        wallJumping = false;
    }


    private IEnumerator stopDashing()
    {
        yield return new WaitForSeconds(dashTime);
        trail.emitting = false;
        isDashing = false;
    }


    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0, Vector2.down, 0.1f, groundlayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, walllayer);
        return raycastHit.collider != null;
    }
}

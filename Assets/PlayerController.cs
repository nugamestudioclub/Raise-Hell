using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Class for controlling the player's movement
 */
public class PlayerController : MonoBehaviour
{
    /*
     * Fields of the player used to manipulate and configure the movement
     */
    [SerializeField]
    private float jumpPower = 10;
    [SerializeField]
    private float moveSpeed = 5;
    private Rigidbody2D rigidbody2d;
    private bool isGrounded;
    
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        //gets the rigidbody that we will use for moving the player
        rigidbody2d = GetComponent<Rigidbody2D>();
        isGrounded = false;
        if(GetComponent<Animator>() != null) 
            animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) //jump
        {
            Jump();
        }
        if (Input.GetKey(KeyCode.A)) //move left
        {
            rigidbody2d.AddForce(moveSpeed * Time.deltaTime * Vector2.left, ForceMode2D.Impulse);
            if (animator)
                animator.GetComponent<SpriteRenderer>().flipX = true;
        }
        if (Input.GetKey(KeyCode.D)) //move right
        {
            rigidbody2d.AddForce(moveSpeed * Time.deltaTime * Vector2.right, ForceMode2D.Impulse);

            if(animator)
                animator.GetComponent<SpriteRenderer>().flipX = false;
        }

        if (animator)
        {
            float dir = rigidbody2d.velocity.x;
            string anim = isGrounded ? (Mathf.Abs(dir) > 0.1 ? "player_running" : "player_idle") : "player_falling";

            animator.Play(anim);
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("onCollisionEnter2D called...");
        //saving current movement in the y direction
        float yMovement = rigidbody2d.velocity.y;

        //if the player is not moving in the y direction
        if (collision.transform.position.y < transform.position.y && yMovement < 0.01f)
        {
            isGrounded = true; //player is grounded
        }       

        if(collision.collider.tag == "Finish" && isGrounded) {
            Debug.Log("Finish");
            SceneManager.LoadScene("Level2");
        }
    }

    /*
     * Gets called When we stay collided with another collider
     */
    private void OnCollisionStay2D(Collision2D collision)
    {
         Debug.Log("onCollisionStay2D called...");
        //saving current movement in the y direction
        float yMovement = rigidbody2d.velocity.y;

        //if the player is not moving in the y direction
        if (collision.transform.position.y < transform.position.y && yMovement < 0.01f)
        {
            isGrounded = true; //player is grounded
        }
    }

    /*
     * Gets called when we stop colliding with another collider
     */
    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("onCollisionExit2D called...");

        float yMovement = rigidbody2d.velocity.y;
        if (Mathf.Abs(yMovement) > Mathf.Epsilon)
        {
            isGrounded = false;
        }
    }

    /*
     * Makes the player get a boost of velocity in the y direction
     */
    private void Jump()
    {
        Debug.Log("Jump called..");

        Vector2 oldVelocity = rigidbody2d.velocity;
        Vector2 newVelocity = new Vector2(oldVelocity.x, jumpPower);
        rigidbody2d.velocity = newVelocity;
    }

    /*
     * When entering a collider that has IsTrigger checked
     */
    
}

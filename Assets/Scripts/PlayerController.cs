using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    public float speed;
    public float jumpForce;

    private Vector2 movement;
    private Rigidbody2D playerRb;
    private bool isGrounded;
    bool facingRight = true;


    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            if (isGrounded)
            {
                playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                isGrounded = false;
            }
        }
        playerRb.velocity = new Vector2(movement.x * speed, playerRb.velocity.y);
        Debug.Log(movement.x);

        if (movement.x < 0 && facingRight)
        {
            flip();
        }
        else if(movement.x > 0 && !facingRight)
        {
            flip();
        }

        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //check for ground collision.
        isGrounded = true;
    }

    void flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f,180f,0f);
    }
}

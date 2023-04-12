using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public float dashForce;
    public float dashDuration;
    private bool isJumping;
    private bool isDashing;
    private float dashTimer;
    public InputAction playerControls;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isJumping = true;
        }

        if (Input.GetButtonDown("Dash") && !isDashing)
        {
            isDashing = true;
            dashTimer = 0f;
            rb.velocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = true;
        }
    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        if (!isDashing)
        {
            Vector2 movement = new Vector2(moveHorizontal * speed, rb.velocity.y);
            rb.velocity = movement;
        }
        else
        {
            if (dashTimer < dashDuration)
            {
                float dashDirection = Mathf.Sign(moveHorizontal);
                rb.AddForce(new Vector2(dashDirection * dashForce, 0f), ForceMode2D.Impulse);
                dashTimer += Time.fixedDeltaTime;
            }
            else
            {
                isDashing = false;
            }
        }
    }
}



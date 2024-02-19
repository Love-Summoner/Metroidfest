using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] public Rigidbody2D PlayerBody;
    [SerializeField] public Transform groundCheck;
    [SerializeField] public LayerMask groundLayer;

    public float speed;
    public float move;
    public float jumpingPower;
    public bool grounded;
    private bool isFacingRight = true;


    // Update is called once per frame.
    void Update()
    {
        move = Input.GetAxisRaw("Horizontal");
        PlayerBody.velocity = new Vector2(move * speed, PlayerBody.velocity.y);
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded()) {
            PlayerBody.velocity += Vector2.up * jumpingPower;
        }
        if (Input.GetKeyUp(KeyCode.Space) && PlayerBody.velocity.y > 0)
        {
            PlayerBody.velocity = new Vector2(PlayerBody.velocity.x, PlayerBody.velocity.y * 0.5f);
        }
        Flip();
    }
    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && move > 0f || !isFacingRight && move < 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
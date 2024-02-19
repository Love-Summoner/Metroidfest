using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] public Rigidbody2D PlayerBody;
    [SerializeField] public Transform groundCheck;
    [SerializeField] public LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;

    public float speed;
    public float move;
    public float jumpingPower;
    public bool grounded;
    private bool isFacingRight = true;

    //Dashing values
    public float dashStrength;
    public float dashingTime;
    public float dashingCooldown;
    private bool canDash = true;
    private bool isDashing =  false;


    // Update is called once per frame.
    void Update()
    {
        if (isDashing)
        {
            return;
        }
        move = Input.GetAxisRaw("Horizontal");
        PlayerBody.velocity = new Vector2(move * speed, PlayerBody.velocity.y);
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded()) {
            PlayerBody.velocity += Vector2.up * jumpingPower;
        }
        if (Input.GetKeyUp(KeyCode.Space) && PlayerBody.velocity.y > 0)
        {
            PlayerBody.velocity = new Vector2(PlayerBody.velocity.x, PlayerBody.velocity.y * 0.5f);
        }
        if (Input.GetKeyDown(KeyCode.Q) && canDash)
        {
            StartCoroutine(Dash());
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
            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = PlayerBody.gravityScale;
        PlayerBody.gravityScale = 0f;
        PlayerBody.velocity = new Vector2(transform.localScale.x * -dashStrength, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        isDashing = false;
        PlayerBody.gravityScale = originalGravity;
        tr.emitting = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
        
    }
}
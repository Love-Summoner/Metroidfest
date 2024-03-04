using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] public Rigidbody2D PlayerBody;
    [SerializeField] public Transform groundCheck;
    [SerializeField] public LayerMask groundLayer;
    [SerializeField] public LayerMask attackLayer;
    [SerializeField] private LayerMask deathLayer;
    [SerializeField] private LayerMask smoke_layer;
    [SerializeField] private BoxCollider2D hurtbox;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private SpriteRenderer sr;

    public float speed;
    public float move;
    public float jumpingPower;
    public bool grounded;
    private bool isFacingRight = false;

    //Dashing values
    public float dashStrength;
    public float dashingTime;
    public float dashingCooldown;
    private bool canDash = true;
    private bool isDashing =  false;

    //Invisibility values
    public bool Invis = false;
    public bool canInvis = true;
    public float invisCool = 5f;
    public float invisTime = 1f;

    public bool obstructed = false;

    //health values

    public bool Invinc = false;
    public float invincTime = 1f;
    public int maxHealth = 5;
    public int health = 5;

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
        if(Input.GetKeyDown(KeyCode.E) && canInvis)
        {
            StartCoroutine(Invisibility());
        }

        if (hurtbox.IsTouchingLayers(smoke_layer))
        {
            obstructed = true;
        }
        else
        {
            obstructed = false;
        }
        if (hurtbox.IsTouchingLayers(attackLayer))
        {
            StartCoroutine(invincible());
            if (!Invinc)
            {
                health -= 1;
                Invinc = true;
            }
            if (health < 1)
            {
                Death();
            }
        }
        if (hurtbox.IsTouchingLayers(deathLayer))
        {
            Death();
        }

        Flip();
    }
    private void Death()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator invincible()
    {
        yield return new WaitForSeconds(invincTime);
        Invinc = false;
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
    
    private IEnumerator Invisibility()
    {
        canInvis = false;
        Invis = true;
        sr.color = new Color(0f, 0f, 0f, .5f);
        yield return new WaitForSeconds(invisTime);
        Invis = false;
        sr.color = new Color(0f, 0f, 0f, 1f);
        yield return new WaitForSeconds(invisCool);
        canInvis = true;
    }
    
    
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = PlayerBody.gravityScale;
        PlayerBody.gravityScale = 0f;
        PlayerBody.velocity = new Vector2(transform.localScale.x * dashStrength, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        isDashing = false;
        PlayerBody.gravityScale = originalGravity;
        tr.emitting = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
        
    }
}
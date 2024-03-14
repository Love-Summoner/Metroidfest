using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour, IDataPersistence
{
    [SerializeField] private Rigidbody2D PlayerBody;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask attackLayer;
    [SerializeField] private LayerMask deathLayer;
    [SerializeField] private LayerMask checkpointLayer;
    [SerializeField] private LayerMask smoke_layer;
    [SerializeField] private BoxCollider2D hurtbox;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Walk_code_player anims;

    public DataPersistenceManager dpm;

    //[SerializeField] private SmokeScreen SmokeScript;
    [SerializeField] private GameObject smoke;


    public float speed;
    public float move;
    public float jumpingPower;
    public bool grounded;
    private bool isFacingRight = false;

    public bool _hasDash = false;
    public bool _hasInvis = false;
    public bool _hasSmoke = false;

    //Dashing values
    public float dashStrength;
    public float dashingTime;
    public float dashingCooldown;
    public bool canDash = false;
    private bool isDashing =  false;

    //Invisibility values
    public bool Invis = false;
    public bool canInvis = false;
    public float invisCool = 5f;
    public float invisTime = 1f;

    //Smoke values
    public bool canSmoke = false;
    public float smokeTime = 3f;
    public float smokeCool = 10f;
    public bool obstructed = false;

    //health values

    public bool Invinc = false;
    public float invincTime = 1f;
    public int maxHealth = 5;
    public int health = 5;

    private Vector3 respawnPoint;
    private int progress;
    private float respawnx;
    private float respawny;

    //for detecting if the player is seen
    private bool seen = false;

    // Update is called once per frame.
    void Start()
    {
        respawnPoint = transform.position;
    }
    void Update()
    {
        if(Invis)
        {
            gameObject.layer = 0;
        }
        else
            gameObject.layer = 7;
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
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
        if(Input.GetKeyDown(KeyCode.Q) && canInvis && !seen)
        {
            StartCoroutine(Invisibility());
        }
        if (Input.GetKeyDown(KeyCode.E) && canSmoke)
        {
            StartCoroutine(Smokescreen());
        }

        if (hurtbox.IsTouchingLayers(smoke_layer))
        {
            obstructed = true;
        }
        else
        {
            obstructed = false;
        }
        /*if (hurtbox.IsTouchingLayers(attackLayer))
        {
            /*
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
            takeDamage();

        }*/
        if (hurtbox.IsTouchingLayers(checkpointLayer))
        {
            respawnPoint = transform.position;
            respawnx = respawnPoint.x;
            respawny = respawnPoint.y;
            health = maxHealth;
            dpm.SaveGame();
            //save

        }
        else if (hurtbox.IsTouchingLayers(deathLayer))
        {
            transform.position = respawnPoint;
        }


        Flip();
    }
    private void Death()
    {
        gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator invincible()
    {
        yield return new WaitForSeconds(invincTime);
        Invinc = false;
    }

    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
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
        sr.color = new Color(1f, 1f, 1f, .5f);
        yield return new WaitForSeconds(invisTime);
        Invis = false;
        sr.color = new Color(1f, 1f, 1f, 1f);
        yield return new WaitForSeconds(invisCool);
        canInvis = true;
    }
    
    
    private IEnumerator Dash()
    {
        anims.dash_anim(dashingTime);
        canDash = false;
        isDashing = true;
        float originalGravity = PlayerBody.gravityScale;
        PlayerBody.gravityScale = 0f;
        PlayerBody.velocity = new Vector2(transform.localScale.x * dashStrength, 0f);
        //tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        isDashing = false;
        PlayerBody.gravityScale = originalGravity;
        //tr.emitting = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
        
    }

    private IEnumerator Smokescreen()
    {
        smoke.transform.position = PlayerBody.transform.position;
        canSmoke = false;
        smoke.SetActive(true);
        yield return new WaitForSeconds(smokeTime);
        smoke.SetActive(false);
        yield return new WaitForSeconds(smokeCool);
        canSmoke = true;


    }

    public IEnumerator noticed()
    {
        seen = true;
        yield return new WaitForSeconds(1);
        seen = false;
    }

    public bool is_seen()
    {
        return seen;
    }

    public void getDash()
    {
        _hasDash = true;
        canDash = true;
    }

    public void getInvis()
    {
        _hasInvis = true;
        canInvis = true;

    }

    public void getSmoke()
    {
        _hasSmoke = true;
        canSmoke = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Dash"))
        {
            getDash();
            Destroy(other.gameObject);
            progress = 1;
        }else if (other.gameObject.CompareTag("Invis"))
        {
            getInvis();
            Destroy(other.gameObject);
            progress = 2;
        }else if (other.gameObject.CompareTag("Smoke"))
        {
            getSmoke();
            Destroy(other.gameObject);
            progress = 3;
        }else if (other.gameObject.layer == 9) //enemy attack
        {
            /*float deltax = transform.position.x - other.transform.position.x;
            if (deltax < 0)
            {
                PlayerBody.velocity = new Vector2(-speed*20, 0);
            }
            else
            {
                PlayerBody.velocity = new Vector2(speed*20, 0);
            }*/
            takeDamage();
            StartCoroutine(flashhit());
        }
    }
    
    public IEnumerator flashhit()
    {
        Color playercolor = sr.color;
        sr.color = Color.red;
        yield return new WaitForSeconds(.5f);
        sr.color = playercolor;
    }

    public void takeDamage()
    {
        StartCoroutine(invincible());
        if (!Invinc)
        {
            Invinc = true;
            health -= 1;

        }
        if (health < 1)
        {
            Death();
        }
    }
    public void LoadData(GameData data)
    {
        this.progress = data.progress;
        if (progress >= 1)
        {
            getDash();
        }
        if (progress >= 2)
        {
            getInvis();
        }
        if (progress == 3)
        {
            getSmoke();
        }
        this.respawnPoint = new Vector3(data.respawnx, data.respawny, 0);
        transform.position = new Vector3(data.respawnx, data.respawny, 0);
    }

    public void SaveData(ref GameData data)
    {
        data.progress = this.progress;
        data.respawnx = this.respawnx;
        data.respawny = this.respawny;
    }
}
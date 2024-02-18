using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Rigidbody2D PlayerBody;
    public float speed;
    public float move;
    public bool grounded;
    

    // Update is called once per frame
    void Update()
    {
        move = Input.GetAxisRaw("Horizontal");
        PlayerBody.velocity = new Vector2(move * speed, PlayerBody.velocity.y);
        if (Input.GetKeyDown(KeyCode.Space) && grounded){
            PlayerBody.velocity += Vector2.up * 5;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground")) {
            Vector3 normal = collision.GetContact(0).normal;
            if(normal == Vector3.up)
            {
                grounded = true;
            }
        }   
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            grounded = false;
        }
    }
}

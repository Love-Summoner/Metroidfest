using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk_code_player : MonoBehaviour
{
    // Start is called before the first frame update
    // Update is called once per frame
    [SerializeField] private Animator walk;
    [SerializeField] private Transform groundcheck;
    [SerializeField] private LayerMask groundLayer;

    void Update()
    {
        if(Input.GetAxisRaw("Horizontal") == 0 || !isGrounded())
            walk.enabled = false;
        else
            walk.enabled = true;
    }
    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundcheck.position, 0.2f, groundLayer);
    }
}

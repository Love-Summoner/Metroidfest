using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk_code_player : MonoBehaviour
{
    // Start is called before the first frame update
    // Update is called once per frame
    [SerializeField] private Animator animation_control;
    [SerializeField] private Transform groundcheck;
    [SerializeField] private LayerMask groundLayer;
    private bool stop;
    private IEnumerator wait;

    void Update()
    {
        if (stop)
            return;

        if (animation_control != null)
        {
            if (Input.GetAxisRaw("Horizontal") == 0 || !isGrounded())
                animation_control.Play("Idle");
            else
                animation_control.Play("Walk");
        }
    }
    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundcheck.position, 0.2f, groundLayer);
    }
    public void attack_anim()
    {
        animation_control.Play("Attack");
        wait = Interrupt(.4f);
        StartCoroutine(wait);
    }

    private IEnumerator Interrupt(float time)
    {
        stop = true;
        yield return new WaitForSeconds(time);
        stop = false;
    }
}

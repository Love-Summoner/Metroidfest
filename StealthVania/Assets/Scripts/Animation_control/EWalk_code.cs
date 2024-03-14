using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EWalk_code : MonoBehaviour
{
    // Start is called before the first frame update
    // Update is called once per frame
    [SerializeField] private  Rigidbody2D body;
    [SerializeField] private Animator animation_control;

    private bool is_attacking;
    private IEnumerator wait;

    void Update()
    {
        if (is_attacking)
            return;

        if (animation_control != null)
        {
            if (Mathf.Abs(body.velocity.x) > 0 && body.velocity.y > -.1f)
                animation_control.Play("Walk");
            else
                animation_control.Play("Idle");
        }
    }
    public void attack_anim()
    {
        animation_control.Play("Attack");
        wait = Interrupt(.4f);
        StartCoroutine(wait);
    }

    private IEnumerator Interrupt(float time)
    {
        is_attacking = true;
        yield return new WaitForSeconds(time);
        is_attacking = false;
    }
}

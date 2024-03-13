using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EWalk_code : MonoBehaviour
{
    // Start is called before the first frame update
    // Update is called once per frame
    [SerializeField] private Animator walk;
    [SerializeField] private  Rigidbody2D body;

    void Update()
    {
        if (Mathf.Abs(body.velocity.x) > .1f && body.velocity.y == 0 )
            walk.enabled = true;
        else
            walk.enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    enum State { 
        IDLE, 
        CHASE, 
        ATTACK
    }
    enum Type { 
        RANGE, 
        MELEE
    }
    
    private State state = State.IDLE;
    [SerializeField] private Type type;
    [SerializeField] private float path_point1, path_point2;
    [SerializeField] private Sight sight;
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D body;

    private bool has_path;
    private bool walk_back = false;

    private void Start()
    {
        if(path_point1 == 0 &&  path_point2 == 0)
        {
            has_path = false;
        }
        else
            has_path = true;
    }
    void Update()
    {
        if(body.velocity != Vector2.zero)
            Flip();

        switch (type)
        {
            case Type.RANGE:
                rangedAI();
                break;
            case Type.MELEE:
                meleeAI();
                break;
        }
    }
    private void rangedAI()
    {
        switch (state)
        {
            case State.IDLE:
                idle();
                break;
            case State.CHASE:
                break;
        }
    }
    private void meleeAI()
    {
        switch (state)
        {
            case State.IDLE:
                idle();
                break;
            case State.CHASE:
                break;
            case State.ATTACK: 
                break;
        }
    }

    private bool move_right = true;
    private void idle()
    {
        if(sight.get_sees_player())
            state = State.CHASE;
        if (!has_path)
            return;
        if (move_right)
        {
            body.velocity = new Vector2(speed, body.velocity.y);
        }
        else if(!move_right)
        {
            body.velocity = new Vector2(-speed, body.velocity.y);
        }
        if (transform.position.x >= path_point2)
        {
            move_right = false;
        }
        else if (transform.position.x <= path_point1)
        {
            move_right = true;
        }

    }
    private void Flip()
    {
        if (walk_back)
            return;
        if (body.velocity.x < 0 && transform.localScale.x > 0)
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            sight.flip_sight(-1);
        }
        else if (body.velocity.x > 0 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            sight.flip_sight(1);
        }
    }
}

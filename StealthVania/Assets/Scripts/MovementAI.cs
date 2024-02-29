using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyAI : MonoBehaviour
{
    enum State { 
        IDLE, 
        CHASE, 
        ATTACK,
        SEARCH
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
    [SerializeField] private Transform player_pos;

    private bool has_path;

    private void Start()
    {
        if(path_point1 == 0 &&  path_point2 == 0)
        {
            has_path = false;
        }
        else
            has_path = true;
    }
    private bool move_back = false;
    void Update()
    {
        if(!move_back && !sight.get_sees_player())
            flip();

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
                ranged_chase();
                break;
            case State.SEARCH:
                search(); 
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
    private Vector2 last_pos = Vector2.zero;
    private void ranged_chase()
    {
        float separation = player_pos.position.x - transform.position.x;
        int horizontal = Math.Sign(separation);

        if (MathF.Abs(separation) > 5)
            body.velocity = new Vector2(speed * horizontal, body.velocity.y);
        if(!sight.get_sees_player())
        {
            state = State.SEARCH;
            last_pos = player_pos.position;
        }
    }
    private void search()
    {
        float separation = last_pos.x - transform.position.x;
        int horizontal = Math.Sign(separation);

        if (MathF.Abs(separation) > .2f)
            body.velocity = new Vector2(speed * horizontal, body.velocity.y);

        if(transform.position.x == last_pos.x)
        {
            body.velocity = new Vector2(0, body.velocity.y);
            state = State.IDLE;
        }
    }
    private void flip()
    {
        if (body.velocity.x < 0 && transform.localScale.x > 0)
            sight.swap_dir(1);
        else if (body.velocity.x > 0 && transform.localScale.x < 0)
            sight.swap_dir(0);
    }
}

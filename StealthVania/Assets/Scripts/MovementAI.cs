using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MovementAI : MonoBehaviour
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

    [SerializeField] private State state = State.IDLE;
    [SerializeField] private Type type;
    [SerializeField] private float path_point1, path_point2;
    [SerializeField] private Sight sight;
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private Transform player_pos;
    [SerializeField] private LayerMask obstruction;
    [SerializeField] private GameObject attack;

    private bool has_path;
    private IEnumerator coroutine;
    public bool is_ranged = true;

    private void Start()
    {
        is_ranged = type == Type.RANGE;
        if(path_point1 == 0 &&  path_point2 == 0)
        {
            has_path = false;
        }
        else
            has_path = true;
    }
    private bool move_back = false;
    private int wall_dir = 0;
    void Update()
    {
        if(stop) return;

        wall_dir = wallcheck();
        if(!move_back && !sight.get_sees_player() && MathF.Abs(body.velocity.x) > .001f)
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
                melee_chase();
                break;
            case State.SEARCH:
                search();
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
            if(wall_dir > 0)
                move_right = false;
            else
                body.velocity = new Vector2(accelerate(1), body.velocity.y);
        }
        else if(!move_right)
        {
            if (wall_dir < 0)
                move_right = true;
            else
                body.velocity = new Vector2(accelerate(-1), body.velocity.y);
        }
        if (transform.position.x > path_point2)
        {
            move_right = false;
        }
        else if (transform.position.x < path_point1)
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
            body.velocity = new Vector2(accelerate(horizontal), body.velocity.y);
        else
            body.velocity = new Vector2(0, body.velocity.y);
        if (!sight.get_sees_player())
        {
            last_pos = player_pos.position;
            state = State.SEARCH;
        }
    }
    private void melee_chase()
    {
        float separation = player_pos.position.x - transform.position.x;
        int horizontal = Math.Sign(separation);

        if (MathF.Abs(separation) > 2)
            body.velocity = new Vector2(accelerate(horizontal), body.velocity.y);
        else if (player_pos.position.y <= transform.position.y+1) 
        {
            body.velocity = new Vector2(0, body.velocity.y);
            Instantiate(attack, new Vector2(transform.position.x+.5f*MathF.Sign(transform.localScale.x), transform.position.y+.5f), new Quaternion(0, 0, MathF.Sign(transform.localScale.x)-1, 0));
            sight.delay(.3f);
            cancel(.3f);
        }
        if (!sight.get_sees_player())
        {
            last_pos = player_pos.position;
            state = State.SEARCH;
        }
    }
    private void search()
    {
        float separation = last_pos.x - transform.position.x;
        int horizontal = Math.Sign(separation);


        if (MathF.Abs(separation) > .2f)
            body.velocity = new Vector2(accelerate(horizontal), body.velocity.y);

        if(MathF.Abs(separation) < .2f || horizontal == wall_dir)
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

    private Ray2D front, back;
    private RaycastHit2D hit;
    private Vector2 back_angle = new Vector2(-1, 0);
    private Vector2 forward_angle = new Vector2(1, 0);

    private int wallcheck()
    {
        front = new Ray2D(new Vector2(transform.position.x, transform.position.y), forward_angle);
        back = new Ray2D(new Vector2(transform.position.x, transform.position.y), back_angle);

        hit = Physics2D.Raycast(front.origin, front.direction , .7f, obstruction);

        if (hit)
        {
            body.velocity = new Vector2(accelerate(-1), body.velocity.y);
            return 1;
        }

        hit = Physics2D.Raycast(back.origin, back_angle, .7f, obstruction);

        if (hit)
        {
            body.velocity = new Vector2(accelerate(1), body.velocity.y);
            return -1;
        }
        return 0;
    }
    private float acceleration = .05f;
    private float accelerate(int dir)
    {
        float cur_speed = body.velocity.x;

        if (Mathf.Abs(cur_speed) < speed)
        {
            cur_speed = cur_speed + acceleration * dir;
            acceleration +=.01f;
        }
        else if (Math.Sign(cur_speed) == dir)
        {
            acceleration = .05f;
            cur_speed = speed * dir;
        }

        return cur_speed;
    }
    private bool stop = false;
    private IEnumerator pause(float time)
    {
        stop = true;
        yield return new WaitForSeconds(time);
        stop = false;
    }
    public void cancel(float  time)
    {
        coroutine = pause(time);
        StartCoroutine(coroutine);
    }
}

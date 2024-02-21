using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;



public class Sight : MonoBehaviour
{
    enum State
    {
        IDLE,
        SCANNING,
        LOCKED_ON,
        SEARCH,
        SHOOT
    }

    Ray2D ray;
    RaycastHit2D hit;
    [SerializeField] private Collider2D cone;
    private float angle = -11.303f * Mathf.PI/180;
    private float base_angle = -11.303f * Mathf.PI / 180;

    public LayerMask hittable;
    public LayerMask play_layer;

    private State state = State.IDLE;
    private GameObject player;
    private bool sees_player = false;
    private bool paused = false;

    // Start is called before the first frame update
    void Start()
    {

        ray = new Ray2D(new Vector2(transform.position.x, transform.position.y+.5f), new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)));
        UnityEngine.Debug.Log(ray.direction);
    }
    private void Update()
    {
        if (state == State.LOCKED_ON)
            sees_player = true;
        else
            sees_player = false;
        switch (state)
        {
            case State.IDLE:
                idle();
                break;
            case State.SCANNING:
                checkforObject(); 
                break;
            case State.LOCKED_ON:
                lock_on();
                break;
            case State.SEARCH:
                search();
                break;
            case State.SHOOT:
                aim();
                break;
        }
    }
    
    private void idle()
    {
        ray = new Ray2D(new Vector2(transform.position.x, transform.position.y + .5f), ray.direction);

        if (player_in_range())
            state = State.SCANNING;
        Vector2 test = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        UnityEngine.Debug.DrawRay(ray.origin, ray.direction, Color.red);
    }

    private void checkforObject()
    {
        ray = new Ray2D(new Vector2(transform.position.x, transform.position.y + .5f), new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)));
        hit = Physics2D.Raycast(ray.origin, ray.direction, 10, hittable);

       if(hit && hit.collider.gameObject.name == "Player")
        {
            player = hit.collider.gameObject;
            state = State.LOCKED_ON;
        }
        if (!player_in_range())
            state = State.IDLE;

        if (player_in_range())
            angle += .1f;

       if(angle > base_angle + 23f * Mathf.PI / 180)
        {
            angle = base_angle;
        }

        UnityEngine.Debug.DrawRay(ray.origin, ray.direction, Color.red);
    }

    Ray2D lock_line;
    RaycastHit2D Target;
    Vector2 new_origin;
    Vector2 diff;
    float cone_angle;

    private IEnumerator coroutine;
    private float lock_time = .5f;
    private void lock_on()
    {
        coroutine = pause(lock_time);
        if(!started)
            StartCoroutine(coroutine);

        new_origin = new Vector2(transform.position.x, transform.position.y + .5f);
        diff = new Vector2(player.transform.position.x - new_origin.x, player.transform.position.y - new_origin.y);

        lock_line = new Ray2D(new_origin, diff);

        cone_angle = Mathf.Acos(lock_line.direction.x) / Mathf.PI * 180;

        if (diff.y > 0)
        {
            if(cone_angle < Mathf.PI/2)
                cone.transform.rotation = Quaternion.Euler(0, 0, cone_angle);
        }
        Target = Physics2D.Raycast(lock_line.origin, lock_line.direction, 10, hittable);

        if (!player_in_range() || (Target && Target.collider.gameObject.name != "Player"))
            state = State.SEARCH;

        UnityEngine.Debug.DrawRay(lock_line.origin, lock_line.direction, Color.green);
        UnityEngine.Debug.DrawRay(lock_line.origin, new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)), Color.red);
        base_angle = (-11.303f+cone_angle) * Mathf.PI / 180;

        if (paused)
        {
            //state = State.SHOOT;
            started = false;
        }
    }

    bool top_hit = false, bottom_hit = false, right = false, repeat = false;
    private void search()
    {
        cone.transform.rotation = Quaternion.Euler(0, 0, cone_angle);
        ray = new Ray2D(new_origin, new Vector2(Mathf.Cos(cone_angle * Mathf.PI / 180), Mathf.Sin(cone_angle * Mathf.PI / 180)));
        hit = Physics2D.Raycast(ray.origin, ray.direction, 10, hittable);

        if (hit && hit.collider.gameObject.name == "Player")
        {
            state = State.LOCKED_ON;
        }

        if (!repeat && cone_angle < 90)
        {
            right = true;
            repeat = true;
        }
        else if(!repeat)
        {
            right = false;
            repeat = true;
        }
            
        if (right)
        {
            right_search();
        }
        else if (!right)
        {
            left_search();
        }
        UnityEngine.Debug.DrawRay(transform.position, new Vector2(Mathf.Cos(cone_angle*Mathf.PI/180), Mathf.Sin(cone_angle * Mathf.PI / 180)), Color.green);
    }
    private void right_search()
    {
        if(!top_hit)
        {
            cone_angle += .5f;
        }
        else
        {
            cone_angle -= .5f;
        }
        if(cone_angle >= 90)
            top_hit = true;
        else if(cone_angle <= 0)
            bottom_hit = true;
        if (top_hit && bottom_hit)
        {
            state = State.IDLE;
            cone.transform.rotation = Quaternion.Euler(0, 0, 0);
            repeat = false;
            top_hit=false;
            bottom_hit=false;
            base_angle = (-11.303f + cone_angle) * Mathf.PI / 180;
        }
    }
    private void left_search()
    {
        if (!top_hit)
        {
            cone_angle -= .5f;
        }
        else
        {
            cone_angle += .5f;
        }

        if (cone_angle <= 90)
            top_hit = true;
        else if (cone_angle >= 180)
            bottom_hit = true;

        if (top_hit && bottom_hit)
        {
            state = State.IDLE;
            cone.transform.rotation = Quaternion.Euler(0, 0, 180);
            repeat = false;
            top_hit = false;
            bottom_hit = false;
            base_angle = (-11.303f + 180) * Mathf.PI / 180;
        }
    }
    private float aim_time = 1f;
    private void aim()
    {
        hit = Physics2D.Raycast(lock_line.origin, lock_line.direction, 10, hittable);

        coroutine = pause(aim_time);
        if(!started)
            StartCoroutine(coroutine);

        if (!paused)
        {
            state = State.SEARCH;
            started = false;

            if (hit && hit.collider.gameObject.name == "Player")
            {
                UnityEngine.Debug.Log("Player was shot");
            }
        }
        UnityEngine.Debug.DrawRay(lock_line.origin, lock_line.direction, Color.black);
    }
    private bool started = false;
    private IEnumerator pause(float time)
    {
        started = true;
        yield return new WaitForSeconds(time);
        paused = !paused;
    }

    private bool player_in_range()
    {
        if (cone.IsTouchingLayers(play_layer))
        {
            return true;
        }
        return false;
    }
    public bool get_sees_player()
    {
        return sees_player;
    }
    public void flip_sight(int dir)
    {
        cone.transform.rotation = Quaternion.Euler(0, 0, 180*dir);
        if(dir < 0 && base_angle < Math.PI/2)
        {
            base_angle += Mathf.PI;
            angle += Mathf.PI;
        }
        else if (dir > 0 && base_angle > Math.PI / 2)
        {
            base_angle -= Mathf.PI;
            angle -= Mathf.PI;
        }
    }
}

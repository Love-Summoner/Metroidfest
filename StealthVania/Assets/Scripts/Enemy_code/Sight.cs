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

    public bool is_smart = false;

    private State state = State.IDLE;
    private GameObject player;
    [SerializeField] private PlayerScript player_script;
    private bool sees_player = false;
    private bool paused = false;
    private bool stop = false;
    [SerializeField] private MovementAI movement;
    [SerializeField] private bool starts_backwards = false;
    [SerializeField] private LineRenderer line;
    [SerializeField] private float offset;

    // Start is called before the first frame update
    void Start()
    {
        if (starts_backwards)
            swap_dir(1);
        ray = new Ray2D(new Vector2(transform.position.x, transform.position.y + offset), new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)));
    }
    private void Update()
    {
        if(stop) return;

        flip();

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
        ray = new Ray2D(new Vector2(transform.position.x, transform.position.y + offset), ray.direction);
        line.SetWidth(.0f, .0f);

        if (player_in_range())
            state = State.SCANNING;
        Vector2 test = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        UnityEngine.Debug.DrawRay(ray.origin, ray.direction, Color.red);
    }

    private void checkforObject()
    {
        line.SetWidth(.0f, .0f);
        ray = new Ray2D(new Vector2(transform.position.x, transform.position.y + offset), new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)));
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
    float cone_angle = 0;

    private IEnumerator coroutine;
    private float lock_time = 1f;
    private bool started = false;
    private void lock_on()
    {
        if (movement.is_ranged)
        {
            line.SetColors(Color.green, Color.green);
            line.SetWidth(.01f, .01f);
            line.SetPosition(0, new Vector2(player.transform.position.x, player.transform.position.y + .5f));
            line.SetPosition(1, new Vector2(transform.position.x, transform.position.y + .5f));
        }
        if (!started && movement.is_ranged)
        {
            coroutine = pause(lock_time);
            StartCoroutine(coroutine);
            started = true;
        }

        new_origin = new Vector2(transform.position.x, transform.position.y + offset);
        diff = new Vector2(player.transform.position.x - new_origin.x, player.transform.position.y+.35f - new_origin.y);

        lock_line = new Ray2D(new_origin, diff);

        cone_angle = Mathf.Acos(lock_line.direction.x) / Mathf.PI * 180;

        cone.transform.rotation = Quaternion.Euler(0, 0, cone_angle);

        Target = Physics2D.Raycast(lock_line.origin, lock_line.direction, 10, hittable);

        if (!player_in_range() || (Target && Target.collider.gameObject.name != "Player"))
        {
            state = State.SEARCH;
        }

        UnityEngine.Debug.DrawRay(lock_line.origin, lock_line.direction, Color.green);
        UnityEngine.Debug.DrawRay(lock_line.origin, new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)), Color.red);
        base_angle = (-11.303f+cone_angle) * Mathf.PI / 180;

        if (!paused && movement.is_ranged)
        {
            started = false;
            state = State.SHOOT;        }
    }

    bool top_hit = false, bottom_hit = false, right = false, repeat = false;
    private void search()
    {
        new_origin = new Vector2(transform.position.x, transform.position.y + offset);
        cone.transform.rotation = Quaternion.Euler(0, 0, cone_angle);
        ray = new Ray2D(new_origin, new Vector2(Mathf.Cos(cone_angle * Mathf.PI / 180), Mathf.Sin(cone_angle * Mathf.PI / 180)));
        hit = Physics2D.Raycast(ray.origin, ray.direction, 10, hittable);
        line.SetWidth(.0f, .0f);

       
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
        UnityEngine.Debug.DrawRay(new_origin, new Vector2(Mathf.Cos(cone_angle*Mathf.PI/180), Mathf.Sin(cone_angle * Mathf.PI / 180)), Color.green);
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
        hit = Physics2D.Raycast(lock_line.origin, lock_line.direction, 50, hittable);
        line.SetColors(Color.red, Color.red);
        line.SetWidth(.01f, .01f);
        line.SetPosition(0, new Vector2(hit.point.x, hit.point.y));
        line.SetPosition(1, new Vector2(transform.position.x, transform.position.y + .5f));


        if (!started)
        {
            coroutine = pause(aim_time);
            movement.cancel(aim_time);
            StartCoroutine(coroutine);
            started = true;
            movement.cancel(aim_time);
}

        UnityEngine.Debug.DrawRay(lock_line.origin, lock_line.direction, Color.black);

        if (paused)
            return;

        line.SetWidth(.02f, .02f);

        if (player_in_range())
            state = State.LOCKED_ON;
        else
        {
            state = State.SEARCH;
            if (is_smart)
                swap_dir(Math.Sign(transform.localScale.x));
        }
        started = false;

        if (hit && hit.collider.gameObject.name == "Player")
        {
            player_script.takeDamage();
        }
    }

    private IEnumerator pause(float time)
    {
        paused = true;
        yield return new WaitForSeconds(time);
        paused = false;
    }

    public bool player_in_range()
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
    private void flip()
    {
        if (cone_angle < 90 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector2(transform.localScale.x*-1, transform.localScale.y);
            cone.transform.localScale = new Vector2(Mathf.Abs(cone.transform.localScale.x), cone.transform.localScale.y);
        }
        else if(cone_angle > 90 && transform.localScale.x > 0)
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            cone.transform.localScale = new Vector2(cone.transform.localScale.x * -1, cone.transform.localScale.y);
        }
    }
    public void swap_dir(int dir)
    {
        if(Math.Sign(dir) < 0)
        {
            dir = 0;
        }
        cone_angle = 180 * dir;
        cone.transform.rotation = Quaternion.Euler(0, 0, cone_angle);
        base_angle = (180 * dir - 11.303f) * Mathf.PI / 180;
    }
    public void ChangeState(int choice)
    {
        switch (choice)
        {
            case 0:
                state = State.IDLE; break;
            case 1:
                state = State.SEARCH; break;
        }
    }


    private IEnumerator blind;
    private IEnumerator wait(float time)
    {
        stop = true;
        yield return new WaitForSeconds(time);
        stop = false;
    }
    public void delay(float time)
    {
        blind = wait(time);
        StartCoroutine(blind);
    }
    public void contact_shoot()
    {

        hit = Physics2D.Raycast(ray.origin, new Vector2(MathF.Cos(cone_angle/180 * Mathf.PI), Mathf.Sin(cone_angle / 180 * Mathf.PI)), 50, hittable);
        line.SetColors(Color.red, Color.red);
        line.SetWidth(.01f, .01f);
        line.SetPosition(0, new Vector2(hit.point.x, hit.point.y));
        line.SetPosition(1, new Vector2(transform.position.x, transform.position.y + .5f));


        if (!started)
        {
            coroutine = pause(aim_time);
            movement.cancel(aim_time);
            StartCoroutine(coroutine);
            started = true;
            movement.cancel(aim_time);
        }

        if (paused)
            return;

        if (player_in_range())
            state = State.LOCKED_ON;
        else
        {
            state = State.SEARCH;
            if (is_smart)
                swap_dir(Math.Sign(transform.localScale.x));
        }
        started = false;

        if (hit && hit.collider.gameObject.name == "Player")
        {
            player_script.takeDamage();
        }
    }
}
